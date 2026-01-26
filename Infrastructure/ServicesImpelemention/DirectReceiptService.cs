using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.DirectReceiptViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class DirectReceiptService : IDirectReceiptService
    {
        private readonly ApplicationDbContext _context;

        public DirectReceiptService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrderRequestVM> GetDirectReceipt(int id)
        {
            var header = await _context.DirectReceiptHeaders
                .Include(drh => drh.DirectReceiptDetails)
                .Include(drh => drh.Supplier)
                .Include(drh => drh.Branch)
                .FirstOrDefaultAsync(drh => drh.DirectReceiptId == id);

            if (header == null) return null;

            return new PurchaseOrderRequestVM
            {
                RequestId = header.DirectReceiptId,
                PurchaseOrderHeaderRequest = new PurchaseOrderHeaderRequest
                {
                    DocDate = header.DocDate,
                    SupplierId = header.SupplierId,
                    BranchId = header.BranchId,
                    RemarksArab = header.RemarksArab,
                    ReferenceInvoiceNumber = header.ReferenceInvoiceNumber
                },
                Items = header.DirectReceiptDetails.Select(drd => new ItemTransferOrderVm
                {
                    ItemId = drd.ItemId,
                    Quantity = drd.Quantity,
                    Vat = drd.VatRate,
                    TotalValue = drd.TotalValue,
                    BonusQuantity = drd.BonusQuantity,
                    BatchNumber = drd.BatchNumber,
                    ExpiryDate = drd.ExpiryDate
                }).ToList()
            };
        }

        public async Task<int> CreateDirectReceipt(PurchaseOrderRequestVM dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Create Direct Receipt Header
                var header = new DirectReceiptHeader
                {
                    DirectReceiptCode = GenerateCode(),
                    DocDate = dto.PurchaseOrderHeaderRequest.DocDate,
                    EntryDate = DateTime.UtcNow,
                    SupplierId = dto.PurchaseOrderHeaderRequest.SupplierId,
                    BranchId = dto.PurchaseOrderHeaderRequest.BranchId,
                    ReferenceInvoiceNumber = dto.PurchaseOrderHeaderRequest.ReferenceInvoiceNumber,
                    RemarksArab = dto.PurchaseOrderHeaderRequest.RemarksArab,
                    StatusId = 2, // Approved/Ready
                    CreatedBy = "System"
                };

                // Calculate totals
                decimal totalValue = 0;
                decimal totalVat = 0;

                _context.DirectReceiptHeaders.Add(header);

                // Create Details
                foreach (var item in dto.Items)
                {
                    var itemVatAmount = item.TotalValue * (item.Vat / 100);
                    totalValue += item.TotalValue;
                    totalVat += itemVatAmount;

                    var detail = new DirectReceiptDetail
                    {
                        DirectReceiptId = header.DirectReceiptId,
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        UnitPrice = item.CustomerPrice,
                        VatRate = item.Vat,
                        VatAmount = itemVatAmount,
                        TotalValue = item.TotalValue,
                        BonusQuantity = item.BonusQuantity,
                        BatchNumber = item.BatchNumber,
                        ExpiryDate = item.ExpiryDate,
                        CreatedBy = "System"
                    };
                    _context.DirectReceiptDetails.Add(detail);
                }

                header.TotalValue = totalValue;
                header.TotalVat = totalVat;
                header.NetTotal = totalValue + totalVat;

                await _context.SaveChangesAsync();

                // Execute immediately for Direct Receipt
                await ExecuteDirectReceipt(header.DirectReceiptId);

                await transaction.CommitAsync();
                return header.DirectReceiptId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"خطأ في إنشاء الاستلام المباشر: {ex.Message}");
            }
        }

        public async Task UpdateDirectReceipt(PurchaseOrderRequestVM dto)
        {
            // Implementation for update
            throw new NotImplementedException();
        }

        public async Task ApproveDirectReceipt(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var header = await _context.DirectReceiptHeaders.FindAsync(id);
                if (header == null) throw new Exception("Receipt not found");

                if (header.StatusId == 2 || header.StatusId == 3) // 2 Approved, 3 Executed
                    throw new Exception("Already approved");

                header.StatusId = 2; // Approved
                await _context.SaveChangesAsync();

                // Auto-Execute after manual approval
                await ExecuteDirectReceipt(id);
                
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }

        private async Task ExecuteDirectReceipt(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Note: This method should be called within an active transaction from CreateDirectReceipt or ApproveDirectReceipt
                    var header = await _context.DirectReceiptHeaders
                        .Include(h => h.DirectReceiptDetails)
                        .FirstOrDefaultAsync(h => h.DirectReceiptId == id);

                    if (header == null) throw new Exception("Receipt not found");
                    if (header.StatusId == 3) return; // 3: Executed/Completed

                    // Get standard Approved status for movements
                    var approvedStatus = await _context.ApprovalStatuses.FirstOrDefaultAsync(s => s.Code == "Approved");
                    var approvedStatusId = approvedStatus?.ApprovalStatusId ?? 1;

                    // Get a system user ID if needed, or use a dummy for now if it's required but automated
                    // In many systems, "System" or a fixed GUID is used.
                    var systemUserId = "System"; // Replace with actual System User Id if required by FK

                    // Create Movement for receipt
                    var movement = new Movement
                    {
                        MovementCode = await GenerateMovementCode(),
                        MovementTypeId = await GetMovementTypeId("REC"),
                        SupplierId = header.SupplierId,
                        BranchToId = header.BranchId,
                        Date = DateTime.UtcNow,
                        InternalBarcode = Guid.NewGuid().ToString("N").Substring(0, 12),
                        Notes = $"Direct Receipt {header.DirectReceiptCode}",
                        CreatedBy = "System",
                        CreatedByUserId = systemUserId,
                        CreatedAt = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow,
                        Status = "Completed",
                        ApprovalStatusId = approvedStatusId,
                        TotalAmount = header.NetTotal
                    };
                    _context.Movements.Add(movement);

                    // Create Movement Lines
                    foreach (var item in header.DirectReceiptDetails)
                    {
                        var movementLine = new MovementLine
                        {
                            MovementId = movement.MovementId,
                            ItemId = item.ItemId,
                            BranchId = header.BranchId,
                            UnitCode = "EA", // Default
                            QtyInput = item.Quantity,
                            ConversionUsedToBase = 1,
                            QtyBase = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            Notes = $"Direct Receipt {header.DirectReceiptCode}",
                            CreatedBy = "System",
                            CreatedAt = DateTime.UtcNow,
                            Status = "Completed"
                        };
                        _context.MovementLines.Add(movementLine);
                    }

                    // Create Supplier Invoice
                    var invoice = new SupplierInvoiceHeader
                    {
                        InvoiceNumber = GenerateInvoiceNumber(),
                        InvoiceDate = header.DocDate,
                        SupplierId = header.SupplierId,
                        DirectReceiptId = header.DirectReceiptId,
                        TotalAmount = header.TotalValue,
                        VatAmount = header.TotalVat,
                        NetAmount = header.NetTotal,
                        CreatedBy = "System"
                    };
                    _context.SupplierInvoiceHeaders.Add(invoice);

                    // Create Invoice Details
                    foreach (var item in header.DirectReceiptDetails)
                    {
                        var invoiceDetail = new SupplierInvoiceDetail
                        {
                            SupplierInvoiceId = invoice.SupplierInvoiceId,
                            ItemId = item.ItemId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            VatRate = item.VatRate,
                            VatAmount = item.VatAmount,
                            TotalValue = item.TotalValue,
                            CreatedBy = "System"
                        };
                        _context.SupplierInvoiceDetails.Add(invoiceDetail);
                    }

                    header.StatusId = 3; // Executed/Completed
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

        public async Task DeleteDirectReceipt(int id)
        {
            var header = await _context.DirectReceiptHeaders
                .Include(drh => drh.DirectReceiptDetails)
                .FirstOrDefaultAsync(drh => drh.DirectReceiptId == id);

            if (header == null) throw new Exception("Direct receipt not found");

            _context.DirectReceiptDetails.RemoveRange(header.DirectReceiptDetails);
            _context.DirectReceiptHeaders.Remove(header);

            await _context.SaveChangesAsync();
        }

        public async Task<List<DirectReceiptListDto>> GetDirectReceipts()
        {
            return await _context.DirectReceiptHeaders
                .Include(drh => drh.Supplier)
                .Include(drh => drh.Branch)
                .Include(drh => drh.Status)
                .Select(drh => new DirectReceiptListDto
                {
                    DirectReceiptId = drh.DirectReceiptId,
                    DirectReceiptCode = drh.DirectReceiptCode,
                    SupplierName = drh.Supplier.Name,
                    BranchName = drh.Branch.Name,
                    EntryDate = drh.EntryDate,
                    DocDate = drh.DocDate,
                    Status = drh.Status != null ? drh.Status.Name : drh.StatusId.ToString(),
                    RemarksArab = drh.RemarksArab
                })
                .ToListAsync();
        }

        private string GenerateCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.DirectReceiptHeaders.Count(drh => drh.CreatedDate.Date == DateTime.Today) + 1;
            return $"DR-{date}-{count:D4}";
        }

        private async Task<string> GenerateMovementCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = await _context.Movements.CountAsync(m => m.CreatedAt.Date == DateTime.Today) + 1;
            return $"MOV-{date}-{count:D4}";
        }

        private async Task<int> GetMovementTypeId(string code)
        {
            var movementType = await _context.MovementTypes
                .FirstOrDefaultAsync(mt => mt.Code == code);

            if (movementType == null)
                throw new Exception($"Movement type {code} not found");

            return movementType.MovementTypeId;
        }

        private string GenerateInvoiceNumber()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.SupplierInvoiceHeaders.Count(sih => sih.CreatedDate.Date == DateTime.Today) + 1;
            return $"INV-{date}-{count:D4}";
        }
    }
}