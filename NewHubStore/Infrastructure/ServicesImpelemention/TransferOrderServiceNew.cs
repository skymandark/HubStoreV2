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
                .Include(toh => toh.FromBranch)
                .Include(toh => toh.ToBranch)
                .Include(toh => toh.ShipmentType)
                .Include(toh => toh.TransferOrderStatus)
                .FirstOrDefaultAsync(toh => toh.TransferOrderId == id);

            if (header == null) return null;

            return new TransferOrderRequestDto
            {
                RequestId = header.TransferOrderId,
                DocDate = header.DocDate,
                EntryDate = header.EntryDate,
                FromBranch = header.FromBranchId,
                ToBranch = header.ToBranchId,
                ShipmentTypeId = header.ShipmentTypeId ?? 0,
                RequestTransferOrderHeader = new RequestTransferOrderHeaderDto
                {
                    DocDate = header.DocDate,
                    Reference = header.Reference,
                    FromBranchId = header.FromBranchId,
                    ToBranchId = header.ToBranchId,
                    Notes = header.Notes
                },
                TransferOrderDetails = header.TransferOrderDetails.Select(tod => new TransferOrderDetailDto
                {
                    TransferOrderDetailId = tod.TransferOrderDetailId,
                    ItemId = tod.ItemId,
                    Quantity = tod.Quantity,
                    Price = tod.Price,
                    CostPrice = tod.CostPrice,
                    Notes = tod.Notes
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
                FromBranchId = dto.FromBranch,
                ToBranchId = dto.ToBranch,
                ShipmentTypeId = dto.ShipmentTypeId,
                Reference = dto.RequestTransferOrderHeader.Reference,
                Notes = dto.RequestTransferOrderHeader.Notes,
                TransferOrderStatusId = 1, // Pending
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
                    Quantity = detail.Quantity,
                    Price = detail.Price,
                    CostPrice = detail.CostPrice,
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
                .FirstOrDefaultAsync(toh => toh.TransferOrderId == dto.RequestId);

            if (header == null) throw new Exception("Transfer order not found");

            header.DocDate = dto.DocDate;
            header.FromBranchId = dto.FromBranch;
            header.ToBranchId = dto.ToBranch;
            header.ShipmentTypeId = dto.ShipmentTypeId;
            header.Reference = dto.RequestTransferOrderHeader.Reference;
            header.Notes = dto.RequestTransferOrderHeader.Notes;
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
                    Quantity = detail.Quantity,
                    Price = detail.Price,
                    CostPrice = detail.CostPrice,
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
                    tod.Quantity = detail.Quantity;
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
                .Include(toh => toh.TransferOrderStatus)
                .Select(toh => new TransferOrderListDto
                {
                    TransferOrderId = toh.TransferOrderId,
                    TransferOrderCode = toh.TransferOrderCode,
                    Reference = toh.Reference,
                    FromBranchName = toh.FromBranch.Name,
                    ToBranchName = toh.ToBranch.Name,
                    ShipmentTypeName = toh.ShipmentType.Name,
                    EntryDate = toh.EntryDate,
                    DocDate = toh.DocDate,
                    Stage = "N/A", // Implement if needed
                    Status = toh.TransferOrderStatus.Name,
                    Notes = toh.Notes
                })
                .ToListAsync();
        }

        private string GenerateCode()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = _context.TransferOrderHeaders.Count(toh => toh.CreatedDate.Date == DateTime.Today) + 1;
            return $"TO-{date}-{count:D4}";
        }
    }
}