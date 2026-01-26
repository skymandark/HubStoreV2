using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class DirectPurchaseOrderDetail
    {
        [Key]
        public int DirectPurchaseOrderDetailId { get; set; }

        [Required]
        public int DirectPurchaseOrderId { get; set; }

        [ForeignKey("DirectPurchaseOrderId")]
        public DirectPurchaseOrderHeader DirectPurchaseOrderHeader { get; set; }

        [Required]
        public int Serial { get; set; }

        [Required]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public int? ItemPackageId { get; set; }

        [ForeignKey("ItemPackageId")]
        public ItemPackage ItemPackage { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BonusQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MainDiscountPercent { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MainDiscountValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AddedDiscountPercent { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AddedDiscountValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal VatValue { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetValue { get; set; }

        [StringLength(500)]
        public string RemarksArab { get; set; }

        [StringLength(500)]
        public string RemarksEng { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}