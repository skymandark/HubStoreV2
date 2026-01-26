using System;
using System.Collections.Generic;

namespace Core.ViewModels.TransferOrderViewModels
{
    public class TransferOrderRequestDto
    {
        public int TransferOrderId { get; set; }
        public string Reference { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime EntryDate { get; set; }
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        public int ShipmentTypeId { get; set; }
        public string Remarks { get; set; }
        public int Step { get; set; }
        public int Status { get; set; }
        public List<TransferOrderDetailDto> TransferOrderDetails { get; set; }
        public List<BranchDto> Branches { get; set; }
        public List<ShipmentTypeDto> ShipmentTypes { get; set; }
        public List<ItemSelectionDto> AvailableItems { get; set; }
        public List<SelectedItemDto> SelectedItems { get; set; }
    }

    public class TransferOrderDetailDto
    {
        public int TransferOrderDetailId { get; set; }
        public int ItemId { get; set; }
        public decimal RequestedQty { get; set; }
        public string Notes { get; set; }
        public int TempOrder { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal ShippedQty { get; set; }
        public decimal ReceivedQty { get; set; }
    }

    public class TransferOrderListDto
    {
        public int RequestId { get; set; }
        public string Reference { get; set; }
        public string ToBranchName { get; set; }
        public string FromBranchName { get; set; }
        public string ShipmentTypeName { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DocDate { get; set; }
        public string StepName { get; set; }
        public string StatusName { get; set; }
        public string Notes { get; set; }
    }

    public class BranchDto
    {
        public int BranchId { get; set; }
        public string Name { get; set; }
    }

    public class ShipmentTypeDto
    {
        public int ShipmentTypeId { get; set; }
        public string Name { get; set; }
    }

    public class ItemSelectionDto
    {
        public int ItemId { get; set; }
        public string ItemNameEng { get; set; }
        public string StockAccount { get; set; }
        public decimal QuantityOnWay { get; set; }
        public decimal ConsumerPrice { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal Quantity { get; set; }
    }

    public class SelectedItemDto
    {
        public int ItemId { get; set; }
        public string ItemNameEng { get; set; }
        public decimal Quantity { get; set; }
        public decimal AvailableStock { get; set; }
        public int TempOrder { get; set; }
    }
}