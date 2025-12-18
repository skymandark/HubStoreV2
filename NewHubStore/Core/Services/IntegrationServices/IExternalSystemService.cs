using System;
using System.Threading.Tasks;
using Core.ViewModels;

namespace Core.Services.IntegrationServices
{
    public interface IExternalSystemService
    {
        Task<bool> SyncWithAccountingAsync(int movementId);
        Task<bool> SyncWithERPAsync(int documentId, string documentType);
        Task<BatchResultViewModel> ImportFromExcelAsync(byte[] fileContent);
        Task<bool> ExportToExternalSystemAsync(object data, string systemType);
    }
}
