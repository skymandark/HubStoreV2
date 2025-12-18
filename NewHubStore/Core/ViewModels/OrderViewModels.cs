using System;
using System.Collections.Generic;

namespace Core.ViewModels.OrderViewModels
{
    // Order View Models
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public int OrderTypeId { get; set; }
        public string OrderTypeName { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int? BranchFromId { get; set; }
        public string BranchFromName { get; set; }
        public int? BranchToId { get; set; }
        public string BranchToName { get; set; }
        public string RequestedByUserId { get; set; }
        public string RequestedByUserName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
        public decimal TotalQuantityBase { get; set; }
        public decimal TotalValueCost { get; set; }
        public int PriorityFlag { get; set; }
        public DateTime? SLA_DueDate { get; set; }
        public string InternalBarcode { get; set; }
        public string ExternalBarcode { get; set; }
        public string Notes { get; set; }
        public List<OrderLineViewModel> Lines { get; set; }
    }

    public class OrderLineViewModel
    {
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public int LineNo { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string UnitCode { get; set; }
        public decimal QtyOrdered { get; set; }
        public decimal QtyReceived { get; set; }
        public decimal ConversionUsedToBase { get; set; }
        public decimal QtyBaseOrdered { get; set; }
        public decimal QtyBaseReceived { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CostValue { get; set; }
        public decimal TaxRate { get; set; }
        public string LineStatus { get; set; }
        public string Notes { get; set; }
    }

    public class POStatusViewModel
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public decimal TotalOrdered { get; set; }
        public decimal TotalReceived { get; set; }
        public int RemainingQuantity { get; set; }
        public string Status { get; set; }
    }

    // Create/Update DTOs
    public class CreateOrderViewModel
    {
        public string OrderCode { get; set; }
        public int OrderTypeId { get; set; }
        public int? SupplierId { get; set; }
        public int? BranchFromId { get; set; }
        public int? BranchToId { get; set; }
        public string RequestedByUserId { get; set; }
        public DateTime RequestedDate { get; set; }
        public int PriorityFlag { get; set; }
        public DateTime? SLA_DueDate { get; set; }
        public string InternalBarcode { get; set; }
        public string ExternalBarcode { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public List<CreateOrderLineViewModel> Lines { get; set; }
    }

    public class UpdateOrderViewModel
    {
        public int? SupplierId { get; set; }
        public int? BranchFromId { get; set; }
        public int? BranchToId { get; set; }
        public int PriorityFlag { get; set; }
        public DateTime? SLA_DueDate { get; set; }
        public string InternalBarcode { get; set; }
        public string ExternalBarcode { get; set; }
        public string Notes { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class CreateOrderLineViewModel
    {
        public int LineNo { get; set; }
        public int ItemId { get; set; }
        public string UnitCode { get; set; }
        public decimal QtyOrdered { get; set; }
        public decimal ConversionUsedToBase { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
    }

    public class UpdateOrderLineViewModel
    {
        public decimal QtyOrdered { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public string Notes { get; set; }
    }

    public class CreatePurchaseOrderViewModel : CreateOrderViewModel
    {
    }

    public class CreateTransferOrderViewModel : CreateOrderViewModel
    {
    }

    public class ReceiveOrderViewModel
    {
        public List<ReceiveOrderLineViewModel> Lines { get; set; }
        public string ReceivingNotes { get; set; }
        public string ReceivedBy { get; set; }
    }

    public class ReceiveOrderLineViewModel
    {
        public int OrderLineId { get; set; }
        public decimal QtyReceived { get; set; }
        public string Notes { get; set; }
    }

    // Filters
    public class OrderFilterViewModel
    {
        public string OrderCode { get; set; }
        public int? OrderTypeId { get; set; }
        public int? SupplierId { get; set; }
        public string Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Barcode { get; set; }
    }
}
