using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.DirectPurchaseOrderViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class DirectPurchaseOrderService : IDirectPurchaseOrderService
    {
        private readonly ApplicationDbContext _context;

        public DirectPurchaseOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateDirectPurchaseOrder(CreateDirectPurchaseOrderViewModel dto, string user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Generate DocCode
                    var docCode = GenerateDocCode();

                    var header = new DirectPurchaseOrderHeader
                    {
                        DocCode = docCode,
                        DocDate = dto.DocDate,
                        EntryDate = DateTime.UtcNow,
                        SupplierId = dto.SupplierId,
                        InvoiceId = dto.InvoiceId,
                        StatusId = 1, // Draft - adjust based on system settings
                        BranchId = dto.BranchId,
                        ShipmentTypeId = dto.ShipmentTypeId,
                        DeliveryDate = dto.DeliveryDate,
                        CreditPayment = dto.CreditPayment,
                        PaymentPeriodDays = dto.PaymentPeriodDays,
                        DueDate = dto.DueDate,
                        CustomerId = dto.CustomerId,
                        Reference = dto.Reference,
                        Notes = dto.Notes,
                        TotalPrice = dto.TotalPrice,
                        TotalDiscount = dto.TotalDiscount,
                        TotalAddedDiscount = dto.TotalAddedDiscount,
                        VatValue = dto.VatValue,
                        NetValue = dto.NetValue,
                        CreatedBy = user,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.DirectPurchaseOrderHeaders.Add(header);

                    int serial = 1;
                    foreach (var detail in dto.Details)
                    {
                        var orderDetail = new DirectPurchaseOrderDetail
                        {
                            DirectPurchaseOrderId = header.DirectPurchaseOrderId,
                            Serial = serial++,
                            ItemId = detail.ItemId,
                            ItemPackageId = detail.ItemPackageId,
                            Quantity = detail.Quantity,
                            BonusQuantity = detail.BonusQuantity,
                            InQuantity = 0, // Initially 0
                            Price = detail.Price,
                            TotalValue = detail.TotalValue,
                            MainDiscountPercent = detail.MainDiscountPercent,
                            MainDiscountValue = detail.MainDiscountValue,
                            AddedDiscountPercent = detail.AddedDiscountPercent,
                            AddedDiscountValue = detail.AddedDiscountValue,
                            VatValue = detail.VatValue,
                            NetValue = detail.NetValue,
                            RemarksArab = detail.RemarksArab,
                            RemarksEng = detail.RemarksEng,
                            CreatedBy = user,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.DirectPurchaseOrderDetails.Add(orderDetail);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return header.DirectPurchaseOrderId;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task UpdateDirectPurchaseOrder(UpdateDirectPurchaseOrderViewModel dto, string user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var header = await _context.DirectPurchaseOrderHeaders
                        .Include(h => h.DirectPurchaseOrderDetails)
                        .FirstOrDefaultAsync(h => h.DirectPurchaseOrderId == dto.DirectPurchaseOrderId);

                    if (header == null) throw new Exception("Direct Purchase Order not found");

                    if (header.StatusId != 1) throw new Exception("Cannot update approved or processed order");

                    header.DocDate = dto.DocDate;
                    header.SupplierId = dto.SupplierId;
                    header.InvoiceId = dto.InvoiceId;
                    header.BranchId = dto.BranchId;
                    header.ShipmentTypeId = dto.ShipmentTypeId;
                    header.DeliveryDate = dto.DeliveryDate;
                    header.CreditPayment = dto.CreditPayment;
                    header.PaymentPeriodDays = dto.PaymentPeriodDays;
                    header.DueDate = dto.DueDate;
                    header.CustomerId = dto.CustomerId;
                    header.Reference = dto.Reference;
                    header.Notes = dto.Notes;
                    header.TotalPrice = dto.TotalPrice;
                    header.TotalDiscount = dto.TotalDiscount;
                    header.TotalAddedDiscount = dto.TotalAddedDiscount;
                    header.VatValue = dto.VatValue;
                    header.NetValue = dto.NetValue;
                    header.ModifiedBy = user;
                    header.ModifiedAt = DateTime.UtcNow;

                    // Remove existing details
                    _context.DirectPurchaseOrderDetails.RemoveRange(header.DirectPurchaseOrderDetails);

                    // Add new details
                    int serial = 1;
                    foreach (var detail in dto.Details)
                    {
                        var orderDetail = new DirectPurchaseOrderDetail
                        {
                            DirectPurchaseOrderId = header.DirectPurchaseOrderId,
                            Serial = serial++,
                            ItemId = detail.ItemId,
                            ItemPackageId = detail.ItemPackageId,
                            Quantity = detail.Quantity,
                            BonusQuantity = detail.BonusQuantity,
                            InQuantity = 0,
                            Price = detail.Price,
                            TotalValue = detail.TotalValue,
                            MainDiscountPercent = detail.MainDiscountPercent,
                            MainDiscountValue = detail.MainDiscountValue,
                            AddedDiscountPercent = detail.AddedDiscountPercent,
                            AddedDiscountValue = detail.AddedDiscountValue,
                            VatValue = detail.VatValue,
                            NetValue = detail.NetValue,
                            RemarksArab = detail.RemarksArab,
                            RemarksEng = detail.RemarksEng,
                            CreatedBy = user,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.DirectPurchaseOrderDetails.Add(orderDetail);
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

        public async Task ApproveDirectPurchaseOrder(int id, string user)
        {
            var header = await _context.DirectPurchaseOrderHeaders.FindAsync(id);
            if (header == null) throw new Exception("Direct Purchase Order not found");

            if (header.StatusId != 1) throw new Exception("Order is not in draft status");

            header.StatusId = 2; // Approved
            header.ModifiedBy = user;
            header.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RejectDirectPurchaseOrder(int id, string user)
        {
            var header = await _context.DirectPurchaseOrderHeaders.FindAsync(id);
            if (header == null) throw new Exception("Direct Purchase Order not found");

            header.StatusId = 3; // Rejected
            header.ModifiedBy = user;
            header.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDirectPurchaseOrder(int id, string user)
        {
            var header = await _context.DirectPurchaseOrderHeaders.FindAsync(id);
            if (header == null) throw new Exception("Direct Purchase Order not found");

            if (header.StatusId != 1) throw new Exception("Cannot delete approved or processed order");

            _context.DirectPurchaseOrderHeaders.Remove(header);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DirectPurchaseOrderListViewModel>> GetDirectPurchaseOrders()
        {
            return await _context.DirectPurchaseOrderHeaders
                .Include(h => h.Supplier)
                .Include(h => h.Branch)
                .Select(h => new DirectPurchaseOrderListViewModel
                {
                    DirectPurchaseOrderId = h.DirectPurchaseOrderId,
                    DocCode = h.DocCode,
                    DocDate = h.DocDate,
                    SupplierName = h.Supplier.Name,
                    BranchName = h.Branch.Name,
                    NetValue = h.NetValue,
                    StatusName = h.StatusId == 1 ? "Draft" : h.StatusId == 2 ? "Approved" : "Rejected",
                    CreatedBy = h.CreatedBy
                })
                .ToListAsync();
        }

        public async Task<DirectPurchaseOrderViewModel> GetDirectPurchaseOrder(int id)
        {
            var header = await _context.DirectPurchaseOrderHeaders
                .Include(h => h.DirectPurchaseOrderDetails)
                .ThenInclude(d => d.Item)
                .Include(h => h.Supplier)
                .Include(h => h.Branch)
                .Include(h => h.ShipmentType)
                .FirstOrDefaultAsync(h => h.DirectPurchaseOrderId == id);

            if (header == null) return null;

            return new DirectPurchaseOrderViewModel
            {
                DirectPurchaseOrderId = header.DirectPurchaseOrderId,
                DocCode = header.DocCode,
                DocDate = header.DocDate,
                EntryDate = header.EntryDate,
                SupplierId = header.SupplierId,
                SupplierName = header.Supplier?.Name,
                InvoiceId = header.InvoiceId,
                StatusId = header.StatusId,
                StatusName = header.StatusId == 1 ? "Draft" : header.StatusId == 2 ? "Approved" : "Rejected",
                BranchId = header.BranchId,
                BranchName = header.Branch?.Name,
                ShipmentTypeId = header.ShipmentTypeId,
                ShipmentTypeName = header.ShipmentType?.Name,
                DeliveryDate = header.DeliveryDate,
                CreditPayment = header.CreditPayment,
                PaymentPeriodDays = header.PaymentPeriodDays,
                DueDate = header.DueDate,
                CustomerId = header.CustomerId,
                Reference = header.Reference,
                Notes = header.Notes,
                TotalPrice = header.TotalPrice,
                TotalDiscount = header.TotalDiscount,
                TotalAddedDiscount = header.TotalAddedDiscount,
                VatValue = header.VatValue,
                NetValue = header.NetValue,
                CreatedBy = header.CreatedBy,
                CreatedAt = header.CreatedAt,
                ModifiedBy = header.ModifiedBy,
                ModifiedAt = header.ModifiedAt,
                Details = header.DirectPurchaseOrderDetails.Select(d => new DirectPurchaseOrderDetailViewModel
                {
                    DirectPurchaseOrderDetailId = d.DirectPurchaseOrderDetailId,
                    Serial = d.Serial,
                    ItemId = d.ItemId,
                    ItemName = d.Item?.NameArab,
                    ItemPackageId = d.ItemPackageId,
                    Quantity = d.Quantity,
                    BonusQuantity = d.BonusQuantity,
                    InQuantity = d.InQuantity,
                    Price = d.Price,
                    TotalValue = d.TotalValue,
                    MainDiscountPercent = d.MainDiscountPercent,
                    MainDiscountValue = d.MainDiscountValue,
                    AddedDiscountPercent = d.AddedDiscountPercent,
                    AddedDiscountValue = d.AddedDiscountValue,
                    VatValue = d.VatValue,
                    NetValue = d.NetValue,
                    RemarksArab = d.RemarksArab,
                    RemarksEng = d.RemarksEng
                }).ToList()
            };
        }

        public async Task CalculateTotals(CreateDirectPurchaseOrderViewModel dto)
        {
            decimal totalPrice = 0;
            decimal totalDiscount = 0;
            decimal totalAddedDiscount = 0;
            decimal totalVat = 0;

            foreach (var detail in dto.Details)
            {
                // Calculate discounts
                decimal lineTotal = detail.Quantity * detail.Price;
                decimal mainDiscount = detail.MainDiscountPercent > 0 ?
                    lineTotal * (detail.MainDiscountPercent / 100) : detail.MainDiscountValue;
                decimal afterMainDiscount = lineTotal - mainDiscount;
                decimal addedDiscount = detail.AddedDiscountPercent > 0 ?
                    afterMainDiscount * (detail.AddedDiscountPercent / 100) : detail.AddedDiscountValue;
                decimal afterDiscounts = afterMainDiscount - addedDiscount;

                // VAT
                decimal vat = afterDiscounts * (detail.VatRate / 100);

                detail.TotalValue = lineTotal;
                detail.VatValue = vat;
                detail.NetValue = afterDiscounts + vat;

                totalPrice += lineTotal;
                totalDiscount += mainDiscount;
                totalAddedDiscount += addedDiscount;
                totalVat += vat;
            }

            dto.TotalPrice = totalPrice;
            dto.TotalDiscount = totalDiscount;
            dto.TotalAddedDiscount = totalAddedDiscount;
            dto.VatValue = totalVat;
            dto.NetValue = totalPrice - totalDiscount - totalAddedDiscount + totalVat;
        }

        private string GenerateDocCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.DirectPurchaseOrderHeaders.Count(h => h.CreatedAt.Date == DateTime.Today) + 1;
            return $"DPO-{date}-{count:D4}";
        }
    }
}