using System;
using System.Collections.Generic;

namespace Core.ViewModels.ApprovalViewModels
{
    public class ApprovalHistoryViewModel
    {
        public int ApprovalHistoryId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string ApprovalStatus { get; set; }
        public string ActionByUserId { get; set; }
        public string ActionByUserName { get; set; }
        public string? ActionRoleId { get; set; }
        public DateTime ActionTimestamp { get; set; }
        public string Note { get; set; }
        public bool IsMandatoryNoteProvided { get; set; }
    }

    public class PendingApprovalViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentType { get; set; }
        public DateTime RequestedDate { get; set; }
        public int PriorityScore { get; set; }
        public string CreatedByUserName { get; set; }
        public string CurrentStatus { get; set; }
    }

    public class ApprovalChainViewModel
    {
        public string DocumentType { get; set; }
        public List<ApprovalStepViewModel> ApprovalSteps { get; set; }
    }

    public class ApprovalStepViewModel
    {
        public int StepNumber { get; set; }
        public string RoleName { get; set; }
        public bool IsMandatory { get; set; }
        public bool AllowPartialApproval { get; set; }
    }

    // Create/Update DTOs
    public class CreateApprovalViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string ActionType { get; set; } // Approve, Reject, ReturnForEdit
        public string UserId { get; set; }
        public string Note { get; set; }
        public List<int> ApprovedLineIds { get; set; }
    }

    // Filters
    public class ApprovalFilterViewModel
    {
        public string DocumentType { get; set; }
        public string Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? PriorityFrom { get; set; }
        public int? PriorityTo { get; set; }
    }
}
