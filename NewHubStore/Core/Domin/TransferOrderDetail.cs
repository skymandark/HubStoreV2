using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class TransferOrderDetail
    {
        [Key]
        public int TransferOrderDetailId { get; set; }

        [Required]
        public int TransferOrderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

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

        [ForeignKey(nameof(TransferOrderId))]
        public virtual TransferOrderHeader TransferOrderHeader { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }
    }
}