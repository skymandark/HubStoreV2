using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.AuditViewModels;

namespace Core.Services.AuditServices
{
    public interface IAuditTrailService
    {
        Task LogAction(string userId, string actionType, string documentType, int documentId, string changes = null,
                      string oldValues = null, string newValues = null, string notes = null,
                      string clientIp = null, string clientDevice = null);

        Task<List<AuditTrailViewModel>> GetAuditTrail(int documentId, string documentType);
        Task<UserActivityViewModel> GetUserActivity(string userId, DateTime? dateFrom, DateTime? dateTo);
        Task<List<AuditTrailViewModel>> SearchAuditLogs(AuditSearchFilterViewModel filters);
    }
}
