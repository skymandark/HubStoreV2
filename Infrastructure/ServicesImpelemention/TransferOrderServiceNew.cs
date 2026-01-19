using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.TransferOrderViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class TransferOrderServiceNew : ITransferOrderServiceNew
    {
        private readonly ApplicationDbContext _context;
        public TransferOrderServiceNew(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TransferOrderRequestDto> GetTransferOrder(int id)
        {
            var header = await _context.TransferOrderHeaders
                .Include(toh => toh.TransferOrderDetails)
                    .ThenInclude(tod => tod.Item)
                .Include(toh => toh.FromBranch)
                .Include(toh => toh.ToBranch)
                .Include(toh => toh.ShipmentType)
                .Include(toh => toh.StockOutHeaders).ThenInclude(so => so.StockOutDetails)
                .Include(toh => toh.StockInHeaders).ThenInclude(si => si.StockInDetails)
                .FirstOrDefaultAsync(toh => toh.TransferOrderId == id);

            if (header == null) return null;

            return new TransferOrderRequestDto
            {
                TransferOrderId = header.TransferOrderId,
                Reference = header.Reference,
                DocDate = header.DocDate,
                EntryDate = header.EntryDate,
                FromBranchId = header.FromBranchId,
                ToBranchId = header.ToBranchId,
                ShipmentTypeId = header.ShipmentTypeId ?? 0,
                Remarks = header.Notes,
                Status = header.TransferOrderStatusId,
                TransferOrderDetails = header.TransferOrderDetails.Select(tod => new TransferOrderDetailDto
                {
                    TransferOrderDetailId = tod.TransferOrderDetailId,
                    ItemId = tod.ItemId,
                    
                    RequestedQty = tod.Qty,
                    Notes = tod.Notes,
                    ShippedQty = header.StockOutHeaders
                        .Where(so => so.Status == 2 || so.Status == 1) 
                        .SelectMany(so => so.StockOutDetails)
                        .Where(d => d.ItemId == tod.ItemId)
                        .Sum(d => d.Qty),
                    ReceivedQty = header.StockInHeaders
                        .Where(si => si.Status == 3) 
                        .SelectMany(si => si.StockInDetails)
                        .Where(d => d.ItemId == tod.ItemId)
                        .Sum(d => d.Qty)
                }).ToList()
            };
        }

        public async Task<int> CreateTransferOrder(TransferOrderRequestDto dto)
        {
            var header = new TransferOrderHeader
            {
                TransferOrderCode = GenerateCode(),
                DocDate = dto.DocDate,
                EntryDate = dto.EntryDate,
                FromBranchId = dto.FromBranchId,
                ToBranchId = dto.ToBranchId,
                ShipmentTypeId = dto.ShipmentTypeId,
                Reference = dto.Reference,
                Notes = dto.Remarks,
                TransferOrderStatusId = 1, // Draft
                CreatedBy = "System"
            };

            _context.TransferOrderHeaders.Add(header);
            await _context.SaveChangesAsync();

            foreach (var detail in dto.TransferOrderDetails)
            {
                var tod = new TransferOrderDetail
                {
                    TransferOrderId = header.TransferOrderId,
                    ItemId = detail.ItemId,
                    Qty = detail.RequestedQty,
                    Notes = detail.Notes,
                    CreatedBy = "System"
                };
                _context.TransferOrderDetails.Add(tod);
            }

            await _context.SaveChangesAsync();
            return header.TransferOrderId;
        }

        public async Task UpdateTransferOrder(TransferOrderRequestDto dto)
        {
            var header = await _context.TransferOrderHeaders
                .Include(toh => toh.TransferOrderDetails)
                .FirstOrDefaultAsync(toh => toh.TransferOrderId == dto.TransferOrderId);

            if (header == null) throw new Exception("Transfer order not found");

            // Prevent editing if already approved (standard logic), unless we allows admin overrides.
            // For now, assume pending editing is okay.
            if (header.TransferOrderStatusId == 2) // Approved
            {
                 // throw new Exception("Cannot edit approved transfer order"); 
            }

            header.DocDate = dto.DocDate;
            header.FromBranchId = dto.FromBranchId;
            header.ToBranchId = dto.ToBranchId;
            header.ShipmentTypeId = dto.ShipmentTypeId;
            header.Reference = dto.Reference;
            header.Notes = dto.Remarks;
            header.ModifiedAt = DateTime.UtcNow;
            header.ModifiedBy = "System";

            // Remove existing details
            _context.TransferOrderDetails.RemoveRange(header.TransferOrderDetails);

            // Add new details
            foreach (var detail in dto.TransferOrderDetails)
            {
                var tod = new TransferOrderDetail
                {
                    TransferOrderId = header.TransferOrderId,
                    ItemId = detail.ItemId,
                    Qty = detail.RequestedQty,
                    Notes = detail.Notes,
                    CreatedBy = "System"
                };
                _context.TransferOrderDetails.Add(tod);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransferOrder(int id)
        {
            var header = await _context.TransferOrderHeaders
                .Include(toh => toh.TransferOrderDetails)
                .FirstOrDefaultAsync(toh => toh.TransferOrderId == id);

            if (header == null) throw new Exception("Transfer order not found");

            _context.TransferOrderDetails.RemoveRange(header.TransferOrderDetails);
            _context.TransferOrderHeaders.Remove(header);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantities(int id, List<TransferOrderDetailDto> details)
        {
            var header = await _context.TransferOrderHeaders
                .Include(toh => toh.TransferOrderDetails)
                .FirstOrDefaultAsync(toh => toh.TransferOrderId == id);

            if (header == null) throw new Exception("Transfer order not found");

            foreach (var detail in details)
            {
                var tod = header.TransferOrderDetails
                    .FirstOrDefault(d => d.TransferOrderDetailId == detail.TransferOrderDetailId);

                if (tod != null)
                {
                    tod.Qty = detail.RequestedQty;
                    tod.ModifiedAt = DateTime.UtcNow;
                    tod.ModifiedBy = "System";
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<TransferOrderListDto>> GetTransferOrders()
        {
            return await _context.TransferOrderHeaders
                .Include(toh => toh.FromBranch)
                .Include(toh => toh.ToBranch)
                .Include(toh => toh.ShipmentType)
                .Select(toh => new TransferOrderListDto
                {
                    RequestId = toh.TransferOrderId,
                    Reference = toh.Reference,
                    FromBranchName = toh.FromBranch.Name,
                    ToBranchName = toh.ToBranch.Name,
                    ShipmentTypeName = toh.ShipmentType != null ? toh.ShipmentType.Name : "",
                    EntryDate = toh.EntryDate,
                    DocDate = toh.DocDate,
                    StepName = "N/A",
                    // Use standard Enum ToString or helper for Arabic names
                    StatusName = toh.TransferOrderStatus.Name,
                    Notes = toh.Notes
                })
                .ToListAsync();
        }

        public async Task ApproveTransferOrder(int id, string user)
        {
            var header = await _context.TransferOrderHeaders.FindAsync(id);
            if (header == null) throw new Exception("Transfer order not found");
            
            if (header.TransferOrderStatusId == 2) throw new Exception("Already approved");

            header.ApprovedBy = user;
            header.ApprovedDate = DateTime.UtcNow;
            header.TransferOrderStatusId = 2; // Approved

            await _context.SaveChangesAsync();
        }

        public async Task ExecuteTransferOrder(int id, List<TransferOrderDetailDto> details, string user)
        {
            var header = await _context.TransferOrderHeaders
                .Include(h => h.TransferOrderDetails)
                .Include(h => h.StockOutHeaders).ThenInclude(so => so.StockOutDetails)
                .FirstOrDefaultAsync(h => h.TransferOrderId == id);

            if (header == null) throw new Exception("Transfer order not found");
            
            if (header.TransferOrderStatusId != 2)
                throw new Exception("Transfer Order must be approved before execution");

            // Creation of StockOut
            var stockOut = new StockOutHeader
            {
                DocCode = "SO-TO-" + header.TransferOrderCode + "-" + DateTime.Now.Ticks,
                DocDate = DateTime.UtcNow,
                BranchId = header.FromBranchId,
                TransferOrderId = header.TransferOrderId,
                Status = 2, // Executed
                TransactionTypeId = 2, // Transfer Out
                CreatedBy = user,
                Remarks = "Generated from Transfer Order " + header.TransferOrderCode
            };
            _context.StockOutHeaders.Add(stockOut);
            await _context.SaveChangesAsync();
            
            foreach (var inputDetail in details)
            {
                 var orderDetail = header.TransferOrderDetails.FirstOrDefault(d => d.ItemId == inputDetail.ItemId);
                 if (orderDetail == null) throw new Exception($"Item {inputDetail.ItemId} is not in the Transfer Order");

                 var previouslyShipped = header.StockOutHeaders
                    .Where(so => so.Status == 2) // Executed
                    .SelectMany(so => so.StockOutDetails)
                    .Where(d => d.ItemId == inputDetail.ItemId)
                    .Sum(d => d.Qty);

                 if (previouslyShipped + inputDetail.RequestedQty > orderDetail.Qty)
                    throw new Exception($"Cannot ship {inputDetail.RequestedQty} of Item {inputDetail.ItemId}. Requested: {orderDetail.Qty}, Already Shipped: {previouslyShipped}");

                 var stockOutDetail = new StockOutDetail
                 {
                     StockOutId = stockOut.StockOutId,
                     ItemId = inputDetail.ItemId,
                     Qty = inputDetail.RequestedQty,
                     BatchNo = inputDetail.BatchNo,
                     ExpiryDate = inputDetail.ExpiryDate,
                     CreatedBy = user,
                     Price = 0,
                     TotalValue = 0
                 };
                 _context.StockOutDetails.Add(stockOutDetail);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ReceiveTransferOrder(int id, List<TransferOrderDetailDto> details, string user)
        {
             var header = await _context.TransferOrderHeaders
                .Include(h => h.TransferOrderDetails)
                .Include(h => h.StockOutHeaders).ThenInclude(so => so.StockOutDetails)
                .Include(h => h.StockInHeaders).ThenInclude(si => si.StockInDetails)
                .FirstOrDefaultAsync(h => h.TransferOrderId == id);

            if (header == null) throw new Exception("Transfer order not found");
            
            if (header.TransferOrderStatusId != 2)
                throw new Exception("Transfer Order must be approved before receiving");

            var stockIn = new StockInHeader
            {
                DocCode = "SI-TO-" + header.TransferOrderCode + "-" + DateTime.Now.Ticks,
                DocDate = DateTime.UtcNow,
                BranchId = header.ToBranchId,
                TransferOrderId = header.TransferOrderId,
                Status = 3, // Received
                TransactionTypeId = 2, // Transfer In
                CreatedBy = user,
                Remarks = "Received from Transfer Order " + header.TransferOrderCode,
                SupplierId = null
            };
            _context.StockInHeaders.Add(stockIn);
            await _context.SaveChangesAsync();

            foreach (var inputDetail in details)
            {
                var totalShipped = header.StockOutHeaders
                    .Where(so => so.Status == 2)
                    .SelectMany(so => so.StockOutDetails)
                    .Where(d => d.ItemId == inputDetail.ItemId)
                    .Sum(d => d.Qty);

                var previouslyReceived = header.StockInHeaders
                    .Where(si => si.Status == 3)
                    .SelectMany(si => si.StockInDetails)
                    .Where(d => d.ItemId == inputDetail.ItemId)
                    .Sum(d => d.Qty);
                
                if (previouslyReceived + inputDetail.RequestedQty > totalShipped)
                    throw new Exception($"Cannot receive {inputDetail.RequestedQty} of Item {inputDetail.ItemId}. Total Shipped: {totalShipped}, Already Received: {previouslyReceived}");

                var stockInDetail = new StockInDetail
                {
                    StockInId = stockIn.StockInId,
                    ItemId = inputDetail.ItemId,
                    Qty = inputDetail.RequestedQty,
                    BatchNo = inputDetail.BatchNo,
                    ExpiryDate = inputDetail.ExpiryDate,
                    CreatedBy = user,
                    Price = 0,
                    TotalValue = 0
                };
                _context.StockInDetails.Add(stockInDetail);
            }
            
            await _context.SaveChangesAsync();
        }

        private string GenerateCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.TransferOrderHeaders.Count(toh => toh.CreatedDate.Date == DateTime.Today) + 1;
            return $"TO-{date}-{count:D4}";
        }
    }
}