using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class StockOutHeaderRequest
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int BranchStockId { get; set; }

        public int? ClientId { get; set; }

        [MaxLength(200)]
        public string ClientName { get; set; }

        [Required]
        public int TransactionTypeId { get; set; } // 6 for Sell, 11 for Transfer

        public int? SellOrderId { get; set; }

        public int? TransferOrderId { get; set; }

        public int? ToBranch { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal TotalAddedDiscount { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal TotalDiscount { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal VatValue { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal NetValue { get; set; }

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

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual AppUser Client { get; set; }

        [ForeignKey(nameof(SellOrderId))]
        public virtual Order SellOrder { get; set; }

        [ForeignKey(nameof(TransferOrderId))]
        public virtual TransferOrderHeader TransferOrder { get; set; }

        [ForeignKey(nameof(ToBranch))]
        public virtual Branch ToBranchNavigation { get; set; }

        public virtual ICollection<StockOutDetailRequest> StockOutDetailRequests { get; set; }
    }
}