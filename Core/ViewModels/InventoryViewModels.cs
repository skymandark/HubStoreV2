using System;
using System.Collections.Generic;

namespace Core.ViewModels.InventoryViewModels
{
    public class BalanceViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal QuantityBase { get; set; }
        public decimal CostValue { get; set; }
        public decimal UnitPrice { get; set; }
        public string BaseUnitCode { get; set; }
        public DateTime AsOfDate { get; set; }
    }

    public class OpeningBalanceViewModel
    {
        public int OpeningBalanceId { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int FiscalYear { get; set; }
        public decimal OpeningQuantityBase { get; set; }
        public decimal CostValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public class MovementImpactViewModel
    {
        public int MovementId { get; set; }
        public string MovementCode { get; set; }
        public int ItemId { get; set; }
        public decimal QtyBase { get; set; }
        public decimal ImpactOnBalance { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
