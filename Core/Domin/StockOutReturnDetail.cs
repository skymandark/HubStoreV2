using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class StockOutReturnDetail
    {
        [Key]
        public int StockOutReturnDetailId { get; set; }

        [Required]
        public int StockOutReturnId { get; set; }

        public int LineSerialNumber { get; set; } // Auto-increment

        [Required]
        public int ItemId { get; set; }

        public int? ItemPackageId { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal OrderQuantity { get; set; } // From Return Order

        [Column(TypeName = "decimal(18,6)")]
        public decimal Qty { get; set; } // Stock Out Quantity

        [Column(TypeName = "decimal(18,6)")]
        public decimal BonusQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsumerPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        [MaxLength(50)]
        public string BatchNo { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; } // Qty * Price

        [Column(TypeName = "decimal(18,6)")]
        public decimal DiscountPercent { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal AddedDiscountPercent { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AddedDiscountValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VatValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        public int? ReturnReasonId { get; set; }

        public int? ReturnOrderDetailId { get; set; }

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

        [ForeignKey(nameof(StockOutReturnId))]
        public virtual StockOutReturnHeader StockOutReturnHeader { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(ItemPackageId))]
        public virtual ItemUnit ItemPackage { get; set; }

        [ForeignKey(nameof(ReturnOrderDetailId))]
        public virtual ReturnOrderDetail ReturnOrderDetail { get; set; }
    }
}