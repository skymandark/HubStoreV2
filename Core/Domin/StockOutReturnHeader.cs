using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public enum ReceiptTypeStockOut
    {
        DirectReturnStockOut,
        ReturnIssue
    }

    public class StockOutReturnHeader
    {
        [Key]
        public int StockOutReturnId { get; set; }

        public ReceiptTypeStockOut ReceiptType { get; set; } = ReceiptTypeStockOut.DirectReturnStockOut;

        [Required]
        [MaxLength(50)]
        public string DocCode { get; set; } // Receipt ID

        public int TransactionType { get; set; } = 12;

        public int? ReturnOrderId { get; set; } // Order ID

        public int? ClientId { get; set; } // Client/Supplier

        public int? SupplierId { get; set; }

        [Required]
        public int BranchId { get; set; }

        public int? BranchStockId { get; set; } // Branch Stock

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DocDate { get; set; }

        [MaxLength(100)]
        public string? Reference { get; set; }

        [MaxLength(1000)]
        public string? Remarks { get; set; }

        public int Status { get; set; } // 0 Draft, 1 Approved, 2 PendingApproval, 3 Executed, 4 Rejected

        // Totals
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDiscount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAddedDiscount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VatValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }

        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [ForeignKey(nameof(BranchStockId))]
        public virtual Branch BranchStock { get; set; }

        [ForeignKey(nameof(ReturnOrderId))]
        public virtual ReturnOrderHeader ReturnOrderHeader { get; set; }

        // Assuming Client and Supplier are entities
        // [ForeignKey(nameof(ClientId))]
        // public virtual object Client { get; set; } // Placeholder

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<StockOutReturnDetail> StockOutReturnDetails { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
    }
}