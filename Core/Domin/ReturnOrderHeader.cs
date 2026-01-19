using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public enum ReturnType
    {
        SupplierReturn,
        CustomerReturn
    }

    public enum ReturnOrderStatus
    {
        Draft,
        PendingApproval,
        Approved,
        Executed,
        PartiallyReceived,
        Closed,
        Rejected
    }

    public class ReturnOrderHeader
    {
        [Key]
        public int ReturnOrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DocCode { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int BranchId { get; set; }

        public ReturnType ReturnType { get; set; }

        public int? SupplierId { get; set; }

        public int? ClientId { get; set; }

        public int? ReturnReasonId { get; set; }

        // Derived Status
        [NotMapped]
        public ReturnOrderStatus Status
        {
            get
            {
                if (ApprovedQty == 0) return ReturnOrderStatus.Draft;
                if (ApprovedQty < RequestedQty) return ReturnOrderStatus.PendingApproval;
                if (ReceivedQty >= RequestedQty) return ReturnOrderStatus.Closed;
                return ReturnOrderStatus.Approved;
            }
        }

        [MaxLength(1000)]
        public string Remarks { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }

        [MaxLength(100)]
        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // الكميات المحسوبة
        [NotMapped]
        public decimal RequestedQty => ReturnOrderDetails?.Sum(d => d.Qty) ?? 0;

        [NotMapped]
        public decimal ApprovedQty => StockOutReturnHeaders?.Where(so => so.Status == 2 || so.Status == 1).Sum(so => so.StockOutReturnDetails?.Sum(d => d.Qty) ?? 0) ?? 0;

        [NotMapped]
        public decimal ReceivedQty => StockInHeaders?.Where(si => si.Status == 3).Sum(si => si.StockInDetails?.Sum(d => d.Qty) ?? 0) ?? 0;

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

        // Assuming Client is another entity, add if needed

        public virtual ICollection<ReturnOrderDetail> ReturnOrderDetails { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
        public virtual ICollection<StockOutReturnHeader> StockOutReturnHeaders { get; set; }
        public virtual ICollection<StockInHeader> StockInHeaders { get; set; }
    }
}