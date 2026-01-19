using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Domin
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        // 1. المعلومات الأساسية (Basic Information)
        [Required]
        [MaxLength(100)]
        public string NameArab { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameEng { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string InternalBarcode { get; set; }

        [MaxLength(500)]
        public string ExternalBarcode { get; set; }

        [MaxLength(100)]
        public string ItemCode { get; set; }

        [MaxLength(100)]
        public string ItemName { get; set; }

        [MaxLength(50)]
        public string MainUnitCode { get; set; }

        [MaxLength(50)]
        public string BaseUnitCode { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AverageCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }

        // 2. الفئات والتصنيفات (Categories and Classifications)
        [Required]
        public int SectionId { get; set; }

        [Required]
        public int MainItemId { get; set; }

        [Required]
        public int SubItemId { get; set; }

        public int? VendorId { get; set; }

        public int? BrandId { get; set; }

        public int? ItemTypeId { get; set; }

        public int? ItemCategoryId { get; set; }

        public int? ParentItemId { get; set; }

        public bool IsParent { get; set; } = false;

        // 3. التغليف والوحدات (Packaging and Units)
        [Required]
        public int SellMainPackageId { get; set; }

        public int? SellSubPackageId { get; set; }

        public int? SellSubPackageCount { get; set; }

        [Required]
        public int BuyMainPackageId { get; set; }

        public int? BuySubPackageId { get; set; }

        public int? BuySubPackageCount { get; set; }

        public int? PackageCount { get; set; }

        // 4. الأسعار والتكاليف (Pricing and Costs)
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CustomerPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BasicPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CostWithoutVat { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ProfitAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? MainDiscount { get; set; }

        public bool NoVat { get; set; } = false;

        // 5. أنواع التداول (Trade Types)
        public int? BuyTradeTypeId { get; set; }

        public int? SellTradeTypeId { get; set; }

        // 6. الحسابات المالية (Financial Accounts)
        [MaxLength(50)]
        public string StockAccount { get; set; }

        [MaxLength(50)]
        public string TradeAccount { get; set; }

        [MaxLength(50)]
        public string VatAccount { get; set; }

        // 7. الحدود والكميات (Limits and Quantities)
        [Column(TypeName = "decimal(18,6)")]
        public decimal? AskQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? SafeQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? MaxQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? MinQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? AverageQuantityPeriodDays { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? CoverageDays { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? OrderMinQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? OrderMaxQuantity { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal? QuotaQuantity { get; set; }

        // 8. الخصائص المنطقية (Boolean Properties)
        [Required]
        public bool Locked { get; set; } = false;

        public bool NoReplenishment { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public bool IsNoStock { get; set; } = false;

        public bool IsDiscontinued { get; set; } = false;

        public bool IsDeficient { get; set; } = false;

        public bool RequiresRefrigeration { get; set; } = false;

        public bool IsPos { get; set; } = false;

        public bool IsOnline { get; set; } = false;

        public bool IsSerialized { get; set; } = false;

        public bool IsGift { get; set; } = false;

        public bool IsPromoted { get; set; } = false;

        public bool HasExpiryDate { get; set; } = false;

        public bool HasBatchNumber { get; set; } = false;

        public bool IsService { get; set; } = false;

        public bool IsRawMaterial { get; set; } = false;

        public bool IsFinishedGood { get; set; } = false;

        // 9. التواريخ والتحديثات (Dates and Updates)
        public DateTime? OfferStartDate { get; set; }

        public DateTime? OfferEndDate { get; set; }

        public DateTime? OfferEndedDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        // حقول النظام
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation Properties - العلاقات
        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }

        [ForeignKey(nameof(MainItemId))]
        public virtual MainItem MainItem { get; set; }

        [ForeignKey(nameof(SubItemId))]
        public virtual SubItem SubItem { get; set; }

        [ForeignKey(nameof(VendorId))]
        public virtual Vendor Vendor { get; set; }

        [ForeignKey(nameof(BrandId))]
        public virtual Brand Brand { get; set; }

        [ForeignKey(nameof(ItemTypeId))]
        public virtual ItemType ItemType { get; set; }

        [ForeignKey(nameof(ItemCategoryId))]
        public virtual ItemCategory ItemCategory { get; set; }

        [ForeignKey(nameof(SellMainPackageId))]
        public virtual Package SellMainPackage { get; set; }

        [ForeignKey(nameof(SellSubPackageId))]
        public virtual Package SellSubPackage { get; set; }

        [ForeignKey(nameof(BuyMainPackageId))]
        public virtual Package BuyMainPackage { get; set; }

        [ForeignKey(nameof(BuySubPackageId))]
        public virtual Package BuySubPackage { get; set; }

        [ForeignKey(nameof(BuyTradeTypeId))]
        public virtual TradeType BuyTradeType { get; set; }
[ForeignKey(nameof(SellTradeTypeId))]
public virtual TradeType SellTradeType { get; set; }


[ForeignKey(nameof(ParentItemId))]
public virtual Item ParentItem { get; set; }
public virtual ICollection<Item> ChildItems { get; set; }


// Collections
        public virtual ICollection<ItemPackage> ItemPackages { get; set; }
        public virtual ICollection<ItemFlag> ItemFlags { get; set; }
        public virtual ICollection<ItemUnit> ItemUnits { get; set; }
        public virtual ICollection<OpeningBalance> OpeningBalances { get; set; }
        public virtual ICollection<MovementLine> MovementLines { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public virtual ICollection<TransferOrderDetail> TransferOrderDetails { get; set; }
        public virtual ICollection<DirectReceiptDetail> DirectReceiptDetails { get; set; }
        public virtual ICollection<SupplierInvoiceDetail> SupplierInvoiceDetails { get; set; }
        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual ICollection<StockInDetail> StockInDetails { get; set; }
        public virtual ICollection<StockOutDetail> StockOutDetails { get; set; }
        public virtual ICollection<ReturnOrderDetail> ReturnOrderDetails { get; set; }
        public virtual ICollection<StockOutReturnDetail> StockOutReturnDetails { get; set; }
    }
}
