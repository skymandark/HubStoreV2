using System;
using System.Collections.Generic;

namespace Core.ViewModels.ApprovalViewModels
{
    public class ApprovalDashboardVm
    {
        public List<ApprovalTypeVm> ApprovalTypes { get; set; } = new List<ApprovalTypeVm>();
    }

    public class ApprovalTypeVm
    {
        public int ApproveDefId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int PendingCount { get; set; }
        public string ControllerName { get; set; }
    }

    public class ApprovalItemDto
    {
        public int RequestId { get; set; }
        public string Reference { get; set; }
        public string PartyName { get; set; } // Supplier or Client
        public string BranchName { get; set; }
        public DateTime DocDate { get; set; }
        public decimal TotalValue { get; set; }
        public string Status { get; set; }
        public int ApproveDefId { get; set; }
    }

    public class BatchApprovalDto
    {
        public List<int> RequestIds { get; set; }
        public int ApproveDefId { get; set; }
        public int TargetStatusId { get; set; }
        public string Note { get; set; }
    }

    public class PendingApprovalViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentType { get; set; }
        public DateTime RequestedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string CurrentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public int PriorityScore { get; set; }
    }

    public class ApprovalFilterViewModel
    {
        public string DocumentType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Status { get; set; }
        public string SearchTerm { get; set; }
        public int? PriorityFrom { get; set; }
        public int? PriorityTo { get; set; }
    }

    public class ApprovalHistoryViewModel
    {
        public int ApprovalHistoryId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string ApprovalStatus { get; set; } // Renamed from StatusName
        public string ActionByUserId { get; set; }
        public string ActionByUserName { get; set; } // Renamed from ActionByUser
        public string ActionRoleId { get; set; }
        public DateTime ActionTimestamp { get; set; }
        public string Note { get; set; }
        public bool IsMandatoryNoteProvided { get; set; }
    }

    public class ApprovalChainViewModel
    {
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; } // Renamed from DocumentTypeName
        public List<ApprovalStepViewModel> ApprovalSteps { get; set; } = new List<ApprovalStepViewModel>(); // Renamed from Steps
    }

    public class ApprovalStepViewModel
    {
        public int StepNumber { get; set; }
        public string RoleName { get; set; }
        public string ActionType { get; set; }
        public bool IsMandatory { get; set; }
        public bool AllowPartialApproval { get; set; }
    }
}
