using System;
using System.Collections.Generic;
using Core.Domin;

namespace Core.ViewModels.ReturnOrderViewModels
{
    public class ReturnOrderRequestDto
    {
        public int ReturnOrderId { get; set; }
        public DateTime DocDate { get; set; }
        public int BranchId { get; set; }
        public ReturnType ReturnType { get; set; }
        public int? SupplierId { get; set; }
        public int? ClientId { get; set; }
        public int? ReturnReasonId { get; set; }
        public string Remarks { get; set; }
        public List<ReturnOrderDetailDto> ReturnOrderDetails { get; set; }
    }

    public class ReturnOrderDetailDto
    {
        public int ReturnOrderDetailId { get; set; }
        public int ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
        public int? ReturnReasonId { get; set; }
        public int OriginalMovementDetailId { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Notes { get; set; }
        public decimal AvailableStock { get; set; }
    }

    public class ReturnOrderListDto
    {
        public int ReturnOrderId { get; set; }
        public string DocCode { get; set; }
        public string ReturnType { get; set; }
        public string SupplierName { get; set; }
        public string ClientName { get; set; }
        public string BranchName { get; set; }
        public DateTime DocDate { get; set; }
        public string Status { get; set; }
        public decimal RequestedQty { get; set; }
        public decimal ApprovedQty { get; set; }
        public decimal ReceivedQty { get; set; }
        public string Remarks { get; set; }
    }
}