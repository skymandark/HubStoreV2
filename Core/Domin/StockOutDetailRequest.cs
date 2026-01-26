using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class StockOutDetailRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        public int Serial { get; set; }

        [Required]
        public int ItemId { get; set; }

        [MaxLength(200)]
        public string ItemName { get; set; }

        [MaxLength(200)]
        public string ItemNameEng { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal BonusQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal ConsumerPrice { get; set; }

        [MaxLength(50)]
        public string BatchNumber { get; set; }

        public DateTime? ExpireDate { get; set; }

        public int? PackageId { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal MainDiscountPercent { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal MainDiscountValue { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal AddedDiscountPercent { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal AddedDiscountValue { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal VatValue { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal NetValue { get; set; }

        [ForeignKey(nameof(RequestId))]
        public virtual StockOutHeaderRequest StockOutHeaderRequest { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(PackageId))]
        public virtual Package Package { get; set; }
    }
}