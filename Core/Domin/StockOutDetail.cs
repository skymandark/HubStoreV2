using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class StockOutDetail
    {
        [Key]
        public int StockOutDetailId { get; set; }

        [Required]
        public int StockOutId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Qty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [MaxLength(50)]
        public string BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int? TransferOrderDetailId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey(nameof(StockOutId))]
        public virtual StockOutHeader StockOutHeader { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(TransferOrderDetailId))]
        public virtual TransferOrderDetail TransferOrderDetail { get; set; }
    }
}