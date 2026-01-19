using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class ReturnOrderDetail
    {
        [Key]
        public int ReturnOrderDetailId { get; set; }

        [Required]
        public int ReturnOrderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Qty { get; set; }

        public int? ReturnReasonId { get; set; }

        // Original Movement Detail - For SupplierReturn links to StockInDetail, For CustomerReturn links to StockOutDetail
        public int? OriginalStockInDetailId { get; set; } // FK to StockInDetail (for SupplierReturn)

        public int? OriginalStockOutDetailId { get; set; } // FK to StockOutDetail (for CustomerReturn)

        [MaxLength(50)]
        public string? BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }

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
        [ForeignKey(nameof(ReturnOrderId))]
        public virtual ReturnOrderHeader ReturnOrderHeader { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(OriginalStockInDetailId))]
        public virtual StockInDetail OriginalStockInDetail { get; set; }

        [ForeignKey(nameof(OriginalStockOutDetailId))]
        public virtual StockOutDetail OriginalStockOutDetail { get; set; }

        public virtual ICollection<StockOutReturnDetail> StockOutReturnDetails { get; set; }
    }
}