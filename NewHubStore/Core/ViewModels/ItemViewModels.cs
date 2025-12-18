using System;
using System.Collections.Generic;

namespace Core.ViewModels.ItemViewModels
{
    // View Models
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? ParentItemId { get; set; }
        public bool IsParent { get; set; }
        public string MainUnitCode { get; set; }
        public string BaseUnitCode { get; set; }
        public string ExternalBarcode { get; set; }
        public string InternalBarcode { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ItemUnitViewModel
    {
        public int ItemUnitId { get; set; }
        public int ItemId { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public decimal ConversionToBase { get; set; }
        public int? ParentUnitId { get; set; }
        public bool IsDefaultForDisplay { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    public class ItemHierarchyViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public List<ItemHierarchyViewModel> ChildItems { get; set; } = new List<ItemHierarchyViewModel>();
    }

    public class UnitConversionHistoryViewModel
    {
        public int ItemUnitHistoryId { get; set; }
        public int ItemId { get; set; }
        public string UnitCode { get; set; }
        public decimal OldConversionToBase { get; set; }
        public decimal NewConversionToBase { get; set; }
        public DateTime ChangedDate { get; set; }
        public string ChangedByUser { get; set; }
        public string Notes { get; set; }
    }

    // Create/Update DTOs
    public class CreateItemViewModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? ParentItemId { get; set; }
        public bool IsParent { get; set; }
        public string MainUnitCode { get; set; }
        public string BaseUnitCode { get; set; }
        public string ExternalBarcode { get; set; }
        public string InternalBarcode { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
    }

    public class UpdateItemViewModel
    {
        public string ItemName { get; set; }
        public string MainUnitCode { get; set; }
        public string BaseUnitCode { get; set; }
        public string ExternalBarcode { get; set; }
        public string InternalBarcode { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class CreateItemUnitViewModel
    {
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public decimal ConversionToBase { get; set; }
        public int? ParentUnitId { get; set; }
        public bool IsDefaultForDisplay { get; set; }
        public string CreatedBy { get; set; }
    }

    public class UpdateItemUnitViewModel
    {
        public string UnitName { get; set; }
        public decimal ConversionToBase { get; set; }
        public bool IsDefaultForDisplay { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedBy { get; set; }
    }

    // Filters
    public class ItemFilterViewModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsParent { get; set; }
        public string Barcode { get; set; }
    }
}
