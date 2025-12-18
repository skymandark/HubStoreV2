using System;
using System.Collections.Generic;

namespace Core.ViewModels.MovementViewModels
{
    // Movement View Models
    public class MovementViewModel
    {
        public int MovementId { get; set; }
        public string MovementCode { get; set; }
        public int MovementTypeId { get; set; }
        public string MovementTypeName { get; set; }
        public int? BranchFromId { get; set; }
        public string BranchFromName { get; set; }
        public int? BranchToId { get; set; }
        public string BranchToName { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal TotalQuantityBase { get; set; }
        public decimal TotalValueCost { get; set; }
        public string InternalBarcode { get; set; }
        public string ExternalBarcode { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public List<MovementLineViewModel> Lines { get; set; }
    }

    public class MovementLineViewModel
    {
        public int MovementLineId { get; set; }
        public int MovementId { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public int BranchId { get; set; }
        public string UnitCode { get; set; }
        public decimal QtyInput { get; set; }
        public decimal ConversionUsedToBase { get; set; }
        public decimal QtyBase { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Notes { get; set; }
    }

    public class MovementLineTotalsViewModel
    {
        public int MovementLineId { get; set; }
        public decimal QtyBase { get; set; }
        public decimal TotalValue { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // Create/Update DTOs
    public class CreateMovementViewModel
    {
        public string MovementCode { get; set; }
        public int MovementTypeId { get; set; }
        public int? BranchFromId { get; set; }
        public int? BranchToId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime Date { get; set; }
        public string InternalBarcode { get; set; }
        public string ExternalBarcode { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public List<CreateMovementLineViewModel> Lines { get; set; }
    }

    public class UpdateMovementViewModel
    {
        public DateTime Date { get; set; }
        public int? BranchFromId { get; set; }
        public int? BranchToId { get; set; }
        public int? SupplierId { get; set; }
        public string InternalBarcode { get; set; }
        public string ExternalBarcode { get; set; }
        public string Notes { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class CreateMovementLineViewModel
    {
        public int ItemId { get; set; }
        public int BranchId { get; set; }
        public string UnitCode { get; set; }
        public decimal QtyInput { get; set; }
        public decimal ConversionUsedToBase { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
    }

    public class UpdateMovementLineViewModel
    {
        public decimal QtyInput { get; set; }
        public decimal ConversionUsedToBase { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; }
    }

    public class CreateTransferViewModel
    {
        public int ItemId { get; set; }
        public int BranchFromId { get; set; }
        public int BranchToId { get; set; }
        public decimal Quantity { get; set; }
        public string UnitCode { get; set; }
        public string CreatedBy { get; set; }
    }

    // Filters
    public class MovementFilterViewModel
    {
        public string MovementCode { get; set; }
        public int? MovementTypeId { get; set; }
        public int? BranchId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Status { get; set; }
        public string Barcode { get; set; }
    }
}
