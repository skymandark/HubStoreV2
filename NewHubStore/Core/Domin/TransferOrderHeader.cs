using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class TransferOrderHeader
    {
        [Key]
        public int TransferOrderId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TransferOrderCode { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int FromBranchId { get; set; }

        [Required]
        public int ToBranchId { get; set; }

        public int? ShipmentTypeId { get; set; }

        [MaxLength(100)]
        public string Reference { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [Required]
        public int TransferOrderStatusId { get; set; }

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

        [ForeignKey(nameof(FromBranchId))]
        public virtual Branch FromBranch { get; set; }

        [ForeignKey(nameof(ToBranchId))]
        public virtual Branch ToBranch { get; set; }

        [ForeignKey(nameof(ShipmentTypeId))]
        public virtual ShipmentType ShipmentType { get; set; }

        [ForeignKey(nameof(TransferOrderStatusId))]
        public virtual TransferOrderStatus TransferOrderStatus { get; set; }

        public virtual ICollection<TransferOrderDetail> TransferOrderDetails { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
    }
}