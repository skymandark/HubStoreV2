using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.OrderViewModels;
using Core.ViewModels.PurchaseOrderViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        public PurchaseOrderService(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        public async Task<OrderViewModel> CreatePO(CreatePurchaseOrderViewModel poDto)
        {
            try
            {
                // تأكد من أن نوع الأمر هو أمر شراء
                var orderType = await _context.OrderTypes
                    .FirstOrDefaultAsync(ot => ot.Code == "PO");

                if (orderType == null)
                    throw new Exception("نوع أمر الشراء غير موجود");

                poDto.OrderTypeId = orderType.OrderTypeId;

                // إنشاء الأمر
                var orderDto = new CreateOrderViewModel
                {
                    OrderCode = poDto.OrderCode,
                    OrderTypeId = poDto.OrderTypeId,
                    SupplierId = poDto.SupplierId,
                    BranchFromId = poDto.BranchFromId,
                    BranchToId = poDto.BranchToId,
                    RequestedByUserId = poDto.RequestedByUserId,
                    RequestedDate = poDto.RequestedDate,
                    PriorityFlag = poDto.PriorityFlag,
                    SLA_DueDate = poDto.SLA_DueDate,
                    InternalBarcode = poDto.InternalBarcode,
                    ExternalBarcode = poDto.ExternalBarcode,
                    Notes = poDto.Notes,
                    CreatedBy = poDto.CreatedBy,
                    Lines = poDto.Lines
                };

                return await _orderService.CreateOrder(orderDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في إنشاء أمر الشراء: {ex.Message}");
            }
        }

        public async Task<OrderViewModel> ReceivePO(int orderId, ReceiveOrderViewModel receivingDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderLines)
                    .ThenInclude(l => l.Item)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (order == null)
                    throw new Exception("أمر الشراء غير موجود");

                if (order.Status != "Approved")
                    throw new Exception("لا يمكن استلام أمر غير معتمد");

                // إنشاء حركة استلام
                var movement = new Movement
                {
                    MovementCode = await GenerateReceivingCode(),
                    MovementTypeId = await GetReceivingMovementTypeId(),
                    SupplierId = order.SupplierId,
                    BranchToId = order.BranchToId,
                    Date = DateTime.UtcNow,
                    InternalBarcode = Guid.NewGuid().ToString("N").Substring(0, 12),
                    Notes = receivingDto.ReceivingNotes,
                    CreatedBy = receivingDto.ReceivedBy,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Completed"
                };
                _context.Movements.Add(movement);
                await _context.SaveChangesAsync();

                // معالجة كل سطر مستلم
                foreach (var lineDto in receivingDto.Lines)
                {
                    var orderLine = order.OrderLines
                        .FirstOrDefault(l => l.OrderLineId == lineDto.OrderLineId);

                    if (orderLine == null)
                        continue;

                    // تسجيل الاستلام
                    var receipt = new Receipt
                    {
                        OrderLineId = orderLine.OrderLineId,
                        MovementId = movement.MovementId,
                        QuantityReceived = lineDto.QtyReceived,
                        QuantityBaseReceived = lineDto.QtyReceived * orderLine.ConversionUsedToBase,
                        ReceivedAt = DateTime.UtcNow,
                        CreatedBy = receivingDto.ReceivedBy
                    };
                    _context.Receipts.Add(receipt);

                    // تحديث سطر الأمر
                    orderLine.QtyReceived += lineDto.QtyReceived;
                    orderLine.QtyBaseReceived += lineDto.QtyReceived * orderLine.ConversionUsedToBase;

                    // تحديث حالة السطر
                    if (orderLine.QtyReceived >= orderLine.QtyOrdered)
                    {
                        orderLine.LineStatus = "Completed";
                    }
                    else
                    {
                        orderLine.LineStatus = "PartiallyReceived";
                    }

                    // إنشاء سطر في الحركة
                    var movementLine = new MovementLine
                    {
                        MovementId = movement.MovementId,
                        ItemId = orderLine.ItemId,
                        BranchId = order.BranchToId.Value,
                        UnitCode = orderLine.UnitCode,
                        QtyInput = lineDto.QtyReceived,
                        ConversionUsedToBase = orderLine.ConversionUsedToBase,
                        QtyBase = lineDto.QtyReceived * orderLine.ConversionUsedToBase,
                        UnitPrice = orderLine.UnitPrice,
                        Notes = lineDto.Notes,
                        CreatedBy = receivingDto.ReceivedBy,
                        CreatedAt = DateTime.UtcNow,
                        Status = "Completed"
                    };
                    _context.MovementLines.Add(movementLine);
                }

                // تحديث حالة الأمر
                if (order.OrderLines.All(l => l.LineStatus == "Completed"))
                {
                    order.Status = "Completed";
                }
                else
                {
                    order.Status = "PartiallyReceived";
                }

                order.CreatedAt = DateTime.UtcNow;
                order.CreatedBy = receivingDto.ReceivedBy;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await _orderService.GetOrder(orderId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"خطأ في استلام أمر الشراء: {ex.Message}");
            }
        }

        public async Task<POStatusViewModel> GetPOStatus(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                throw new Exception("أمر الشراء غير موجود");

            var totalOrdered = order.OrderLines.Sum(l => l.QtyBaseOrdered);
            var totalReceived = order.OrderLines.Sum(l => l.QtyBaseReceived);

            return new POStatusViewModel
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                TotalOrdered = totalOrdered,
                TotalReceived = totalReceived,
                RemainingQuantity = (int)(totalOrdered - totalReceived),
                Status = order.Status
            };
        }

        private async Task<string> GenerateReceivingCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = await _context.Movements
                .CountAsync(m => m.MovementCode.StartsWith("REC-") &&
                                m.CreatedAt.Date == DateTime.Today) + 1;

            return $"REC-{date}-{count:D4}";
        }

        private async Task<int> GetReceivingMovementTypeId()
        {
            var movementType = await _context.MovementTypes
                .FirstOrDefaultAsync(mt => mt.Code == "REC");

            if (movementType == null)
                throw new Exception("نوع حركة الاستلام غير موجود");

            return movementType.MovementTypeId;
        }

        // New Implementation for PurchaseOrderHeader
        public async Task<PurchaseOrderRequestDto?> GetPurchaseOrder(int id)
        {
            var po = await _context.PurchaseOrderHeaders
                .Include(h => h.PurchaseOrderDetails).ThenInclude(d => d.Item)
                .Include(h => h.Supplier)
                .Include(h => h.Branch)
                .FirstOrDefaultAsync(h => h.PurchaseOrderId == id);

            if (po == null) return null;

            return new PurchaseOrderRequestDto
            {
                PurchaseOrderId = po.PurchaseOrderId,
                PurchaseOrderCode = po.PurchaseOrderCode,
                DocDate = po.DocDate,
                SupplierId = po.SupplierId,
                BranchId = po.BranchId,
                Remarks = po.Notes,
                Status = po.Status.ToString(),
                PurchaseOrderDetails = po.PurchaseOrderDetails.Select(d => new PurchaseOrderDetailDto
                {
                    PurchaseOrderDetailId = d.PurchaseOrderDetailId,
                    ItemId = d.ItemId,
                    Quantity = d.OrderedQuantity,
                    Price = d.Price,
                    TotalValue = d.NetValue
                }).ToList()
            };
        }

        public async Task<int> CreatePurchaseOrder(PurchaseOrderRequestDto dto)
        {
            var header = new PurchaseOrderHeader
            {
                PurchaseOrderCode = "PO-" + DateTime.Now.Ticks, // Simplified Code Gen
                DocDate = dto.DocDate,
                SupplierId = dto.SupplierId,
                BranchId = dto.BranchId,
                Notes = dto.Remarks,
                Status = PurchaseOrderStatus.Draft,
                CreatedBy = "System", // User usually passed in args
                CreatedDate = DateTime.UtcNow
            };

            _context.PurchaseOrderHeaders.Add(header);
            await _context.SaveChangesAsync();

            foreach (var d in dto.PurchaseOrderDetails)
            {
                var detail = new PurchaseOrderDetail
                {
                    PurchaseOrderId = header.PurchaseOrderId,
                    ItemId = d.ItemId,
                    OrderedQuantity = d.Quantity,
                    Price = d.Price,
                    NetValue = d.TotalValue
                };
                _context.PurchaseOrderDetails.Add(detail);
            }

            await _context.SaveChangesAsync();
            return header.PurchaseOrderId;
        }

        public async Task UpdatePurchaseOrder(PurchaseOrderRequestDto dto)
        {
             var header = await _context.PurchaseOrderHeaders
                .Include(h => h.PurchaseOrderDetails)
                .FirstOrDefaultAsync(h => h.PurchaseOrderId == dto.PurchaseOrderId);

            if (header == null) throw new Exception("Purchase Order not found");

            if (header.Status == PurchaseOrderStatus.Approved) 
                throw new Exception("Cannot edit approved purchase order");

            header.DocDate = dto.DocDate;
            header.SupplierId = dto.SupplierId;
            header.BranchId = dto.BranchId;
            header.Notes = dto.Remarks;

            _context.PurchaseOrderDetails.RemoveRange(header.PurchaseOrderDetails);
            
             foreach (var d in dto.PurchaseOrderDetails)
            {
                var detail = new PurchaseOrderDetail
                {
                    PurchaseOrderId = header.PurchaseOrderId,
                    ItemId = d.ItemId,
                    OrderedQuantity = d.Quantity,
                    Price = d.Price,
                    NetValue = d.TotalValue
                };
                _context.PurchaseOrderDetails.Add(detail);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeletePurchaseOrder(int id)
        {
            var header = await _context.PurchaseOrderHeaders.FindAsync(id);
            if (header == null) throw new Exception("PO not found");
             if (header.Status == PurchaseOrderStatus.Approved) 
                throw new Exception("Cannot delete approved purchase order");
            
            _context.PurchaseOrderHeaders.Remove(header);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantities(int id, List<PurchaseOrderDetailDto> details)
        {
             var header = await _context.PurchaseOrderHeaders
                .Include(h => h.PurchaseOrderDetails)
                .FirstOrDefaultAsync(h => h.PurchaseOrderId == id);
            
             if (header == null) throw new Exception("PO not found");

             foreach(var d in details)
             {
                 var detail = header.PurchaseOrderDetails.FirstOrDefault(x => x.PurchaseOrderDetailId == d.PurchaseOrderDetailId);
                 if(detail != null)
                 {
                     detail.OrderedQuantity = d.Quantity;
                 }
             }
             await _context.SaveChangesAsync();
        }

        public async Task<List<PurchaseOrderListDto>> GetPurchaseOrders()
        {
             return await _context.PurchaseOrderHeaders
                .Include(h => h.Supplier)
                .Include(h => h.Branch)
                .Select(h => new PurchaseOrderListDto
                {
                    PurchaseOrderId = h.PurchaseOrderId,
                    PurchaseOrderCode = h.PurchaseOrderCode,
                    DocDate = h.DocDate,
                    SupplierName = h.Supplier.Name,
                    BranchName = h.Branch.Name,
                    Status = h.Status.ToString(),
                    TotalValue = h.NetTotal
                }).ToListAsync();
        }

        public async Task SubmitForApproval(int id, string user)
        {
            var po = await _context.PurchaseOrderHeaders.FindAsync(id);
            if (po == null) throw new Exception("PO not found");
            po.Status = PurchaseOrderStatus.Submitted; // Explicitly Submit
            await _context.SaveChangesAsync();
        }

        public async Task ApprovePurchaseOrder(int id, string user)
        {
            var po = await _context.PurchaseOrderHeaders.FindAsync(id);
            if (po == null) throw new Exception("Purchase order not found");

            po.Status = PurchaseOrderStatus.Approved;
            po.ApprovedBy = user;
            po.ApprovedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RejectPurchaseOrder(int id, string user, string reason)
        {
            var po = await _context.PurchaseOrderHeaders.FindAsync(id);
            if (po == null) throw new Exception("PO not found");
            po.Status = PurchaseOrderStatus.Rejected;
            await _context.SaveChangesAsync();
        }

        public async Task ClosePurchaseOrder(int id, string user)
        {
            var po = await _context.PurchaseOrderHeaders.FindAsync(id);
             if (po == null) throw new Exception("PO not found");
            po.Status = PurchaseOrderStatus.Closed;
            po.ClosedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task CancelPurchaseOrder(int id, string user)
        {
             var po = await _context.PurchaseOrderHeaders.FindAsync(id);
             if (po == null) throw new Exception("PO not found");
             // Logic for cancel (maybe soft delete or status cancelled if exists, or Rejected)
             po.Status = PurchaseOrderStatus.Rejected; 
             await _context.SaveChangesAsync();
        }
    }
}