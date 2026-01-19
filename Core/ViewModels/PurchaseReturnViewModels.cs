using System;
using System.Collections.Generic;

namespace Core.ViewModels.PurchaseReturnViewModels
{
    public class PurchaseReturnRequestDto
    {
        public int PurchaseReturnId { get; set; }
        public DateTime ReturnDate { get; set; }
        public int GoodsReceiptId { get; set; }
        public int BranchId { get; set; }
        public int? WarehouseId { get; set; }
        public string Remarks { get; set; }
        public List<PurchaseReturnDetailDto> PurchaseReturnDetails { get; set; }
    }

    public class PurchaseReturnDetailDto
    {
        public int PurchaseReturnDetailId { get; set; }
        public int GoodsReceiptDetailId { get; set; }
        public int ItemId { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal ReturnedBonusQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalValue { get; set; }
        public string Notes { get; set; }
    }

    public class PurchaseReturnListDto
    {
        public int PurchaseReturnId { get; set; }
        public string ReturnCode { get; set; }
        public string GoodsReceiptCode { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}