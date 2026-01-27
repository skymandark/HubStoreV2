using System;
using System.Collections.Generic;

namespace Core.ViewModels.StockOutReturnViewModels
{
    public class StockOutReturnVm
    {
        public int RequestId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? ClientId { get; set; }
        public string? ClientName { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? BranchStockId { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Reference { get; set; }
        public string? Remarks { get; set; }

        // Financial Totals
        public decimal TotalValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAddedDiscount { get; set; }
        public decimal TotalPrice { get; set; }

        // Lookup Lists
        public List<BranchDto>? BranchList { get; set; }
        public List<TransactionTypeDto>? TransactionTypeList { get; set; }
        public List<StockOutReturnDetailVm> StockOutReturnDetails { get; set; } = new();
    }

    public class StockOutReturnDetailVm
    {
        public int StockOutReturnDetailId { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? ItemPackageId { get; set; }
        public string? PackageName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }
        public decimal StockOutQuantity { get; set; } // Quantity from original StockOut
        public string? BatchNumber { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal AddedDiscountPercent { get; set; }
        public decimal AddedDiscountValue { get; set; }
        public decimal NetValue { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal VatValue { get; set; }
        public string? Remarks { get; set; }
        public int? ReturnReasonId { get; set; }
        public int? ReturnOrderDetailId { get; set; }

        // Backwards compatibility for old views
        public decimal Qty { get => Quantity; set => Quantity = value; }
        public decimal TotalValue { get => TotalPrice; set => TotalPrice = value; }
        public string? BatchNo { get => BatchNumber; set => BatchNumber = value; }
        public DateTime? ExpiryDate { get => ExpireDate; set => ExpireDate = value; }
        public string? Notes { get => Remarks; set => Remarks = value; }
        public decimal OrderQty { get; set; }
        public decimal PreviouslyExecutedQty { get; set; }
        public decimal RemainingQty { get; set; }
    }

    public class BranchDto
    {
        public int BranchId { get; set; }
        public string Name { get; set; }
    }

    public class TransactionTypeDto
    {
        public int TransactionTypeId { get; set; }
        public string Name { get; set; }
    }

    public class StockOutReturnListDto
    {
        public int StockOutReturnId { get; set; }
        public string DocCode { get; set; }
        public string ClientName { get; set; }
        public string BranchName { get; set; }
        public DateTime DocDate { get; set; }
        public string Status { get; set; }
        public decimal TotalValue { get; set; }
        public decimal NetValue { get; set; }
        public string Remarks { get; set; }
        // For old view if needed
        public string SupplierName { get => ClientName; set => ClientName = value; }
    }

    public class StockOutReturnRequestDto
    {
        public int StockOutReturnId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? ClientId { get; set; }
        public string? ClientName { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int? BranchStockId { get; set; }
        public int? ReturnOrderId { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Reference { get; set; }
        public string? Remarks { get; set; }

        // Financial Totals
        public decimal TotalValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAddedDiscount { get; set; }
        public decimal TotalPrice { get; set; }

        // Lookup Lists
        public List<BranchDto>? BranchList { get; set; }
        public List<TransactionTypeDto>? TransactionTypeList { get; set; }
        public List<StockOutReturnDetailVm> StockOutReturnDetails { get; set; } = new();
    }

    public class ReturnOrderForStockOutDto
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        // Backwards compatibility
        public int ReturnOrderId { get => OrderId; set => OrderId = value; }
        public string DocCode { get => OrderCode; set => OrderCode = value; }
        public DateTime DocDate { get => OrderDate; set => OrderDate = value; }
        public string SupplierName { get; set; }
    }
}