using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class InventoryLayer
    {
        [Key]
        public int InventoryLayerId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public DateTime ReceiptDate { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal QuantityRemaining { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost => QuantityRemaining * UnitCost;

        [MaxLength(50)]
        public string BatchNo { get; set; }

        public DateTime ExpiryDate { get; set; }

        [Required]
        public int SourceDocumentId { get; set; } // StockInId or MovementId

        [Required]
        [MaxLength(20)]
        public string SourceDocumentType { get; set; } // "StockIn", "Movement"

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }
    }
}