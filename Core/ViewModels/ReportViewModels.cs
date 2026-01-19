using System;
using System.Collections.Generic;

namespace Core.ViewModels.ReportViewModels
{
    // Stock Reports
    public class StockReportViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal QuantityBase { get; set; }
        public string BaseUnitCode { get; set; }
        public decimal CostValue { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime AsOfDate { get; set; }
    }

    public class StockMovementReportViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public int MovementId { get; set; }
        public string MovementCode { get; set; }
        public string MovementType { get; set; }
        public decimal Quantity { get; set; }
        public DateTime MovementDate { get; set; }
        public string Status { get; set; }
    }

    public class StockValuationReportViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime AsOfDate { get; set; }
    }

    public class LowStockReportViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int BranchId { get; set; }
        public decimal CurrentQuantity { get; set; }
        public decimal MinimumThreshold { get; set; }
        public bool IsLowStock { get; set; }
    }

    public class StockAgeingReportViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public int DaysInStock { get; set; }
        public decimal Quantity { get; set; }
        public string AgeCategory { get; set; } // Fresh, Old, Very Old
        public DateTime LastMovementDate { get; set; }
    }

    public class ABCAnalysisReportViewModel
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public decimal AnnualValue { get; set; }
        public decimal PercentageOfTotal { get; set; }
        public string Classification { get; set; } // A, B, C
    }

    // Movement Reports
    public class MovementSummaryReportViewModel
    {
        public int MovementId { get; set; }
        public string MovementCode { get; set; }
        public string MovementType { get; set; }
        public DateTime MovementDate { get; set; }
        public int LineCount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public string Status { get; set; }
    }

    public class MovementDetailsReportViewModel
    {
        public int MovementId { get; set; }
        public string MovementCode { get; set; }
        public string MovementType { get; set; }
        public DateTime MovementDate { get; set; }
        public List<MovementLineDetailViewModel> Lines { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class MovementLineDetailViewModel
    {
        public int LineNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public string UnitCode { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineValue { get; set; }
    }

    // Order Reports
    public class OrderStatusReportViewModel
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public string OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class PendingOrderReportViewModel
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public string OrderType { get; set; }
        public int DaysPending { get; set; }
        public decimal TotalQuantity { get; set; }
        public int PriorityScore { get; set; }
    }

    public class OrderFulfillmentReportViewModel
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal FulfillmentPercentage { get; set; }
        public DateTime? CompletionDate { get; set; }
    }

    public class SLAComplianceReportViewModel
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; }
        public DateTime SLADueDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
        public bool IsCompliant { get; set; }
        public int DaysVariance { get; set; }
    }

    // Approval Reports
    public class ApprovalMetricsViewModel
    {
        public int TotalDocumentsSubmitted { get; set; }
        public int TotalApproved { get; set; }
        public int TotalRejected { get; set; }
        public int TotalPending { get; set; }
        public double AverageApprovalTime { get; set; }
        public double ApprovalRate { get; set; }
    }

    public class ApprovalBottlenecksViewModel
    {
        public List<BottleneckItemViewModel> Bottlenecks { get; set; }
    }

    public class BottleneckItemViewModel
    {
        public string ApprovalRole { get; set; }
        public int PendingDocuments { get; set; }
        public double AverageWaitingTime { get; set; }
        public string CriticalItems { get; set; }
    }

    public class UserApprovalActivityViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int DocumentsApproved { get; set; }
        public int DocumentsRejected { get; set; }
        public double AverageTimePerApproval { get; set; }
        public DateTime LastApprovalDate { get; set; }
    }

    // Filter
    public class ReportFilterViewModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? BranchId { get; set; }
        public int? ItemId { get; set; }
        public int? SupplierId { get; set; }
        public string Status { get; set; }
    }
}
