using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels;
using Core.ViewModels.ApprovalViewModels;

namespace Core.Services.ApprovalServices
{
    public interface IApprovalService
    {
        Task<ServiceResult> ApproveDocumentAsync(int documentId, string documentType,
            string userId, string note);

        Task<ServiceResult> RejectDocumentAsync(int documentId, string documentType,
            string userId, string note);

        Task<ServiceResult> ReturnForEditAsync(int documentId, string documentType,
            string userId, string note);

        Task<ServiceResult> PartialApproveAsync(int documentId, List<int> linesApproved,
            string userId, string note, string documentType);

        Task<List<PendingApprovalViewModel>> GetPendingApprovalsAsync(string userId,
            ApprovalFilterViewModel filters);

        Task<List<ApprovalHistoryViewModel>> GetApprovalHistoryAsync(int documentId,
            string documentType);

        bool ValidateMandatoryNote(string note, bool isMandatoryAction = true);

        Task<ServiceResult> BulkApproveAsync(List<int> documentIds, string userId,
            string note, string documentType);
    }
}
