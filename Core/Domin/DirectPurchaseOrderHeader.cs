using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class DirectPurchaseOrderHeader
    {
        [Key]
        public int DirectPurchaseOrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string DocCode { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        [Required]
        public DateTime EntryDate { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        public int? InvoiceId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        public int? ShipmentTypeId { get; set; }

        [ForeignKey("ShipmentTypeId")]
        public ShipmentType ShipmentType { get; set; }

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
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDiscount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAddedDiscount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal VatValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetValue { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        // Navigation
        public ICollection<DirectPurchaseOrderDetail> DirectPurchaseOrderDetails { get; set; } = new List<DirectPurchaseOrderDetail>();
    }
}