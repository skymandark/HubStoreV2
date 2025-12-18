using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class MovementLine
    {
        [Key]
        public int MovementLineId { get; set; }

        [Required]
        public int MovementId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UnitCode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal QtyInput { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal ConversionUsedToBase { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal QtyBase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        public int MovementTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [MaxLength(100)]
        public string ExternalBarcode { get; set; }

        [MaxLength(100)]
        public string InternalBarcode { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        [ForeignKey(nameof(MovementId))]
        public virtual Movement Movement { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [ForeignKey(nameof(MovementTypeId))]
        public virtual MovementType MovementType { get; set; }
        public bool IsApproved { get; set; }
        public int Quantity { get; set; }
        public int LineNumber { get; set; }
    }
}

