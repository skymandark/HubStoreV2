using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels;
using Core.ViewModels.OrderViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderViewModel> CreateOrder(CreateOrderViewModel orderDto)
        {
            try
            {
                // توليد كود الأمر إذا لم يكن موجوداً
                if (string.IsNullOrEmpty(orderDto.OrderCode))
                {
                    orderDto.OrderCode = await GenerateOrderCode(orderDto.OrderTypeId);
                }

                var order = new Order
                {
                    OrderCode = orderDto.OrderCode,
                    OrderTypeId = orderDto.OrderTypeId,
                    SupplierId = orderDto.SupplierId,
                    BranchFromId = orderDto.BranchFromId,
                    BranchToId = orderDto.BranchToId,
                    RequestedByUserId = orderDto.RequestedByUserId,
                    RequestedDate = orderDto.RequestedDate,
                    PriorityFlag = orderDto.PriorityFlag,
                    SLA_DueDate = orderDto.SLA_DueDate,
                    InternalBarcode = orderDto.InternalBarcode,
                    ExternalBarcode = orderDto.ExternalBarcode,
                    Notes = orderDto.Notes,
                    CreatedBy = orderDto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Draft"
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // إضافة سطور الأمر
                if (orderDto.Lines != null && orderDto.Lines.Any())
                {
                    int lineNo = 1;
                    foreach (var lineDto in orderDto.Lines)
                    {
                        var line = new OrderLine
                        {
                            OrderId = order.OrderId,
                            LineNo = lineNo++,
                            ItemId = lineDto.ItemId,
                            UnitCode = lineDto.UnitCode,
                            QtyOrdered = lineDto.QtyOrdered,
                            QtyReceived = 0,
                            ConversionUsedToBase = lineDto.ConversionUsedToBase,
                            QtyBaseOrdered = lineDto.QtyOrdered * lineDto.ConversionUsedToBase,
                            QtyBaseReceived = 0,
                            UnitPrice = lineDto.UnitPrice,
                            CostValue = lineDto.QtyOrdered * lineDto.UnitPrice,
                            TaxRate = lineDto.TaxRate,
                            LineStatus = "Draft",
                            Notes = lineDto.Notes,
                            CreatedBy = lineDto.CreatedBy,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.OrderLines.Add(line);
                    }
                    await _context.SaveChangesAsync();
                }

                // حساب الإجماليات
                await CalculateOrderTotals(order.OrderId);

                return await GetOrder(order.OrderId);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إنشاء الأمر: {ex.Message}");
            }
        }

        public async Task<OrderViewModel> UpdateOrder(int orderId, UpdateOrderViewModel orderDto)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderLines)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    throw new Exception("الأمر غير موجود");

                if (order.Status != "Draft")
                    throw new Exception("لا يمكن تعديل الأمر إلا إذا كان في حالة مسودة");

                order.SupplierId = orderDto.SupplierId;
                order.BranchFromId = orderDto.BranchFromId;
                order.BranchToId = orderDto.BranchToId;
                order.PriorityFlag = orderDto.PriorityFlag;
                order.SLA_DueDate = orderDto.SLA_DueDate;
                order.InternalBarcode = orderDto.InternalBarcode;
                order.ExternalBarcode = orderDto.ExternalBarcode;
                order.Notes = orderDto.Notes;
                order.ModifiedBy = orderDto.ModifiedBy;
                order.ModifiedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return await GetOrder(orderId);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في تحديث الأمر: {ex.Message}");
            }
        }

        public async Task<OrderViewModel> GetOrder(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderType)
                .Include(o => o.Supplier)
                .Include(o => o.BranchFrom)
                .Include(o => o.BranchTo)
                .Include(o => o.OrderLines)
                    .ThenInclude(l => l.Item)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new Exception("الأمر غير موجود");

            // حساب الإجماليات مباشرة من السطور
            decimal totalQuantityBase = order.OrderLines.Sum(l => l.QtyBaseOrdered);
            decimal totalValueCost = order.OrderLines.Sum(l => l.CostValue);

            // تحديث القيم في الكيان إذا كانت مختلفة (اختياري)
            if (order.TotalQuantityBase != totalQuantityBase ||
                order.TotalValueCost != totalValueCost)
            {
                order.TotalQuantityBase = totalQuantityBase;
                order.TotalValueCost = totalValueCost;
                await _context.SaveChangesAsync();
            }

            var orderViewModel = new OrderViewModel
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                OrderTypeId = order.OrderTypeId,
                OrderTypeName = order.OrderType?.Name,
                SupplierId = order.SupplierId,
                SupplierName = order.Supplier?.Name,
                BranchFromId = order.BranchFromId,
                BranchFromName = order.BranchFrom?.Name,
                BranchToId = order.BranchToId,
                BranchToName = order.BranchTo?.Name,
                RequestedByUserId = order.RequestedByUserId,
                RequestedDate = order.RequestedDate,
                Status = order.Status,
                TotalQuantityBase = totalQuantityBase,
                TotalValueCost = totalValueCost,
                PriorityFlag = order.PriorityFlag,
                SLA_DueDate = order.SLA_DueDate,
                InternalBarcode = order.InternalBarcode,
                ExternalBarcode = order.ExternalBarcode,
                Notes = order.Notes,
                Lines = order.OrderLines.Select(line => new OrderLineViewModel
                {
                    OrderLineId = line.OrderLineId,
                    OrderId = line.OrderId,
                    LineNo = line.LineNo,
                    ItemId = line.ItemId,
                    ItemCode = line.Item?.ItemCode,
                    UnitCode = line.UnitCode,
                    QtyOrdered = line.QtyOrdered,
                    QtyReceived = line.QtyReceived,
                    ConversionUsedToBase = line.ConversionUsedToBase,
                    QtyBaseOrdered = line.QtyBaseOrdered,
                    QtyBaseReceived = line.QtyBaseReceived,
                    UnitPrice = line.UnitPrice,
                    CostValue = line.CostValue,
                    TaxRate = line.TaxRate,
                    LineStatus = line.LineStatus,
                    Notes = line.Notes
                }).ToList()
            };

            return orderViewModel;
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrders(OrderFilterViewModel filters, PaginationViewModel pagination)
        {
            var query = _context.Orders
                .Include(o => o.OrderType)
                .Include(o => o.Supplier)
                .Include(o => o.BranchFrom)
                .Include(o => o.BranchTo)
                .AsQueryable();

            // تطبيق الفلاتر
            if (!string.IsNullOrEmpty(filters.OrderCode))
                query = query.Where(o => o.OrderCode.Contains(filters.OrderCode));

            if (filters.OrderTypeId.HasValue)
                query = query.Where(o => o.OrderTypeId == filters.OrderTypeId);

            if (filters.SupplierId.HasValue)
                query = query.Where(o => o.SupplierId == filters.SupplierId);

            if (filters.DateFrom.HasValue)
                query = query.Where(o => o.RequestedDate >= filters.DateFrom);

            if (filters.DateTo.HasValue)
                query = query.Where(o => o.RequestedDate <= filters.DateTo);

            if (!string.IsNullOrEmpty(filters.Status))
                query = query.Where(o => o.Status == filters.Status);

            if (!string.IsNullOrEmpty(filters.Barcode))
                query = query.Where(o => o.InternalBarcode == filters.Barcode || o.ExternalBarcode == filters.Barcode);

            // التصفح
            query = query.OrderByDescending(o => o.RequestedDate)
                        .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                        .Take(pagination.PageSize);

            var orders = await query.ToListAsync();

            var result = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                result.Add(new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderCode = order.OrderCode,
                    OrderTypeId = order.OrderTypeId,
                    OrderTypeName = order.OrderType?.Name,
                    SupplierId = order.SupplierId,
                    SupplierName = order.Supplier?.Name,
                    BranchFromId = order.BranchFromId,
                    BranchFromName = order.BranchFrom?.Name,
                    BranchToId = order.BranchToId,
                    BranchToName = order.BranchTo?.Name,
                    RequestedByUserId = order.RequestedByUserId,
                    RequestedDate = order.RequestedDate,
                    Status = order.Status,
                    TotalQuantityBase = order.TotalQuantityBase,
                    TotalValueCost = order.TotalValueCost,
                    PriorityFlag = order.PriorityFlag,
                    SLA_DueDate = order.SLA_DueDate,
                    InternalBarcode = order.InternalBarcode,
                    ExternalBarcode = order.ExternalBarcode,
                    Notes = order.Notes,
                    Lines = new List<OrderLineViewModel>()
                });
            }

            return result;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderLines)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    throw new Exception("الأمر غير موجود");

                if (order.Status != "Draft")
                    throw new Exception("لا يمكن حذف الأمر إلا إذا كان في حالة مسودة");

                // حذف السطور أولاً
                _context.OrderLines.RemoveRange(order.OrderLines);

                // حذف الأمر
                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في حذف الأمر: {ex.Message}");
            }
        }

        public async Task<bool> SubmitOrder(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderLines)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    throw new Exception("الأمر غير موجود");

                if (order.Status != "Draft")
                    throw new Exception("لا يمكن إرسال الأمر إلا إذا كان في حالة مسودة");

                if (!order.OrderLines.Any())
                    throw new Exception("لا يمكن إرسال أمر بدون سطور");

                order.Status = "Pending";
                order.CreatedAt = DateTime.UtcNow;
                order.CreatedBy = "System";

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إرسال الأمر: {ex.Message}");
            }
        }

        public async Task<bool> CloseOrderManually(int orderId, string reason)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);

                if (order == null)
                    throw new Exception("الأمر غير موجود");

                if (order.Status == "Completed" || order.Status == "Cancelled")
                    throw new Exception("لا يمكن إغلاق أمر مغلق بالفعل");

                order.Status = "Closed";
                order.CreatedAt = DateTime.UtcNow;
                order.CreatedBy = "System";
                order.ClosingReason = reason;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إغلاق الأمر: {ex.Message}");
            }
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrdersByBarcode(string barcode)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderType)
                .Include(o => o.Supplier)
                .Include(o => o.BranchFrom)
                .Include(o => o.BranchTo)
                .Where(o => o.InternalBarcode == barcode || o.ExternalBarcode == barcode)
                .ToListAsync();

            var result = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                result.Add(new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderCode = order.OrderCode,
                    OrderTypeId = order.OrderTypeId,
                    OrderTypeName = order.OrderType?.Name,
                    SupplierId = order.SupplierId,
                    SupplierName = order.Supplier?.Name,
                    BranchFromId = order.BranchFromId,
                    BranchFromName = order.BranchFrom?.Name,
                    BranchToId = order.BranchToId,
                    BranchToName = order.BranchTo?.Name,
                    RequestedByUserId = order.RequestedByUserId,
                    RequestedDate = order.RequestedDate,
                    Status = order.Status,
                    TotalQuantityBase = order.TotalQuantityBase,
                    TotalValueCost = order.TotalValueCost,
                    PriorityFlag = order.PriorityFlag,
                    SLA_DueDate = order.SLA_DueDate,
                    InternalBarcode = order.InternalBarcode,
                    ExternalBarcode = order.ExternalBarcode,
                    Notes = order.Notes,
                    Lines = new List<OrderLineViewModel>()
                });
            }

            return result;
        }

        private async Task CalculateOrderTotals(int orderId)
        {
            var lines = await _context.OrderLines
                .Where(l => l.OrderId == orderId)
                .ToListAsync();

            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.TotalQuantityBase = lines.Sum(l => l.QtyBaseOrdered);
                order.TotalValueCost = lines.Sum(l => l.CostValue);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<string> GenerateOrderCode(int orderTypeId)
        {
            var orderType = await _context.OrderTypes.FindAsync(orderTypeId);
            var prefix = orderType?.Code ?? "ORD";
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = await _context.Orders
                .CountAsync(o => o.OrderTypeId == orderTypeId &&
                                o.CreatedAt.Date == DateTime.Today) + 1;

            return $"{prefix}-{date}-{count:D4}";
        }
    }
}