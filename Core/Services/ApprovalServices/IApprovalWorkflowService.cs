using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels.ApprovalViewModels;

namespace Core.Services.ApprovalServices
{
    public interface IApprovalWorkflowService
    {
        Task<ApprovalChainViewModel> GetApprovalChainAsync(string documentType);
        Task<bool> CheckApprovalPermissionsAsync(string userId, string documentType,
            string action, int? documentId = null);
        Task<ServiceResult> EscalateApprovalAsync(int documentId, string documentType,
            string reason, string userId);
        int CalculatePriorityScore(object documentDto);
        Task<bool> IsUserInApprovalChainAsync(string userId, string documentType);
        Task<int> GetCurrentApprovalStepAsync(int documentId, string documentType);
    }
}
