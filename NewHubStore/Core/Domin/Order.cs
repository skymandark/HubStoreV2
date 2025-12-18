using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string OrderCode { get; set; }

        [Required]
        public int OrderTypeId { get; set; }

        public int? RelatedDocumentId { get; set; }

        public int? SupplierId { get; set; }

        public int? BranchFromId { get; set; }

        public int? BranchToId { get; set; }

        [Required]
        public string RequestedByUserId { get; set; }

        [Required]
        public DateTime RequestedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [Required]
        public int ApprovalStatusId { get; set; }

        public bool IsManualClosed { get; set; } = false;

        [Column(TypeName = "decimal(18,6)")]
        public decimal TotalQuantityBase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValueCost { get; set; }

        public int PriorityFlag { get; set; } = 0;

        public DateTime? SLA_DueDate { get; set; }

        [MaxLength(100)]
        public string ExternalBarcode { get; set; }

        [MaxLength(100)]
        public string InternalBarcode { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey(nameof(OrderTypeId))]
        public virtual OrderType OrderType { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey(nameof(BranchFromId))]
        public virtual Branch BranchFrom { get; set; }

        [ForeignKey(nameof(BranchToId))]
        public virtual Branch BranchTo { get; set; }

        [ForeignKey(nameof(RequestedByUserId))]
        public virtual AppUser RequestedByUser { get; set; }

        [ForeignKey(nameof(ApprovalStatusId))]
        public virtual ApprovalStatus ApprovalStatus { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }

        [MaxLength(500)]
        public string ClosingReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
