using System;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.InventoryViewModels
{
    public class ItemTransactionViewModel
    {
        public int BaseId { get; set; }
        public string TransactionType { get; set; }
        public int TransactionTypeId { get; set; }
        public DateTime DocDate { get; set; }
        public string Remarks { get; set; }
        public decimal? InQuantity { get; set; }
        public decimal? InBonusQuantity { get; set; }
        public decimal? OutQuantity { get; set; }
        public decimal? OutBonusQuantity { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string BatchNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Balance { get; set; }
        public decimal OpenBalance { get; set; }
        public DateTime EntryDate { get; set; }
        public string UserData { get; set; }
        public int? SupplierClientId { get; set; }
        public string SupplierClientName { get; set; }
        public int? ItemPackageId { get; set; }
        public string ItemPackageName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }

    public class ItemBalanceSummaryViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal TotalIn { get; set; }
        public decimal TotalOut { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime AsOfDate { get; set; }
    }

    public class ItemSheetSearchViewModel
    {
        [Required(ErrorMessage = "يجب اختيار الصنف")]
        public int ItemId { get; set; }
        
        [Required(ErrorMessage = "يجب اختيار المخزن")]
        public int BranchId { get; set; }
        
        [Required(ErrorMessage = "يجب تحديد تاريخ البداية")]
        public DateTime FromDate { get; set; }
        
        [Required(ErrorMessage = "يجب تحديد تاريخ النهاية")]
        public DateTime ToDate { get; set; }
        
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int? TransactionTypeId { get; set; }
    }
}
