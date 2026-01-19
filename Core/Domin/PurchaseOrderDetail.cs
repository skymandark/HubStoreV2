using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class PurchaseOrderDetail
    {
        [Key]
        public int PurchaseOrderDetailId { get; set; }

        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int UnitId { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal OrderedQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Vat { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetValue { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal ReceivedQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal RemainingQuantity { get; set; }

        // Computed quantities (for compatibility with TransferOrder pattern)
        [NotMapped]
        public decimal ComputedReceivedQuantity => StockInDetails?.Where(si => si.StockInHeader.Status == 3).Sum(d => d.Qty) ?? 0;

        [NotMapped]
        public decimal ComputedRemainingQuantity => OrderedQuantity - ComputedReceivedQuantity;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(PurchaseOrderId))]
        public virtual PurchaseOrderHeader PurchaseOrderHeader { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(UnitId))]
        public virtual ItemUnit Unit { get; set; }

        public virtual ICollection<StockInDetail> StockInDetails { get; set; }
    }
}