using System;
using System.Collections.Generic;
using Core.Domin;

namespace Core.ViewModels.DirectReturnStockOutViewModels
{
    public class DirectReturnStockOutRequestDto
    {
        public int StockOutReturnId { get; set; }
        public ReceiptTypeStockOut ReceiptType { get; set; }
        public string DocCode { get; set; }
        public int TransactionType { get; set; }
        public int? ReturnOrderId { get; set; }
        public int? ClientId { get; set; }
        public int? SupplierId { get; set; }
        public int BranchId { get; set; }
        public int? BranchStockId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DocDate { get; set; }
        public string Reference { get; set; }
        public string Remarks { get; set; }
        public List<DirectReturnStockOutDetailDto> StockOutReturnDetails { get; set; }
        // Dropdown data
        public List<ReturnOrderDto> ReturnOrders { get; set; }
        public List<ClientDto> Clients { get; set; }
        public List<SupplierDto> Suppliers { get; set; }
        public List<BranchDto> Branches { get; set; }
        public List<ItemDto> Items { get; set; }
        public List<ItemUnitDto> ItemPackages { get; set; }
    }

    public class DirectReturnStockOutDetailDto
    {
        public int StockOutReturnDetailId { get; set; }
        public int LineSerialNumber { get; set; }
        public int ItemId { get; set; }
        public int? ItemPackageId { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal Qty { get; set; }
        public decimal BonusQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal ConsumerPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal Value { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal AddedDiscountPercent { get; set; }
        public decimal AddedDiscountValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        public decimal TotalValue { get; set; }
        public string Remarks { get; set; }
        public int? ReturnReasonId { get; set; }
        public int? ReturnOrderDetailId { get; set; }
    }

    public class ReturnOrderDto
    {
        public int ReturnOrderId { get; set; }
        public string DocCode { get; set; }
        public string SupplierName { get; set; }
        public string ClientName { get; set; }
    }

    public class ClientDto
    {
        public int ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class BranchDto
    {
        public int BranchId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ItemDto
    {
        public int ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ItemUnitDto
    {
        public int ItemUnitId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class DirectReturnStockOutListDto
    {
        public int StockOutReturnId { get; set; }
        public string DocCode { get; set; }
        public string ReturnOrderCode { get; set; }
        public string SupplierName { get; set; }
        public string ClientName { get; set; }
        public string BranchName { get; set; }
        public DateTime DocDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}