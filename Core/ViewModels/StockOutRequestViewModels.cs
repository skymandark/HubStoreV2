using System;
using System.Collections.Generic;

namespace Core.ViewModels.StockOutRequestViewModels
{
    public class StockOutRequestDto
    {
        public int RequestId { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;
        public int BranchId { get; set; }
        public int BranchStockId { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int TransactionTypeId { get; set; }
        public int? SellOrderId { get; set; }
        public int? TransferOrderId { get; set; }
        public int? ToBranch { get; set; }
        public string Notes { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TotalAddedDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        public List<StockOutDetailRequestDto> StockOutDetailRequests { get; set; }

        public List<BranchDto> Branches { get; set; } = new();
        public List<ItemDto> Items { get; set; } = new();
    }

    public class StockOutDetailRequestDto
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int Serial { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNameEng { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public decimal ConsumerPrice { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? PackageId { get; set; }
        public decimal TotalValue { get; set; }
        public decimal MainDiscountPercent { get; set; }
        public decimal MainDiscountValue { get; set; }
        public decimal AddedDiscountPercent { get; set; }
        public decimal AddedDiscountValue { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
    }

    public class StockOutRequestListDto
    {
        public int RequestId { get; set; }
        public string DocCode { get; set; }
        public DateTime DocDate { get; set; }
        public string BranchName { get; set; }
        public string ClientName { get; set; }
        public string TransactionType { get; set; }
        public decimal TotalValue { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    // Additional DTOs if needed
    public class BranchDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }

    public class ItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNameEng { get; set; }
    }

    public class StockOutVm
    {
        public int RequestId { get; set; }
        public int SoId { get; set; }
        public bool IsSellOrder { get; set; }
        public bool IsTransferApprove { get; set; }
        
        public DateTime DocDate { get; set; } = DateTime.Today;
        public int BranchId { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int TransactionTypeId { get; set; }
        public int? ToBranchId { get; set; }
        public string Notes { get; set; }
        
        public decimal TotalValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        
        public List<StockOutDetailRequestDto> StockOutDetailRequests { get; set; } = new();
        public List<BranchDto> Branches { get; set; } = new();
        public List<ItemDto> Items { get; set; } = new();
    }
}