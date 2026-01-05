using System;
using System.Collections.Generic;

namespace Core.ViewModels.TransferOrderViewModels
{
    public class TransferOrderRequestDto
    {
        public int RequestId { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime EntryDate { get; set; }
        public int FromBranch { get; set; }
        public int ToBranch { get; set; }
        public int ShipmentTypeId { get; set; }
        public RequestTransferOrderHeaderDto RequestTransferOrderHeader { get; set; }
        public List<ShipmentTypeDto> ShipmentTypes { get; set; }
        public List<BranchDto> ToBranches { get; set; }
        public List<BranchDto> FromBranches { get; set; }
        public List<TransferOrderDetailDto> TransferOrderDetails { get; set; }
    }

    public class RequestTransferOrderHeaderDto
    {
        public DateTime DocDate { get; set; }
        public string Reference { get; set; }
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        public string Notes { get; set; }
    }

    public class ItemTransferOrderVm
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string StockAccount { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class TransferOrderDetailDto
    {
        public int TransferOrderDetailId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public string Notes { get; set; }
    }

    public class ShipmentTypeDto
    {
        public int ShipmentTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class BranchDto
    {
        public int BranchId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class TransferOrderStatusDto
    {
        public int TransferOrderStatusId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    // For listing
    public class TransferOrderListDto
    {
        public int TransferOrderId { get; set; }
        public string TransferOrderCode { get; set; }
        public string Reference { get; set; }
        public string FromBranchName { get; set; }
        public string ToBranchName { get; set; }
        public string ShipmentTypeName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DocDate { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}