using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace Core.Domin
{
    public class ApprovalHistory
    {
        [Key]
        public int ApprovalHistoryId { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int ApprovalStatusId { get; set; }

        [Required]
        public string ActionByUserId { get; set; }

        [MaxLength(450)] // يجب أن يتوافق مع حجم RoleId في IdentityRole
        public string ActionRoleId { get; set; } // تغيير من int? إلى string

        [Required]
        public DateTime ActionTimestamp { get; set; } = DateTime.UtcNow;

        [MaxLength(1000)]
        public string Note { get; set; }

        [Required]
        public bool IsMandatoryNoteProvided { get; set; } = false;

        [ForeignKey(nameof(DocumentTypeId))]
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey(nameof(ApprovalStatusId))]
        public virtual ApprovalStatus ApprovalStatus { get; set; }

        [ForeignKey(nameof(ActionByUserId))]
        public virtual AppUser ActionByUser { get; set; }
    }

    public class ApprovalChain
    {
        [Key]
        public int ApprovalChainId { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int StepNumber { get; set; }

        [Required]
        [MaxLength(450)]
        public string RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ActionType { get; set; } // Approve, Reject, Return

        public bool IsMandatory { get; set; } = true;

        public bool AllowPartialApproval { get; set; } = false;

        public int? MinimumApprovalsRequired { get; set; }

        public int? MaximumRejectionsAllowed { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        public virtual DocumentType DocumentType { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual IdentityRole Role { get; set; }
    }

    public class PendingApproval
    {
        [Key]
        public int PendingApprovalId { get; set; }

        public int DocumentId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentType { get; set; }
        public DateTime RequestedDate { get; set; }
        public string CreatedByUserName { get; set; }
        public string CurrentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public int PriorityScore { get; set; }
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ServiceResult Success(string message = null, object data = null)
            => new ServiceResult { IsSuccess = true, Message = message, Data = data };

        public static ServiceResult Fail(string message, List<string> errors = null)
            => new ServiceResult { IsSuccess = false, Message = message, Errors = errors ?? new List<string>() };
    }
}