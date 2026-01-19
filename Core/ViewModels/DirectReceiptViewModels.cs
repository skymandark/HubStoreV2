using System;
using System.Collections.Generic;

namespace Core.ViewModels.DirectReceiptViewModels
{
    public class PurchaseOrderRequestVM
    {
        public int RequestId { get; set; }
        public PurchaseOrderHeaderRequest PurchaseOrderHeaderRequest { get; set; }
        public List<ItemTransferOrderVm> Items { get; set; }
        public List<string> Images { get; set; } // For uploaded images
    }

    public class PurchaseOrderHeaderRequest
    {
        public DateTime DocDate { get; set; }
        public int SupplierId { get; set; }
        public int BranchId { get; set; }
        public string RemarksArab { get; set; }
        public string ReferenceInvoiceNumber { get; set; }
    }

    public class ItemTransferOrderVm
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal CustomerPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Vat { get; set; }
        public decimal TotalValue { get; set; }
        public decimal BonusQuantity { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    // For listing
    public class DirectReceiptListDto
    {
        public int DirectReceiptId { get; set; }
        public string DirectReceiptCode { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DocDate { get; set; }
        public string Status { get; set; }
        public string RemarksArab { get; set; }
        public List<string> Images { get; set; }
    }
}