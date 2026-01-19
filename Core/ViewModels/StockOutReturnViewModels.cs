using System;
using System.Collections.Generic;

namespace Core.ViewModels.StockOutReturnViewModels
{
    public class StockOutReturnRequestDto
    {
        public int StockOutReturnId { get; set; }
        public DateTime DocDate { get; set; }
        public int BranchId { get; set; }
        public int? SupplierId { get; set; }
        public int ReturnOrderId { get; set; }
        public int? ReturnReasonId { get; set; }
        public string Remarks { get; set; }
        public List<StockOutReturnDetailDto> StockOutReturnDetails { get; set; }
    }

    public class StockOutReturnDetailDto
    {
        public int StockOutReturnDetailId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
        public int? ReturnReasonId { get; set; }
        public int ReturnOrderDetailId { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Notes { get; set; }
        // For UI: RemainingQty, etc.
        public decimal OrderQty { get; set; }
        public decimal PreviouslyExecutedQty { get; set; }
        public decimal RemainingQty { get; set; }
    }

    public class StockOutReturnListDto
    {
        public int StockOutReturnId { get; set; }
        public string DocCode { get; set; }
        public string SupplierName { get; set; }
        public string BranchName { get; set; }
        public DateTime DocDate { get; set; }
        public string Status { get; set; }
        public decimal TotalValue { get; set; }
        public string Remarks { get; set; }
    }

    public class ReturnOrderForStockOutDto
    {
        public int ReturnOrderId { get; set; }
        public string DocCode { get; set; }
        public string SupplierName { get; set; }
        public DateTime DocDate { get; set; }
        public List<ReturnOrderDetailForStockOutDto> Details { get; set; }
    }

    public class ReturnOrderDetailForStockOutDto
    {
        public int ReturnOrderDetailId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public decimal ExecutedQty { get; set; }
        public decimal RemainingQty { get; set; }
        public decimal Price { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}