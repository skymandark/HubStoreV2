using System;
using System.Collections.Generic;

namespace Core.ViewModels.StockInViewModels
{
    public class StockInRequestDto
    {
        public int StockInId { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;
        public int BranchId { get; set; }
        public int? SupplierId { get; set; }
        public string InvoiceNo { get; set; }
        public int? PurchaseOrderId { get; set; }
        public int? TransferOrderId { get; set; }
        public int? ReturnOrderId { get; set; }
        public int TransactionTypeId { get; set; } = 1;
        public int Status { get; set; } // 0 Draft, 1 Approved, 2 PendingApproval, 3 Received, 4 Rejected
        public decimal TotalValue { get; set; }
        public decimal VatValue { get; set; }
        public string Remarks { get; set; }
        public List<StockInDetailDto> StockInDetails { get; set; }

        public List<SupplierDto> Suppliers { get; set; } = new();
        public List<BranchDto> Branches { get; set; } = new();
        public List<ItemDto> Items { get; set; } = new();
    }

    public class StockInDetailDto
    {
        public int StockInDetailId { get; set; }
        public int ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal BonusQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal ConsumerPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal NetValue { get; set; }
        public decimal TotalValue { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? PurchaseOrderDetailId { get; set; }
        public int? TransferOrderDetailId { get; set; }
        public decimal AvailableStock { get; set; }
    }

    public class StockInListDto
    {
        public int StockInId { get; set; }
        public string DocCode { get; set; }
        public string PurchaseOrderCode { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public DateTime DocDate { get; set; }
        public string InvoiceNo { get; set; }
        public decimal TotalValue { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
    }

    public class BranchDto
    {
        public int BranchId { get; set; }
        public string Name { get; set; }
    }

    public class ItemDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
    }
}