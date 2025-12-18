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
    public class TransferOrderService : ITransferOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;

        public TransferOrderService(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<OrderViewModel> CreateTO(CreateTransferOrderViewModel toDto)
        {
            try
            {
                // تأكد من أن نوع الأمر هو أمر تحويل
                var orderType = await _context.OrderTypes
                    .FirstOrDefaultAsync(ot => ot.Code == "TO");

                if (orderType == null)
                    throw new Exception("نوع أمر التحويل غير موجود");

                toDto.OrderTypeId = orderType.OrderTypeId;

                // التحقق من توفر المخزون لكل عنصر
                foreach (var line in toDto.Lines)
                {
                    var availableQty = await CheckItemAvailability(line.ItemId, toDto.BranchFromId.Value,
                        line.QtyOrdered * line.ConversionUsedToBase);

                    if (!availableQty)
                        throw new Exception($"الرصيد غير كافٍ للصنف {line.ItemId} في الفرع المصدر");
                }

                // إنشاء الأمر
                var orderDto = new CreateOrderViewModel
                {
                    OrderCode = toDto.OrderCode,
                    OrderTypeId = toDto.OrderTypeId,
                    BranchFromId = toDto.BranchFromId,
                    BranchToId = toDto.BranchToId,
                    RequestedByUserId = toDto.RequestedByUserId,
                    RequestedDate = toDto.RequestedDate,
                    PriorityFlag = toDto.PriorityFlag,
                    SLA_DueDate = toDto.SLA_DueDate,
                    InternalBarcode = toDto.InternalBarcode,
                    ExternalBarcode = toDto.ExternalBarcode,
                    Notes = toDto.Notes,
                    CreatedBy = toDto.CreatedBy,
                    Lines = toDto.Lines
                };

                return await _orderService.CreateOrder(orderDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إنشاء أمر التحويل: {ex.Message}");
            }
        }

        public async Task<bool> ProcessTO(int orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderLines)
                    .ThenInclude(l => l.Item)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    throw new Exception("أمر التحويل غير موجود");

                if (order.Status != "Approved")
                    throw new Exception("لا يمكن معالجة أمر تحويل غير معتمد");

                // التحقق من توفر المخزون مرة أخرى
                foreach (var line in order.OrderLines)
                {
                    var availableQty = await CheckItemAvailability(line.ItemId, order.BranchFromId.Value,
                        line.QtyBaseOrdered);

                    if (!availableQty)
                        throw new Exception($"الرصيد غير كافٍ للصنف {line.Item.ItemCode} في الفرع المصدر");
                }

                // إنشاء حركة تحويل خارجي
                var movementOut = new Movement
                {
                    MovementCode = await GenerateTransferCode("TOUT"),
                    MovementTypeId = await GetMovementTypeIdByCode("TOUT"),
                    BranchFromId = order.BranchFromId,
                    BranchToId = order.BranchToId,
                    Date = DateTime.UtcNow,
                    InternalBarcode = Guid.NewGuid().ToString("N").Substring(0, 12),
                    Notes = $"تحويل من أمر {order.OrderCode}",
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    Status = "Completed"
                };
                _context.Movements.Add(movementOut);
                await _context.SaveChangesAsync();

                // إنشاء حركة تحويل داخلي
                var movementIn = new Movement
                {
                    MovementCode = await GenerateTransferCode("TOIN"),
                    MovementTypeId = await GetMovementTypeIdByCode("TOIN"),
                    BranchFromId = order.BranchFromId,
                    BranchToId = order.BranchToId,
                    Date = DateTime.UtcNow,
                    InternalBarcode = Guid.NewGuid().ToString("N").Substring(0, 12),
                    Notes = $"تحويل إلى أمر {order.OrderCode}",
                    CreatedBy = "System",
                    CreatedAt = DateTime.UtcNow,
                    Status = "Completed"
                };
                _context.Movements.Add(movementIn);
                await _context.SaveChangesAsync();

                // معالجة كل سطر
                foreach (var orderLine in order.OrderLines)
                {
                    // سطر الحركة الخارجية (الخصم من الفرع المصدر)
                    var movementLineOut = new MovementLine
                    {
                        MovementId = movementOut.MovementId,
                        ItemId = orderLine.ItemId,
                        BranchId = order.BranchFromId.Value,
                        UnitCode = orderLine.UnitCode,
                        QtyInput = orderLine.QtyOrdered,
                        ConversionUsedToBase = orderLine.ConversionUsedToBase,
                        QtyBase = orderLine.QtyBaseOrdered,
                        UnitPrice = orderLine.UnitPrice,
                        Notes = $"تحويل خارجي - {order.OrderCode}",
                        CreatedBy = "System",
                        CreatedAt = DateTime.UtcNow,
                        Status = "Completed"
                    };
                    _context.MovementLines.Add(movementLineOut);

                    // سطر الحركة الداخلية (الإضافة للفرع الهدف)
                    var movementLineIn = new MovementLine
                    {
                        MovementId = movementIn.MovementId,
                        ItemId = orderLine.ItemId,
                        BranchId = order.BranchToId.Value,
                        UnitCode = orderLine.UnitCode,
                        QtyInput = orderLine.QtyOrdered,
                        ConversionUsedToBase = orderLine.ConversionUsedToBase,
                        QtyBase = orderLine.QtyBaseOrdered,
                        UnitPrice = orderLine.UnitPrice,
                        Notes = $"تحويل داخلي - {order.OrderCode}",
                        CreatedBy = "System",
                        CreatedAt = DateTime.UtcNow,
                        Status = "Completed"
                    };
                    _context.MovementLines.Add(movementLineIn);

                    // تحديث سطر الأمر
                    orderLine.QtyReceived = orderLine.QtyOrdered;
                    orderLine.QtyBaseReceived = orderLine.QtyBaseOrdered;
                    orderLine.LineStatus = "Completed";
                }

                // تحديث حالة الأمر
                order.Status = "Completed";
                order.CreatedAt = DateTime.UtcNow;
                order.CreatedBy = "System";

                // إكمال التحويل بين الحركتين
                var transferService = new TransferService(_context, null);
                await transferService.CompleteTransfer(movementOut.MovementId, movementIn.MovementId);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"خطأ في معالجة أمر التحويل: {ex.Message}");
            }
        }

        private async Task<bool> CheckItemAvailability(int itemId, int branchId, decimal requiredQty)
        {
            try
            {
                // حساب الرصيد المتاح من OpeningBalances والحركات
                var openingBalance = await _context.OpeningBalances
                    .FirstOrDefaultAsync(ob => ob.ItemId == itemId && ob.BranchId == branchId);

                var totalIn = await _context.MovementLines
                    .Include(ml => ml.Movement)
                    .Where(ml => ml.ItemId == itemId &&
                                 ml.BranchId == branchId &&
                                 ml.Movement.Status == "Completed" &&
                                 ml.Movement.MovementType.Code == "TI")
                    .SumAsync(ml => ml.QtyBase);

                var totalOut = await _context.MovementLines
                    .Include(ml => ml.Movement)
                    .Where(ml => ml.ItemId == itemId &&
                                 ml.BranchId == branchId &&
                                 ml.Movement.Status == "Completed" &&
                                 ml.Movement.MovementType.Code == "TO")
                    .SumAsync(ml => ml.QtyBase);

                var availableQty = (openingBalance?.Quantity ?? 0) + totalIn - totalOut;

                return availableQty >= requiredQty;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<string> GenerateTransferCode(string prefix)
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = await _context.Movements
                .CountAsync(m => m.MovementCode.StartsWith(prefix) &&
                                m.CreatedAt.Date == DateTime.Today) + 1;

            return $"{prefix}-{date}-{count:D4}";
        }

        private async Task<int> GetMovementTypeIdByCode(string code)
        {
            var movementType = await _context.MovementTypes
                .FirstOrDefaultAsync(mt => mt.Code == code);

            if (movementType == null)
                throw new Exception($"نوع الحركة {code} غير موجود");

            return movementType.MovementTypeId;
        }
    }
}