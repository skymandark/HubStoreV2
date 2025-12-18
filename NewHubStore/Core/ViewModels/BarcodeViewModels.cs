using System;
using System.Collections.Generic;

namespace Core.ViewModels.BarcodeViewModels
{
    public class BarcodeViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        public string BarcodeType { get; set; } // Internal, External
        public DateTime GeneratedDate { get; set; }
    }

    public class BarcodeDetailsViewModel
    {
        public string Barcode { get; set; }
        public string BarcodeType { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal? CurrentQuantity { get; set; }
        public string BaseUnitCode { get; set; }
    }
}
