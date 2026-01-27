using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.StockOutReturnViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Services.SettingServices;

namespace Infrastructure.ServicesImpelemention
{
    public class StockOutReturnService : IStockOutReturnService
    {
        private readonly ApplicationDbContext _context;

        private readonly ISettingService _settingService;

        public StockOutReturnService(ApplicationDbContext context, ISettingService settingService)
        {
            _context = context;
            _settingService = settingService;
        }

        public async Task<int> CreateStockOutReturn(StockOutReturnVm dto, string user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var header = new StockOutReturnHeader
                {
                    DocCode = GenerateDocCode(),
                    TransactionType = dto.TransactionTypeId,
                    ClientId = dto.ClientId,
                    BranchId = dto.BranchId,
                    BranchStockId = dto.BranchStockId,
                    DocDate = dto.DocDate,
                    EntryDate = DateTime.UtcNow,
                    Reference = dto.Reference,
                    Remarks = dto.Remarks,
                    Status = 0, // Draft
                    TotalValue = dto.TotalValue,
                    TotalDiscount = dto.TotalDiscount,
                    TotalAddedDiscount = dto.TotalAddedDiscount,
                    VatValue = dto.VatValue,
                    NetValue = dto.NetValue,
                    TotalPrice = dto.TotalPrice,
                    CreatedBy = user,
                    CreatedDate = DateTime.UtcNow
                };

                // Check Approval Workflow Setting
                bool useApproval = await _settingService.GetApprovalWorkflowModeAsync();
                if (!useApproval)
                {
                    header.Status = 1; // Approved
                    header.ApprovedBy = user;
                    header.ApprovedDate = DateTime.UtcNow;
                }

                _context.StockOutReturnHeaders.Add(header);
                await _context.SaveChangesAsync();

                foreach (var detail in dto.StockOutReturnDetails)
                {
                    var line = new StockOutReturnDetail
                    {
                        StockOutReturnId = header.StockOutReturnId,
                        ItemId = detail.ItemId,
                        ItemPackageId = detail.ItemPackageId,
                        OrderQuantity = detail.StockOutQuantity,
                        Qty = detail.Quantity,
                        Price = detail.Price,
                        Value = detail.Value,
                        BatchNo = detail.BatchNumber,
                        ExpiryDate = detail.ExpireDate,
                        DiscountPercent = detail.DiscountPercent,
                        DiscountValue = detail.DiscountValue,
                        AddedDiscountPercent = detail.AddedDiscountPercent,
                        AddedDiscountValue = detail.AddedDiscountValue,
                        VatValue = detail.VatValue,
                        NetValue = detail.NetValue,
                        TotalValue = detail.TotalPrice,
                        Remarks = detail.Remarks,
                        ReturnReasonId = detail.ReturnReasonId,
                        ReturnOrderDetailId = detail.ReturnOrderDetailId,
                        CreatedBy = user,
                        CreatedDate = DateTime.UtcNow
                    };
                    _context.StockOutReturnDetails.Add(line);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return header.StockOutReturnId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateStockOutReturn(StockOutReturnVm dto, string user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var header = await _context.StockOutReturnHeaders
                    .Include(h => h.StockOutReturnDetails)
                    .FirstOrDefaultAsync(h => h.StockOutReturnId == dto.RequestId);

                if (header == null) throw new Exception("الطلب غير موجود");
                if (header.Status != 0) throw new Exception("لا يمكن تعديل طلب تمت الموافقة عليه أو تنفيذه");

                header.TransactionType = dto.TransactionTypeId;
                header.ClientId = dto.ClientId;
                header.BranchId = dto.BranchId;
                header.BranchStockId = dto.BranchStockId;
                header.DocDate = dto.DocDate;
                header.Reference = dto.Reference;
                header.Remarks = dto.Remarks;
                header.TotalValue = dto.TotalValue;
                header.TotalDiscount = dto.TotalDiscount;
                header.TotalAddedDiscount = dto.TotalAddedDiscount;
                header.VatValue = dto.VatValue;
                header.NetValue = dto.NetValue;
                header.TotalPrice = dto.TotalPrice;
                header.ModifiedBy = user;
                header.ModifiedAt = DateTime.UtcNow;

                _context.StockOutReturnDetails.RemoveRange(header.StockOutReturnDetails);

                foreach (var detail in dto.StockOutReturnDetails)
                {
                    var line = new StockOutReturnDetail
                    {
                        StockOutReturnId = header.StockOutReturnId,
                        ItemId = detail.ItemId,
                        ItemPackageId = detail.ItemPackageId,
                        OrderQuantity = detail.StockOutQuantity,
                        Qty = detail.Quantity,
                        Price = detail.Price,
                        Value = detail.Value,
                        BatchNo = detail.BatchNumber,
                        ExpiryDate = detail.ExpireDate,
                        DiscountPercent = detail.DiscountPercent,
                        DiscountValue = detail.DiscountValue,
                        AddedDiscountPercent = detail.AddedDiscountPercent,
                        AddedDiscountValue = detail.AddedDiscountValue,
                        VatValue = detail.VatValue,
                        NetValue = detail.NetValue,
                        TotalValue = detail.TotalPrice,
                        Remarks = detail.Remarks,
                        ReturnReasonId = detail.ReturnReasonId,
                        ReturnOrderDetailId = detail.ReturnOrderDetailId,
                        CreatedBy = user,
                        CreatedDate = DateTime.UtcNow
                    };
                    _context.StockOutReturnDetails.Add(line);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ApproveStockOutReturn(int id, string user)
        {
            var header = await _context.StockOutReturnHeaders.FindAsync(id);
            if (header == null) throw new Exception("الطلب غير موجود");
            
            header.Status = 1; // Approved
            header.ApprovedBy = user;
            header.ApprovedDate = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }

        public async Task SubmitStockOutReturn(int id, string user)
        {
            var header = await _context.StockOutReturnHeaders.FindAsync(id);
            if (header == null) throw new Exception("الطلب غير موجود");

            header.Status = 2; // Pending Approval
            header.ModifiedBy = user;
            header.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RejectStockOutReturn(int id, string user, string reason)
        {
            var header = await _context.StockOutReturnHeaders.FindAsync(id);
            if (header == null) throw new Exception("الطلب غير موجود");
            
            header.Status = 4; // Rejected as per comment in header (Actually 4 is rejected, 2 is pending)
            header.Remarks = (header.Remarks ?? "") + " | سبب الرفض: " + reason;
            header.ModifiedBy = user;
            header.ModifiedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();
        }

        public async Task ExecuteStockOutReturn(int id, string user)
        {
            // Implementation for execution (inventory movement) would go here
            // For now, just mark as Executed
            var header = await _context.StockOutReturnHeaders.FindAsync(id);
            if (header == null) throw new Exception("الطلب غير موجود");
            
            header.Status = 3; // Executed
            await _context.SaveChangesAsync();
        }

        public async Task<List<StockOutReturnListDto>> GetStockOutReturns()
        {
            return await (await GetStockOutReturnsQuery()).ToListAsync();
        }

        public async Task<IQueryable<StockOutReturnListDto>> GetStockOutReturnsQuery()
        {
            return _context.StockOutReturnHeaders
                .Include(h => h.Branch)
                .Select(h => new StockOutReturnListDto
                {
                    StockOutReturnId = h.StockOutReturnId,
                    DocCode = h.DocCode,
                    ClientName = "", // Needs join with Client if exists
                    BranchName = h.Branch.Name,
                    DocDate = h.DocDate,
                    Status = h.Status.ToString(),
                    TotalValue = h.TotalValue,
                    NetValue = h.NetValue,
                    Remarks = h.Remarks
                });
        }

        public async Task<StockOutReturnVm> GetStockOutReturn(int id)
        {
            var header = await _context.StockOutReturnHeaders
                .Include(h => h.StockOutReturnDetails)
                .ThenInclude(d => d.Item)
                .FirstOrDefaultAsync(h => h.StockOutReturnId == id);

            if (header == null) return null;

            return new StockOutReturnVm
            {
                RequestId = header.StockOutReturnId,
                TransactionTypeId = header.TransactionType,
                ClientId = header.ClientId,
                BranchId = header.BranchId,
                BranchStockId = header.BranchStockId,
                DocDate = header.DocDate,
                EntryDate = header.EntryDate,
                Reference = header.Reference,
                Remarks = header.Remarks,
                TotalValue = header.TotalValue,
                TotalDiscount = header.TotalDiscount,
                TotalAddedDiscount = header.TotalAddedDiscount,
                VatValue = header.VatValue,
                NetValue = header.NetValue,
                TotalPrice = header.TotalPrice,
                StockOutReturnDetails = header.StockOutReturnDetails.Select(d => new StockOutReturnDetailVm
                {
                    StockOutReturnDetailId = d.StockOutReturnDetailId,
                    ItemId = d.ItemId,
                    ItemName = d.Item?.NameArab,
                    ItemPackageId = d.ItemPackageId,
                    Quantity = d.Qty,
                    Price = d.Price,
                    Value = d.Value,
                    StockOutQuantity = d.OrderQuantity,
                    BatchNumber = d.BatchNo,
                    ExpireDate = d.ExpiryDate,
                    DiscountPercent = d.DiscountPercent,
                    DiscountValue = d.DiscountValue,
                    AddedDiscountPercent = d.AddedDiscountPercent,
                    AddedDiscountValue = d.AddedDiscountValue,
                    NetValue = d.NetValue,
                    TotalPrice = d.TotalValue,
                    VatValue = d.VatValue,
                    Remarks = d.Remarks,
                    ReturnReasonId = d.ReturnReasonId,
                    ReturnOrderDetailId = d.ReturnOrderDetailId
                }).ToList()
            };
        }

        public async Task<List<ReturnOrderForStockOutDto>> GetSalesOrdersForClient(int clientId, int branchId)
        {
            // Placeholder: This should query Sales Orders (or however they are stored)
            // For now, returning empty list or example
            return new List<ReturnOrderForStockOutDto>();
        }

        public async Task<List<StockOutReturnDetailVm>> GetSalesOrderDetailsForReturn(int orderId)
        {
            // Placeholder: This should query Sales Order Details
            return new List<StockOutReturnDetailVm>();
        }

        private string GenerateDocCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.StockOutReturnHeaders.Count(h => h.CreatedDate.Date == DateTime.Today) + 1;
            return $"SOR-{date}-{count:D4}";
        }
    }
}
