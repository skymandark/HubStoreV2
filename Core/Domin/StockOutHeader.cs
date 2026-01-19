using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class StockOutHeader
    {
        [Key]
        public int StockOutId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DocCode { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int BranchId { get; set; }

        public int? TransferOrderId { get; set; }
        
        public int? ReturnOrderId { get; set; } // Link to ReturnOrder

        public int TransactionTypeId { get; set; } = 2; // Transfer Out

        public int Status { get; set; } // 0 Draft, 1 Approved, 2 Executed

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [MaxLength(1000)]
        public string Remarks { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [ForeignKey(nameof(TransferOrderId))]
        public virtual TransferOrderHeader TransferOrderHeader { get; set; }

        [ForeignKey(nameof(ReturnOrderId))]
        public virtual ReturnOrderHeader ReturnOrderHeader { get; set; }

        public virtual ICollection<StockOutDetail> StockOutDetails { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
    }
}