using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.AuditViewModels;

namespace Core.Services.AuditServices
{
    public interface IExportLogService
    {
        Task LogExport(string userId, string exportType, string filtersJson, int rowCount, string fileReference, DateTime? expiryDate = null);
        Task<List<ExportLogViewModel>> GetExportLogs(ExportLogFilterViewModel filters);
        Task<int> CleanupExpiredExports();
        Task<bool> ValidateExportPermission(string userId, string exportType);
    }
}
