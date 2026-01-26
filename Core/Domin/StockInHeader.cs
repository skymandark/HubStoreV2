using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class StockInHeader
    {
        [Key]
        public int StockInId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DocCode { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int BranchId { get; set; }

        public int? SupplierId { get; set; }

        [MaxLength(50)]
        public string InvoiceNo { get; set; }

        public int? PurchaseOrderId { get; set; }

        public int? TransferOrderId { get; set; }

        public int? ReturnOrderId { get; set; }

        public int TransactionTypeId { get; set; } = 1;

        public int Status { get; set; } // 0 Draft, 1 Approved, 2 PendingApproval, 3 Received, 4 Rejected

        // Computed quantities
        [NotMapped]
        public decimal RequestedQty => PurchaseOrderHeader?.PurchaseOrderDetails?.Sum(d => d.OrderedQuantity) ?? 0;

        [NotMapped]
        public decimal ReceivedQty => StockInDetails?.Where(si => si.StockInHeader.Status == 3).Sum(d => d.Qty) ?? 0;

        // Derived Status (for compatibility with TransferOrder pattern)
        [NotMapped]
        public string DerivedStatus
        {
            get
            {
                if (ReceivedQty == 0)
                    return "Approved";
                if (ReceivedQty < RequestedQty)
                    return "PartiallyReceived";
                if (ReceivedQty >= RequestedQty)
                    return "Closed";
                return "Draft";
            }
        }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VatValue { get; set; }

        [MaxLength(1000)]
        public string Remarks { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        [MaxLength(100)]
        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey(nameof(PurchaseOrderId))]
        public virtual PurchaseOrderHeader PurchaseOrderHeader { get; set; }

        [ForeignKey(nameof(TransferOrderId))]
        public virtual TransferOrderHeader TransferOrderHeader { get; set; }

        [ForeignKey(nameof(ReturnOrderId))]
        public virtual ReturnOrderHeader ReturnOrderHeader { get; set; }

        public virtual ICollection<StockInDetail> StockInDetails { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
    }
}