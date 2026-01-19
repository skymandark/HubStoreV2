using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public enum ReceiptType
    {
        NormalPurchase,
        Return,
        DirectPurchase
    }

    public enum PurchaseOrderStatus
    {
        Draft,
        Submitted,
        Approved,
        Rejected,
        PartiallyReceived,
        FullyReceived,
        Closed
    }

    public class PurchaseOrderHeader
    {
        [Key]
        public int PurchaseOrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PurchaseOrderCode { get; set; } // Auto Generated

        [Required]
        public DateTime DocDate { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int BranchId { get; set; }

        public int? BranchStockId { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [MaxLength(500)]
        public string PaymentTerms { get; set; }

        public int? CurrencyId { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal ExchangeRate { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBeforeDiscount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VatValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetTotal { get; set; }

        [Required]
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;

        // Computed quantities
        [NotMapped]
        public decimal TotalOrderedQty => PurchaseOrderDetails?.Sum(d => d.OrderedQuantity) ?? 0;

        [NotMapped]
        public decimal TotalReceivedQty => StockInHeaders?.Where(si => si.Status == 3).Sum(si => si.StockInDetails?.Sum(d => d.Qty) ?? 0) ?? 0;

        // Derived Status (for compatibility, mapping to Enum names)
        [NotMapped]
        public string DerivedStatus
        {
            get
            {
                if (TotalReceivedQty == 0)
                    return PurchaseOrderStatus.Approved.ToString();
                if (TotalReceivedQty < TotalOrderedQty)
                    return "PartiallyReceived"; 
                if (TotalReceivedQty >= TotalOrderedQty)
                    return PurchaseOrderStatus.Closed.ToString();
                return PurchaseOrderStatus.Draft.ToString();
            }
        }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }

        [MaxLength(100)]
        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [ForeignKey(nameof(BranchStockId))]
        public virtual Branch BranchStock { get; set; }

        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
        public virtual ICollection<StockInHeader> StockInHeaders { get; set; }
    }
}