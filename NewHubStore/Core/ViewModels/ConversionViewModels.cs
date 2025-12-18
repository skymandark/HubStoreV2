using System;
using System.Collections.Generic;

namespace Core.ViewModels.ConversionViewModels
{
    public class ConversionResultViewModel
    {
        public int ItemId { get; set; }
        public string UnitCode { get; set; }
        public decimal QtyInput { get; set; }
        public decimal ConversionFactor { get; set; }
        public decimal QtyBase { get; set; }
    }

    public class BreakdownResultViewModel
    {
        public int ItemId { get; set; }
        public decimal BaseQuantity { get; set; }
        public Dictionary<string, decimal> BreakdownByUnit { get; set; } = new Dictionary<string, decimal>();
    }

    public class ConversionHistoryViewModel
    {
        public int ItemId { get; set; }
        public string UnitCode { get; set; }
        public decimal OldFactor { get; set; }
        public decimal NewFactor { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
