using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.ExportViewModels;

namespace Core.Services.ExportServices
{
    public interface IExportService
    {
        Task<ExportResultViewModel> ExportToExcelAsync(List<object> data, string format);
        Task<ExportResultViewModel> ExportToPDFAsync(List<object> data, string format);
        Task<ExportResultViewModel> ExportToCSVAsync(List<object> data);
        Task<ValidationResult> ValidateExportRequestAsync(string userId, int rowCount);
        Task<byte[]> GetExportFileAsync(string fileReference);
    }
}
