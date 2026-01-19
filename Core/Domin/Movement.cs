using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public  class Movement
    {
        [Key]
        public int MovementId { get; set; }

        [Required]
        [MaxLength(50)]
        public string MovementCode { get; set; }

        [Required]
        public int MovementTypeId { get; set; }

        public int? BranchFromId { get; set; }

        public int? BranchToId { get; set; }

        public int? SupplierId { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int ApprovalStatusId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public bool IsPriority { get; set; } = false;

        [MaxLength(1000)]
        public string Notes { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(MovementTypeId))]
        public virtual MovementType MovementType { get; set; }

        [ForeignKey(nameof(BranchFromId))]
        public virtual Branch BranchFrom { get; set; }

        [ForeignKey(nameof(BranchToId))]
        public virtual Branch BranchTo { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual AppUser CreatedByUser { get; set; }

        [ForeignKey(nameof(ApprovalStatusId))]
        public virtual ApprovalStatus ApprovalStatus { get; set; }

        public virtual ICollection<MovementLine> MovementLines { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal TotalQuantityBase { get; set; }
        public decimal TotalValueCost { get; set; }
        public string InternalBarcode { get; set; }
        public string ExternalBarcode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
