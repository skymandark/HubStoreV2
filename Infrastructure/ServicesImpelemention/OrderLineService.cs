using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.OrderViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class OrderLineService : IOrderLineService
    {
        private readonly ApplicationDbContext _context;

        public OrderLineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderLineViewModel> AddOrderLine(int orderId, CreateOrderLineViewModel lineDto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                    throw new Exception("الأمر غير موجود");

                if (order.Status != "Draft")
                    throw new Exception("لا يمكن إضافة سطور إلا لأمر في حالة مسودة");

                // التحقق من عدم تكرار رقم السطر
                var existingLineNo = await _context.OrderLines
                    .AnyAsync(l => l.OrderId == orderId && l.LineNo == lineDto.LineNo);

                if (existingLineNo)
                    throw new Exception($"رقم السطر {lineDto.LineNo} موجود بالفعل");

                var line = new OrderLine
                {
                    OrderId = orderId,
                    LineNo = lineDto.LineNo,
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
                await _context.SaveChangesAsync();

                // تحديث إجماليات الأمر
                await UpdateOrderTotals(orderId);

                return await GetOrderLineViewModel(line.OrderLineId);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إضافة سطر الأمر: {ex.Message}");
            }
        }

        public async Task<OrderLineViewModel> UpdateOrderLine(int lineId, UpdateOrderLineViewModel lineDto)
        {
            try
            {
                var line = await _context.OrderLines
                    .Include(l => l.Order)
                    .FirstOrDefaultAsync(l => l.OrderLineId == lineId);

                if (line == null)
                    throw new Exception("سطر الأمر غير موجود");

                if (line.Order.Status != "Draft")
                    throw new Exception("لا يمكن تعديل سطور أمر إلا إذا كان الأمر في حالة مسودة");

                line.QtyOrdered = lineDto.QtyOrdered;
                line.QtyBaseOrdered = lineDto.QtyOrdered * line.ConversionUsedToBase;
                line.UnitPrice = lineDto.UnitPrice;
                line.CostValue = lineDto.QtyOrdered * lineDto.UnitPrice;
                line.TaxRate = lineDto.TaxRate;
                line.Notes = lineDto.Notes;
                line.CreatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // تحديث إجماليات الأمر
                await UpdateOrderTotals(line.OrderId);

                return await GetOrderLineViewModel(lineId);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في تحديث سطر الأمر: {ex.Message}");
            }
        }

        public async Task<bool> DeleteOrderLine(int lineId)
        {
            try
            {
                var line = await _context.OrderLines
                    .Include(l => l.Order)
                    .FirstOrDefaultAsync(l => l.OrderLineId == lineId);

                if (line == null)
                    throw new Exception("سطر الأمر غير موجود");

                if (line.Order.Status != "Draft")
                    throw new Exception("لا يمكن حذف سطور أمر إلا إذا كان الأمر في حالة مسودة");

                _context.OrderLines.Remove(line);
                await _context.SaveChangesAsync();

                // تحديث إجماليات الأمر
                await UpdateOrderTotals(line.OrderId);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في حذف سطر الأمر: {ex.Message}");
            }
        }

        public async Task<IEnumerable<OrderLineViewModel>> GetOrderLines(int orderId)
        {
            var lines = await _context.OrderLines
                .Include(l => l.Item)
                .Where(l => l.OrderId == orderId)
                .OrderBy(l => l.LineNo)
                .ToListAsync();

            return lines.Select(line => new OrderLineViewModel
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
            }).ToList();
        }

        public async Task<bool> RecordPartialReceive(int lineId, decimal qtyReceived, int movementId)
        {
            try
            {
                var line = await _context.OrderLines
                    .Include(l => l.Order)
                    .FirstOrDefaultAsync(l => l.OrderLineId == lineId);

                if (line == null)
                    throw new Exception("سطر الأمر غير موجود");

                if (line.Order.Status != "InProgress")
                    throw new Exception("لا يمكن تسجيل استلام إلا لأمر قيد التنفيذ");

                if (qtyReceived <= 0)
                    throw new Exception("الكمية المستلمة يجب أن تكون أكبر من صفر");

                if (line.QtyReceived + qtyReceived > line.QtyOrdered)
                    throw new Exception("الكمية المستلمة تتجاوز الكمية المطلوبة");

                var qtyBaseReceived = qtyReceived * line.ConversionUsedToBase;

                line.QtyReceived += qtyReceived;
                line.QtyBaseReceived += qtyBaseReceived;

                // تحديث حالة السطر
                if (line.QtyReceived >= line.QtyOrdered)
                {
                    line.LineStatus = "Completed";
                }
                else
                {
                    line.LineStatus = "PartiallyReceived";
                }

                // تسجيل حركة الاستلام
                var receipt = new Receipt
                {
                    OrderLineId = lineId,
                    MovementId = movementId,
                    QuantityReceived = qtyReceived,
                    QuantityBaseReceived = qtyBaseReceived,
                    ReceivedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };
                _context.Receipts.Add(receipt);

                await _context.SaveChangesAsync();

                // التحقق من حالة الأمر الكاملة
                await CheckOrderCompletion(line.OrderId);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في تسجيل الاستلام الجزئي: {ex.Message}");
            }
        }

        private async Task<OrderLineViewModel> GetOrderLineViewModel(int lineId)
        {
            var line = await _context.OrderLines
                .Include(l => l.Item)
                .FirstOrDefaultAsync(l => l.OrderLineId == lineId);

            if (line == null)
                throw new Exception("سطر الأمر غير موجود");

            return new OrderLineViewModel
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
            };
        }

        private async Task UpdateOrderTotals(int orderId)
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

        private async Task CheckOrderCompletion(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order != null && order.OrderLines.All(l => l.LineStatus == "Completed"))
            {
                order.Status = "Completed";
                order.CreatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }

}