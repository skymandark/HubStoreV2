using System;
using System.Collections.Generic;
using Core.Domin;

namespace Core.ViewModels.PurchaseOrderViewModels
{
    public class PurchaseOrderRequestDto
    {
        public int PurchaseOrderId { get; set; }
        public ReceiptType ReceiptType { get; set; }
        public string PurchaseOrderCode { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime EntryDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierDelegate { get; set; }
        public string InvoiceId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int? BranchStockId { get; set; }
        public string Reference { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool CreditPayment { get; set; }
        public DateTime? DueDate { get; set; }
        public int? PaymentPeriodDays { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public decimal TotalValue { get; set; }
        public List<PurchaseOrderDetailDto> PurchaseOrderDetails { get; set; }
        // Dropdown data
        public List<SupplierDto> Suppliers { get; set; }
        public List<BranchDto> Branches { get; set; }
        public List<ItemDto> Items { get; set; }
        public List<ItemUnitDto> ItemPackages { get; set; }
    }

    public class PurchaseOrderDetailDto
    {
        public int PurchaseOrderDetailId { get; set; }
        public int LineSerialNumber { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int? ItemPackageId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
        public decimal MainDiscountPercent { get; set; }
        public decimal MainDiscountValue { get; set; }
        public decimal AddedDiscountPercent { get; set; }
        public decimal AddedDiscountValue { get; set; }
        public decimal BonusQuantity { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        public string Remarks { get; set; }
        public decimal CostPrice { get; set; }
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


    // For listing
    public class PurchaseOrderListDto
    {
        public int PurchaseOrderId { get; set; }
        public string PurchaseOrderCode { get; set; }
        public string Reference { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DocDate { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public decimal TotalValue { get; set; }
    }
}