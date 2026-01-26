using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.DirectPurchaseOrderViewModels
{
    public class CreateDirectPurchaseOrderViewModel
    {
        [Required]
        public DateTime DocDate { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public int? InvoiceId { get; set; }

        [Required]
        public int BranchId { get; set; }

        public int? ShipmentTypeId { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public bool CreditPayment { get; set; }

        public int? PaymentPeriodDays { get; set; }

        public DateTime? DueDate { get; set; }

        public int? CustomerId { get; set; }

        [StringLength(500)]
        public string Reference { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [Required]
        public List<CreateDirectPurchaseOrderDetailViewModel> Details { get; set; } = new List<CreateDirectPurchaseOrderDetailViewModel>();

        // Calculated fields
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAddedDiscount { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
    }

    public class CreateDirectPurchaseOrderDetailViewModel
    {
        [Required]
        public int ItemId { get; set; }

        public int? ItemPackageId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BonusQuantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, 100)]
        public decimal MainDiscountPercent { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MainDiscountValue { get; set; }

        [Range(0, 100)]
        public decimal AddedDiscountPercent { get; set; }

        [Range(0, double.MaxValue)]
        public decimal AddedDiscountValue { get; set; }

        [Range(0, double.MaxValue)]
        public decimal VatRate { get; set; }

        [StringLength(500)]
        public string RemarksArab { get; set; }

        [StringLength(500)]
        public string RemarksEng { get; set; }

        // Calculated fields
        public decimal TotalValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        public decimal AvailableStock { get; set; }
    }

    public class UpdateDirectPurchaseOrderViewModel : CreateDirectPurchaseOrderViewModel
    {
        [Required]
        public int DirectPurchaseOrderId { get; set; }
    }

    public class DirectPurchaseOrderViewModel
    {
        public int DirectPurchaseOrderId { get; set; }
        public string DocCode { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime EntryDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? InvoiceId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int? ShipmentTypeId { get; set; }
        public string ShipmentTypeName { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool CreditPayment { get; set; }
        public int? PaymentPeriodDays { get; set; }
        public DateTime? DueDate { get; set; }
        public int? CustomerId { get; set; }
        public string Reference { get; set; }
        public string Notes { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAddedDiscount { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public List<DirectPurchaseOrderDetailViewModel> Details { get; set; } = new List<DirectPurchaseOrderDetailViewModel>();
    }

    public class DirectPurchaseOrderDetailViewModel
    {
        public int DirectPurchaseOrderDetailId { get; set; }
        public int Serial { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int? ItemPackageId { get; set; }
        public string ItemPackageName { get; set; }
        public decimal Quantity { get; set; }
        public decimal BonusQuantity { get; set; }
        public decimal InQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
        public decimal MainDiscountPercent { get; set; }
        public decimal MainDiscountValue { get; set; }
        public decimal AddedDiscountPercent { get; set; }
        public decimal AddedDiscountValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal NetValue { get; set; }
        public string RemarksArab { get; set; }
        public string RemarksEng { get; set; }
    }

    public class DirectPurchaseOrderListViewModel
    {
        public int DirectPurchaseOrderId { get; set; }
        public string DocCode { get; set; }
        public DateTime DocDate { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public decimal NetValue { get; set; }
        public string StatusName { get; set; }
        public string CreatedBy { get; set; }
    }
}