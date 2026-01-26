using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.StockInViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class StockInService : IStockInService
    {
        private readonly ApplicationDbContext _context;
        public StockInService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateStockIn(StockInRequestDto dto, string user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var header = new StockInHeader
                    {
                        DocCode = GenerateDocCode(),
                        DocDate = dto.DocDate,
                        BranchId = dto.BranchId,
                        SupplierId = dto.SupplierId,
                        InvoiceNo = dto.InvoiceNo,
                        PurchaseOrderId = dto.PurchaseOrderId,
                        TransactionTypeId = dto.TransactionTypeId,
                        Remarks = dto.Remarks,
                        CreatedBy = user
                    };

                    header.Status = 0; // Draft
                    _context.StockInHeaders.Add(header);

                    foreach (var detail in dto.StockInDetails)
                    {
                        var stockInDetail = new StockInDetail
                        {
                            StockInId = header.StockInId,
                            ItemId = detail.ItemId,
                            Qty = detail.Qty,
                            BonusQuantity = detail.BonusQuantity,
                            Price = detail.Price,
                            ConsumerPrice = detail.ConsumerPrice,
                            Discount = detail.Discount,
                            VatRate = detail.VatRate,
                            VatAmount = detail.VatAmount,
                            NetValue = detail.NetValue,
                            TotalValue = detail.TotalValue,
                            BatchNo = detail.BatchNo,
                            ExpiryDate = detail.ExpiryDate,
                            PurchaseOrderDetailId = detail.PurchaseOrderDetailId,
                            TransferOrderDetailId = detail.TransferOrderDetailId,
                            CreatedBy = user
                        };
                        _context.StockInDetails.Add(stockInDetail);
                    }

                    // Calculate totals
                    header.TotalValue = dto.StockInDetails.Sum(d => d.TotalValue);
                    // VAT calculation might depend on item tax or global setting, using simple 14% as placeholder or 0 if not specified
                    header.VatValue = header.TotalValue * 0.14m;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    // If Direct Purchase, usually handled via direct execution or separate action
                    // Reverting to simpler logic
                    if (dto.TransactionTypeId == 4)
                    {
                        // Logic if direct
                    }

                    return header.StockInId;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task UpdateStockIn(StockInRequestDto dto, string user)
        {
            var header = await _context.StockInHeaders
                .Include(si => si.StockInDetails)
                .FirstOrDefaultAsync(si => si.StockInId == dto.StockInId);

            if (header == null) throw new Exception("StockIn not found");

            // Prevent update if Approved or Executed
            if (header.Status == 1 || header.Status == 3) throw new Exception("Cannot update approved or executed StockIn");

            header.DocDate = dto.DocDate;
            header.BranchId = dto.BranchId;
            header.SupplierId = dto.SupplierId;
            header.InvoiceNo = dto.InvoiceNo;
            header.PurchaseOrderId = dto.PurchaseOrderId;
            header.Remarks = dto.Remarks;
            header.ModifiedBy = user;
            header.ModifiedAt = DateTime.UtcNow;

            // Remove existing details
            _context.StockInDetails.RemoveRange(header.StockInDetails);

            // Add new details
            foreach (var detail in dto.StockInDetails)
            {
                var stockInDetail = new StockInDetail
                {
                    StockInId = header.StockInId,
                    ItemId = detail.ItemId,
                    Qty = detail.Qty,
                    BonusQuantity = detail.BonusQuantity,
                    Price = detail.Price,
                    ConsumerPrice = detail.ConsumerPrice,
                    Discount = detail.Discount,
                    VatRate = detail.VatRate,
                    VatAmount = detail.VatAmount,
                    NetValue = detail.NetValue,
                    TotalValue = detail.TotalValue,
                    BatchNo = detail.BatchNo,
                    ExpiryDate = detail.ExpiryDate,
                    PurchaseOrderDetailId = detail.PurchaseOrderDetailId,
                    TransferOrderDetailId = detail.TransferOrderDetailId,
                    CreatedBy = user
                };
                _context.StockInDetails.Add(stockInDetail);
            }

            // Recalculate totals
            header.TotalValue = dto.StockInDetails.Sum(d => d.TotalValue);
            header.VatValue = header.TotalValue * 0.14m;

            await _context.SaveChangesAsync();
        }

        public async Task ApproveStockIn(int id, string user)
        {
            var header = await _context.StockInHeaders.FindAsync(id);
            if (header == null) throw new Exception("StockIn not found");
            
            if (header.Status == 1 || header.Status == 3) throw new Exception("Already approved");

            header.Status = 1; // Approved
            header.ApprovedBy = user;
            header.ApprovedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ReceiveStockIn(int id, string user)
        {
            // Valid for PO receipts / Transfer receipts. Direct Purchase uses ExecuteDirectStockIn.
            var header = await _context.StockInHeaders
                .Include(si => si.StockInDetails)
                .FirstOrDefaultAsync(si => si.StockInId == id);

            if (header == null) throw new Exception("StockIn not found");

            if (header.Status != 1) throw new Exception("StockIn must be approved first");

            // Should probably check TransactionType here too, but generic logic applies

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    header.Status = 3; // Received/Executed

                    // Create Movement
                    var movement = new Movement
                    {
                        MovementCode = GenerateMovementCode(),
                        MovementTypeId = 1, // IN
                        BranchToId = header.BranchId,
                        SupplierId = header.SupplierId,
                        Date = DateTime.UtcNow,
                        Notes = $"StockIn {header.DocCode}",
                        CreatedBy = user,
                        Status = "Completed",
                        CreatedByUserId = user, // Ensure this matches User ID logic
                        ApprovalStatusId = 2 // Approved
                    };
                    _context.Movements.Add(movement);

                    foreach (var detail in header.StockInDetails)
                    {
                        var movementLine = new MovementLine
                        {
                            MovementId = movement.MovementId,
                            ItemId = detail.ItemId,
                            BranchId = header.BranchId,
                            QtyInput = detail.Qty,
                            UnitPrice = detail.Price,
                            CreatedBy = user,
                            Status = "Completed"
                        };
                        _context.MovementLines.Add(movementLine);

                        // Create inventory layer for FIFO costing
                        var layer = new InventoryLayer
                        {
                            ItemId = detail.ItemId,
                            BranchId = header.BranchId,
                            ReceiptDate = header.DocDate,
                            QuantityRemaining = detail.Qty,
                            UnitCost = detail.Price,
                            BatchNo = detail.BatchNo,
                            ExpiryDate = detail.ExpiryDate ?? DateTime.MinValue,
                            SourceDocumentId = header.StockInId,
                            SourceDocumentType = "StockIn"
                        };
                        _context.InventoryLayers.Add(layer);
                    }

                    // Update ReceivedQuantity in PurchaseOrderDetail
                    foreach (var detail in header.StockInDetails.Where(d => d.PurchaseOrderDetailId.HasValue))
                    {
                        var poDetail = await _context.PurchaseOrderDetails
                            .FirstOrDefaultAsync(pd => pd.PurchaseOrderDetailId == detail.PurchaseOrderDetailId.Value);

                        if (poDetail != null)
                        {
                            poDetail.ReceivedQuantity += detail.Qty;
                            poDetail.ModifiedBy = user;
                            poDetail.ModifiedAt = DateTime.UtcNow;
                        }
                    }

                    // Check if all items in the related PO are fully received
                    if (header.PurchaseOrderId.HasValue)
                    {
                        var poHeader = await _context.PurchaseOrderHeaders
                            .Include(p => p.PurchaseOrderDetails)
                            .FirstOrDefaultAsync(p => p.PurchaseOrderId == header.PurchaseOrderId.Value);

                        if (poHeader != null)
                        {
                            bool allReceived = poHeader.PurchaseOrderDetails.All(pd => pd.ReceivedQuantity >= pd.OrderedQuantity);
                            if (allReceived)
                            {
                                poHeader.Status = PurchaseOrderStatus.FullyReceived;
                            }
                            else
                            {
                                poHeader.Status = PurchaseOrderStatus.PartiallyReceived;
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        
        private async Task ExecuteDirectStockIn(int id, string user)
        {
             // Very similar to ReceiveStockIn but dedicated flow for Direct Purchase
             // Reuse ReceiveStockIn logic for consistency
             await ReceiveStockIn(id, user);
        }

        public async Task<List<StockInListDto>> GetStockIns()
        {
            return await _context.StockInHeaders
                .Include(si => si.Branch)
                .Include(si => si.Supplier)
                .Select(si => new StockInListDto
                {
                    StockInId = si.StockInId,
                    DocCode = si.DocCode,
                    PurchaseOrderCode = si.PurchaseOrderHeader != null ? si.PurchaseOrderHeader.PurchaseOrderCode : "",
                    SupplierName = si.Supplier != null ? si.Supplier.Name : "", // Handle Nullable Supplier
                    BranchName = si.Branch.Name,
                    DocDate = si.DocDate,
                    InvoiceNo = si.InvoiceNo,
                    TotalValue = si.TotalValue,
                    Status = si.Status.ToString(),
                    Remarks = si.Remarks
                })
                .ToListAsync();
        }

        public async Task DeleteStockIn(int id, string user)
        {
            var header = await _context.StockInHeaders.FindAsync(id);
            if (header == null) throw new Exception("StockIn not found");

            if (header.Status == 1 || header.Status == 3) throw new Exception("Cannot delete approved or received StockIn");

            _context.StockInHeaders.Remove(header);
            await _context.SaveChangesAsync();
        }

        public async Task DirectSaveStockIn(int purchaseOrderId, List<StockInDetailDto> details, string user)
        {
            var po = await _context.PurchaseOrderHeaders
                .Include(p => p.PurchaseOrderDetails)
                .FirstOrDefaultAsync(p => p.PurchaseOrderId == purchaseOrderId);
            if (po == null) throw new Exception("Purchase Order not found");

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var header = new StockInHeader
                    {
                        DocCode = GenerateDocCode(),
                        DocDate = DateTime.UtcNow,
                        EntryDate = DateTime.UtcNow,
                        BranchId = po.BranchId,
                        SupplierId = po.SupplierId,
                        InvoiceNo = null,
                        PurchaseOrderId = purchaseOrderId,
                        TransactionTypeId = 1,
                        Status = 3, // Directly received
                        Remarks = $"Direct receipt from PO {po.PurchaseOrderCode}",
                        CreatedBy = user,
                        ApprovedBy = user,
                        ApprovedDate = DateTime.UtcNow
                    };

                    _context.StockInHeaders.Add(header);

                    var addedDetails = new List<StockInDetail>();
                    foreach (var detail in details)
                    {
                        var stockInDetail = new StockInDetail
                        {
                            StockInId = header.StockInId,
                            ItemId = detail.ItemId,
                            Qty = detail.Qty,
                            BonusQuantity = detail.BonusQuantity,
                            Price = detail.Price,
                            ConsumerPrice = detail.ConsumerPrice,
                            Discount = detail.Discount,
                            VatRate = detail.VatRate,
                            VatAmount = detail.VatAmount,
                            NetValue = detail.NetValue,
                            TotalValue = detail.TotalValue,
                            BatchNo = detail.BatchNo,
                            ExpiryDate = detail.ExpiryDate,
                            PurchaseOrderDetailId = detail.PurchaseOrderDetailId,
                            CreatedBy = user
                        };
                        _context.StockInDetails.Add(stockInDetail);
                        addedDetails.Add(stockInDetail);
                    }

                    // Calculate totals
                    header.TotalValue = details.Sum(d => d.TotalValue);
                    header.VatValue = details.Sum(d => d.VatAmount);

                    // Create Movement
                    var movement = new Movement
                    {
                        MovementCode = GenerateMovementCode(),
                        MovementTypeId = 1, // IN
                        BranchToId = header.BranchId,
                        SupplierId = header.SupplierId,
                        Date = DateTime.UtcNow,
                        Notes = $"Direct StockIn {header.DocCode}",
                        CreatedBy = user,
                        Status = "Completed",
                        CreatedByUserId = user,
                        ApprovalStatusId = 2
                    };
                    _context.Movements.Add(movement);

                    foreach (var detail in addedDetails)
                    {
                        var movementLine = new MovementLine
                        {
                            MovementId = movement.MovementId,
                            ItemId = detail.ItemId,
                            BranchId = header.BranchId,
                            QtyInput = detail.Qty,
                            UnitPrice = detail.Price,
                            CreatedBy = user,
                            Status = "Completed"
                        };
                        _context.MovementLines.Add(movementLine);
                    }

                    // Update ReceivedQuantity
                    foreach (var detail in addedDetails.Where(d => d.PurchaseOrderDetailId.HasValue))
                    {
                        var poDetail = await _context.PurchaseOrderDetails
                            .FirstOrDefaultAsync(pd => pd.PurchaseOrderDetailId == detail.PurchaseOrderDetailId.Value);

                        if (poDetail != null)
                        {
                            poDetail.ReceivedQuantity += detail.Qty;
                            poDetail.ModifiedBy = user;
                            poDetail.ModifiedAt = DateTime.UtcNow;
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<Core.ViewModels.PurchaseOrderViewModels.PurchaseOrderDetailDto>> GetPurchaseOrderDetailsForReceipt(int purchaseOrderId)
        {
            var poDetails = await _context.PurchaseOrderDetails
                .Where(pd => pd.PurchaseOrderId == purchaseOrderId)
                .Select(pd => new Core.ViewModels.PurchaseOrderViewModels.PurchaseOrderDetailDto
                {
                    PurchaseOrderDetailId = pd.PurchaseOrderDetailId,
                    ItemId = pd.ItemId,
                    Quantity = pd.OrderedQuantity - pd.ReceivedQuantity, // Remaining to receive
                    Price = pd.Price,
                    TotalValue = pd.NetValue
                })
                .ToListAsync();

            return poDetails;
        }

        public async Task CalculateValues(StockInRequestDto dto)
        {
            foreach (var detail in dto.StockInDetails)
            {
                // NetValue = (Qty * Price) - Discount
                detail.NetValue = (detail.Qty * detail.Price) - detail.Discount;
                // VatAmount = NetValue * VatRate / 100
                detail.VatAmount = detail.NetValue * (detail.VatRate / 100);
                // TotalValue = NetValue + VatAmount
                detail.TotalValue = detail.NetValue + detail.VatAmount;
            }

            dto.TotalValue = dto.StockInDetails.Sum(d => d.TotalValue);
            dto.VatValue = dto.StockInDetails.Sum(d => d.VatAmount);
        }

        private string GenerateDocCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.StockInHeaders.Count(si => si.CreatedDate.Date == DateTime.Today) + 1;
            return $"SI-{date}-{count:D4}";
        }

        private string GenerateMovementCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.Movements.Count(m => m.CreatedAt.Date == DateTime.Today) + 1;
            return $"MOV-{date}-{count:D4}";
        }
    }
}