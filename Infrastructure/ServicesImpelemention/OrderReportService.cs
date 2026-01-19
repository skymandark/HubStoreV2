using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.ReportingServices;
using Core.ViewModels.ReportViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class OrderReportService : IOrderReportService
    {
        private readonly ApplicationDbContext _context;

        public OrderReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderStatusReportViewModel>> GetOrderStatusReport(
            ReportFilterViewModel filters)
        {
            var query = from order in _context.Orders
                        join orderType in _context.OrderTypes on order.OrderTypeId equals orderType.OrderTypeId
                        join status in _context.ApprovalStatuses on order.ApprovalStatusId equals status.ApprovalStatusId
                        where (filters.BranchId == null ||
                               order.BranchFromId == filters.BranchId ||
                               order.BranchToId == filters.BranchId) &&
                              (filters.SupplierId == null || order.SupplierId == filters.SupplierId) &&
                              (filters.DateFrom == null || order.RequestedDate >= filters.DateFrom) &&
                              (filters.DateTo == null || order.RequestedDate <= filters.DateTo) &&
                              (string.IsNullOrEmpty(filters.Status) || status.Name == filters.Status)
                        select new
                        {
                            order,
                            orderType,
                            status,
                            Lines = _context.OrderLines
                                .Where(ol => ol.OrderId == order.OrderId)
                                .Include(ol => ol.Item)
                                .ToList()
                        };

            var orders = await query.ToListAsync();

            return orders.Select(o => new OrderStatusReportViewModel
            {
                OrderId = o.order.OrderId,
                OrderCode = o.order.OrderCode,
                OrderType = o.orderType.Name,
                OrderDate = o.order.RequestedDate,
                Status = o.status.Name,
                TotalQuantity = o.Lines.Sum(l => l.QtyOrdered),
                TotalValue = o.Lines.Sum(l => l.QtyOrdered * l.UnitPrice),
                DueDate = o.order.SLA_DueDate
            }).OrderByDescending(o => o.OrderDate).ToList();
        }

        public async Task<List<PendingOrderReportViewModel>> GetPendingOrdersReport(
            ReportFilterViewModel filters)
        {
            var orders = await GetOrderStatusReport(filters);
            var currentDate = DateTime.UtcNow;

            return orders
                .Where(o => o.Status == "Pending" || o.Status == "Processing")
                .Select(o => new PendingOrderReportViewModel
                {
                    OrderId = o.OrderId,
                    OrderCode = o.OrderCode,
                    OrderType = o.OrderType,
                    DaysPending = (int)(currentDate - o.OrderDate).TotalDays,
                    TotalQuantity = o.TotalQuantity,
                    PriorityScore = CalculatePriorityScore(o, currentDate)
                })
                .OrderByDescending(o => o.PriorityScore)
                .ThenByDescending(o => o.DaysPending)
                .ToList();
        }

        private int CalculatePriorityScore(OrderStatusReportViewModel order, DateTime currentDate)
        {
            int score = 0;

            int daysPending = (int)(currentDate - order.OrderDate).TotalDays;
            score += daysPending * 2;

            if (order.DueDate.HasValue && order.DueDate.Value < currentDate)
                score += 10;

            if (order.TotalValue > 10000)
                score += 5;
            else if (order.TotalValue > 5000)
                score += 3;

            if (order.OrderType == "Urgent")
                score += 15;

            return score;
        }

        public async Task<List<OrderFulfillmentReportViewModel>> GetOrderFulfillmentReport(
            ReportFilterViewModel filters)
        {
            var orders = await GetOrderStatusReport(filters);

            var result = new List<OrderFulfillmentReportViewModel>();

            foreach (var order in orders)
            {
                var receivedQuantity = await CalculateReceivedQuantity(order.OrderId);
                var fulfillmentPercentage = order.TotalQuantity > 0 ?
                    (receivedQuantity / order.TotalQuantity) * 100 : 0;

                result.Add(new OrderFulfillmentReportViewModel
                {
                    OrderId = order.OrderId,
                    OrderCode = order.OrderCode,
                    QuantityOrdered = order.TotalQuantity,
                    QuantityReceived = receivedQuantity,
                    FulfillmentPercentage = fulfillmentPercentage,
                    CompletionDate = order.Status == "Completed" ? order.OrderDate : (DateTime?)null
                });
            }

            return result.Where(o => o.FulfillmentPercentage < 100).ToList();
        }

        private async Task<decimal> CalculateReceivedQuantity(int orderId)
        {
            // الحصول على مجموع QtyReceived من OrderLines
            var totalReceived = await _context.OrderLines
                .Where(ol => ol.OrderId == orderId)
                .SumAsync(ol => ol.QtyReceived);

            return totalReceived;
        }

        public async Task<List<SLAComplianceReportViewModel>> GetSLAComplianceReport(
            ReportFilterViewModel filters)
        {
            var orders = await GetOrderStatusReport(filters);
            var currentDate = DateTime.UtcNow;

            return orders.Select(o => new SLAComplianceReportViewModel
            {
                OrderId = o.OrderId,
                OrderCode = o.OrderCode,
                SLADueDate = CalculateSLADueDate(o),
                ActualCompletionDate = o.Status == "Completed" ? o.OrderDate : (DateTime?)null, // تصحيح: o.OrderDate
                IsCompliant = IsSLACompliant(o, currentDate),
                DaysVariance = CalculateDaysVariance(o, currentDate)
            }).ToList();
        }

        private DateTime CalculateSLADueDate(OrderStatusReportViewModel order)
        {
            int slaDays = order.OrderType == "Urgent" ? 2 : 5;
            return order.OrderDate.AddDays(slaDays);
        }

        private bool IsSLACompliant(OrderStatusReportViewModel order, DateTime currentDate)
        {
            var slaDueDate = CalculateSLADueDate(order);

            if (order.Status != "Completed")
                return slaDueDate >= currentDate;

            return order.OrderDate <= slaDueDate;
        }

        private int CalculateDaysVariance(OrderStatusReportViewModel order, DateTime currentDate)
        {
            var slaDueDate = CalculateSLADueDate(order);
            var actualDate = order.Status == "Completed" ? order.OrderDate : currentDate;

            return (int)(slaDueDate - actualDate).TotalDays;
        }
    }
}