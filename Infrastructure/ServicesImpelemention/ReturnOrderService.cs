using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.ReturnOrderViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class ReturnOrderService : IReturnOrderService
    {
        private readonly ApplicationDbContext _context;
        public ReturnOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateReturnOrder(ReturnOrderRequestDto dto, string user)
        {
            var header = new ReturnOrderHeader
            {
                DocCode = GenerateDocCode(),
                DocDate = dto.DocDate,
                BranchId = dto.BranchId,
                ReturnType = dto.ReturnType,
                SupplierId = dto.SupplierId,
                ClientId = dto.ClientId,
                ReturnReasonId = dto.ReturnReasonId,
                Remarks = dto.Remarks,
                CreatedBy = user
            };

            _context.ReturnOrderHeaders.Add(header);
            await _context.SaveChangesAsync();

            foreach (var detail in dto.ReturnOrderDetails)
            {
                // Validation Logic
                if (dto.ReturnType == ReturnType.SupplierReturn)
                {
                    var original = await _context.StockInDetails.FindAsync(detail.OriginalMovementDetailId);
                    if (original == null) throw new Exception($"Original StockIn Detail {detail.OriginalMovementDetailId} not found");

                    var returnedQty = await _context.ReturnOrderDetails
                        .Where(x => x.OriginalStockInDetailId == detail.OriginalMovementDetailId && !x.IsDeleted)
                        .SumAsync(x => x.Qty);

                    if (returnedQty + detail.Qty > original.Qty)
                        throw new Exception($"Cannot return {detail.Qty}. Already returned {returnedQty} of {original.Qty}");
                }
                else if (dto.ReturnType == ReturnType.CustomerReturn)
                {
                    var original = await _context.StockOutDetails.FindAsync(detail.OriginalMovementDetailId);
                    if (original == null) throw new Exception($"Original StockOut Detail {detail.OriginalMovementDetailId} not found");

                    var returnedQty = await _context.ReturnOrderDetails
                        .Where(x => x.OriginalStockOutDetailId == detail.OriginalMovementDetailId && !x.IsDeleted)
                        .SumAsync(x => x.Qty);

                    if (returnedQty + detail.Qty > original.Qty)
                        throw new Exception($"Cannot return {detail.Qty}. Already returned {returnedQty} of {original.Qty}");
                }

                var rod = new ReturnOrderDetail
                {
                    ReturnOrderId = header.ReturnOrderId,
                    ItemId = detail.ItemId,
                    Qty = detail.Qty,
                    ReturnReasonId = detail.ReturnReasonId,
                    // Set OriginalMovementDetailId based on type
                    OriginalStockInDetailId = dto.ReturnType == ReturnType.SupplierReturn ? detail.OriginalMovementDetailId : null,
                    OriginalStockOutDetailId = dto.ReturnType == ReturnType.CustomerReturn ? detail.OriginalMovementDetailId : null,
                    BatchNo = detail.BatchNo,
                    ExpiryDate = detail.ExpiryDate,
                    Notes = detail.Notes,
                    CreatedBy = user
                };
                _context.ReturnOrderDetails.Add(rod);
            }

            await _context.SaveChangesAsync();
            return header.ReturnOrderId;
        }

        public async Task UpdateReturnOrder(ReturnOrderRequestDto dto, string user)
        {
            var header = await _context.ReturnOrderHeaders
                .Include(roh => roh.ReturnOrderDetails)
                .FirstOrDefaultAsync(roh => roh.ReturnOrderId == dto.ReturnOrderId);

            if (header == null) throw new Exception("Return order not found");

            // Prevent editing if approved
            if (header.Status == ReturnOrderStatus.Approved || header.Status == ReturnOrderStatus.Executed)
                throw new Exception("Cannot edit approved or executed return order");

            header.DocDate = dto.DocDate;
            header.BranchId = dto.BranchId;
            header.ReturnType = dto.ReturnType;
            header.SupplierId = dto.SupplierId;
            header.ClientId = dto.ClientId;
            header.ReturnReasonId = dto.ReturnReasonId;
            header.Remarks = dto.Remarks;
            header.ModifiedBy = user;
            header.ModifiedAt = DateTime.UtcNow;

            // Remove existing details
            _context.ReturnOrderDetails.RemoveRange(header.ReturnOrderDetails);

            // Add new details
            foreach (var detail in dto.ReturnOrderDetails)
            {
                // Validation Logic
                if (dto.ReturnType == ReturnType.SupplierReturn)
                {
                    var original = await _context.StockInDetails.FindAsync(detail.OriginalMovementDetailId);
                    if (original == null) throw new Exception($"Original StockIn Detail {detail.OriginalMovementDetailId} not found");

                    var returnedQty = await _context.ReturnOrderDetails
                        .Where(x => x.OriginalStockInDetailId == detail.OriginalMovementDetailId && x.ReturnOrderId != header.ReturnOrderId && !x.IsDeleted)
                        .SumAsync(x => x.Qty);

                    if (returnedQty + detail.Qty > original.Qty)
                        throw new Exception($"Cannot return {detail.Qty}. Already returned {returnedQty} of {original.Qty}");
                }
                else if (dto.ReturnType == ReturnType.CustomerReturn)
                {
                    var original = await _context.StockOutDetails.FindAsync(detail.OriginalMovementDetailId);
                    if (original == null) throw new Exception($"Original StockOut Detail {detail.OriginalMovementDetailId} not found");

                    var returnedQty = await _context.ReturnOrderDetails
                        .Where(x => x.OriginalStockOutDetailId == detail.OriginalMovementDetailId && x.ReturnOrderId != header.ReturnOrderId && !x.IsDeleted)
                        .SumAsync(x => x.Qty);

                    if (returnedQty + detail.Qty > original.Qty)
                        throw new Exception($"Cannot return {detail.Qty}. Already returned {returnedQty} of {original.Qty}");
                }

                var rod = new ReturnOrderDetail
                {
                    ReturnOrderId = header.ReturnOrderId,
                    ItemId = detail.ItemId,
                    Qty = detail.Qty,
                    ReturnReasonId = detail.ReturnReasonId,
                    OriginalStockInDetailId = dto.ReturnType == ReturnType.SupplierReturn ? detail.OriginalMovementDetailId : null,
                    OriginalStockOutDetailId = dto.ReturnType == ReturnType.CustomerReturn ? detail.OriginalMovementDetailId : null,
                    BatchNo = detail.BatchNo,
                    ExpiryDate = detail.ExpiryDate,
                    Notes = detail.Notes,
                    CreatedBy = user
                };
                _context.ReturnOrderDetails.Add(rod);
            }

            await _context.SaveChangesAsync();
        }

        public async Task ApproveReturnOrder(int id, string user)
        {
            var header = await _context.ReturnOrderHeaders.FindAsync(id);
            if (header == null) throw new Exception("Return order not found");

            if (header.ApprovedDate.HasValue) throw new Exception("Already approved");

            header.ApprovedBy = user;
            header.ApprovedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ExecuteReturnOrder(int id, string user)
        {
            var header = await _context.ReturnOrderHeaders
                .Include(h => h.ReturnOrderDetails)
                .FirstOrDefaultAsync(h => h.ReturnOrderId == id);

            if (header == null) throw new Exception("Return order not found");
            
            if (header.Status != ReturnOrderStatus.Approved) 
                throw new Exception("Return Order must be approved before execution.");
            
            // Check if already executed
            if (header.Status == ReturnOrderStatus.Executed || header.Status == ReturnOrderStatus.Closed)
                 throw new Exception("Return Order is already executed.");

            // Create StockOut/StockIn based on type
            if (header.ReturnType == ReturnType.SupplierReturn)
            {
                // Create StockOut
                var stockOut = new StockOutHeader
                {
                    DocCode = "SO-" + header.DocCode,
                    DocDate = DateTime.UtcNow,
                    BranchId = header.BranchId,
                    ReturnOrderId = header.ReturnOrderId,
                    Status = 2, // Executed/Completed
                    CreatedBy = user,
                    TransactionTypeId = 2, // Transfer Out / Return
                    Remarks = "Generated from Supplier Return Order " + header.DocCode
                };
                _context.StockOutHeaders.Add(stockOut);
                await _context.SaveChangesAsync();

                foreach (var d in header.ReturnOrderDetails)
                {
                    decimal price = 0;
                    if (d.OriginalStockInDetailId.HasValue)
                    {
                        var original = await _context.StockInDetails.FindAsync(d.OriginalStockInDetailId.Value);
                        if (original != null) price = original.Price;
                    }

                    var detail = new StockOutDetail
                    {
                        StockOutId = stockOut.StockOutId,
                        ItemId = d.ItemId,
                        Qty = d.Qty,
                        Price = price,
                        TotalValue = d.Qty * price,
                        BatchNo = d.BatchNo,
                        ExpiryDate = d.ExpiryDate,
                        CreatedBy = user
                    };
                    _context.StockOutDetails.Add(detail);
                }
                
                // Update Total Value of Header
                stockOut.TotalValue = await _context.StockOutDetails.Where(x => x.StockOutId == stockOut.StockOutId).SumAsync(x => x.TotalValue);
            }
            else // Customer Return
            {
                // Create StockIn
                var stockIn = new StockInHeader
                {
                    DocCode = "SI-" + header.DocCode,
                    DocDate = DateTime.UtcNow,
                    BranchId = header.BranchId,
                    ReturnOrderId = header.ReturnOrderId, // Link
                    Status = 2, // Executed/Completed
                    SupplierId = null, // Customer Return doesn't have Supplier
                    CreatedBy = user,
                    TransactionTypeId = 1, // Purchase / Input
                    Remarks = "Generated from Customer Return Order " + header.DocCode
                };
                _context.StockInHeaders.Add(stockIn);
                await _context.SaveChangesAsync();

                foreach (var d in header.ReturnOrderDetails)
                {
                     decimal price = 0;
                    if (d.OriginalStockOutDetailId.HasValue)
                    {
                        var original = await _context.StockOutDetails.FindAsync(d.OriginalStockOutDetailId.Value);
                        if (original != null) price = original.Price;
                    }

                    var detail = new StockInDetail
                    {
                        StockInId = stockIn.StockInId,
                        ItemId = d.ItemId,
                        Qty = d.Qty,
                        Price = price, 
                        TotalValue = d.Qty * price,
                        BatchNo = d.BatchNo,
                        ExpiryDate = d.ExpiryDate,
                        CreatedBy = user
                    };
                    _context.StockInDetails.Add(detail);
                }

                // Update Total Value
                stockIn.TotalValue = await _context.StockInDetails.Where(x => x.StockInId == stockIn.StockInId).SumAsync(x => x.TotalValue);
                stockIn.VatValue = stockIn.TotalValue * 0.14m; // Approximate
            }

            // Status will be computed as Executed when StockOutReturnHeaders exist
            
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReturnOrderListDto>> GetReturnOrders()
        {
            return await _context.ReturnOrderHeaders
                .Include(roh => roh.Branch)
                .Include(roh => roh.Supplier)
                .Select(roh => new ReturnOrderListDto
                {
                    ReturnOrderId = roh.ReturnOrderId,
                    DocCode = roh.DocCode,
                    ReturnType = roh.ReturnType.ToString(),
                    SupplierName = roh.Supplier != null ? roh.Supplier.Name : "",
                    ClientName = "",
                    BranchName = roh.Branch.Name,
                    DocDate = roh.DocDate,
                    Status = roh.Status.ToString(),
                    RequestedQty = roh.RequestedQty,
                    ApprovedQty = roh.ApprovedQty,
                    ReceivedQty = roh.ReceivedQty,
                    Remarks = roh.Remarks
                })
                .ToListAsync();
        }

        private string GenerateDocCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.ReturnOrderHeaders.Count(roh => roh.CreatedDate.Date == DateTime.Today) + 1;
            return $"RO-{date}-{count:D4}";
        }
    }
}