using System;
using System.Collections.Generic;

namespace Core.ViewModels.AuditViewModels
{
    public class AuditTrailViewModel
    {
        public long LogId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ActionType { get; set; }
        public string DocumentType { get; set; }
        public int DocumentId { get; set; }
        public DateTime ActionDate { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string Notes { get; set; }
        public string ClientIp { get; set; }
        public string ClientDevice { get; set; }
    }

    public class UserActivityViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int TotalActions { get; set; }
        public List<ActionSummaryViewModel> Actions { get; set; }
    }

    public class ActionSummaryViewModel
    {
        public string ActionType { get; set; }
        public int Count { get; set; }
    }

    public class AttemptLogViewModel
    {
        public long AttemptLogId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int? DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string ActionAttempted { get; set; }
        public string ValidationMessage { get; set; }
        public string Reason { get; set; }
        public string ClientIp { get; set; }
        public string ClientDevice { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class FailureAnalysisViewModel
    {
        public int TotalFailures { get; set; }
        public List<FailurePatternViewModel> TopPatterns { get; set; }
        public Dictionary<string, int> FailuresByType { get; set; }
        public Dictionary<string, int> FailuresByUser { get; set; }
    }

    public class FailurePatternViewModel
    {
        public string Pattern { get; set; }
        public int Occurrences { get; set; }
        public double Percentage { get; set; }
    }

    public class ExportLogViewModel
    {
        public long ExportLogId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ExportType { get; set; }
        public string FiltersJson { get; set; }
        public int RowCount { get; set; }
        public string FileReference { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    // Create DTOs
    public class CreateAttemptLogViewModel
    {
        public string UserId { get; set; }
        public int? DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string ActionAttempted { get; set; }
        public string ValidationMessage { get; set; }
        public string Reason { get; set; }
        public string ClientIp { get; set; }
        public string ClientDevice { get; set; }
    }

    // Filters
    public class AuditSearchFilterViewModel
    {
        public string UserId { get; set; }
        public string ActionType { get; set; }
        public string DocumentType { get; set; }
        public int? DocumentId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string SearchText { get; set; }
    }

    public class AttemptLogFilterViewModel
    {
        public string UserId { get; set; }
        public string ActionAttempted { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class ExportLogFilterViewModel
    {
        public string UserId { get; set; }
        public string ExportType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? Expired { get; set; }
    }
}
