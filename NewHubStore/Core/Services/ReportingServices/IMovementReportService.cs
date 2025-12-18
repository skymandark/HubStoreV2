using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.ReportViewModels;

namespace Core.Services.ReportingServices
{
    public interface IMovementReportService
    {
        Task<List<MovementSummaryReportViewModel>> GetMovementSummary(ReportFilterViewModel filters, DateTime? dateFrom, DateTime? dateTo);
        Task<MovementDetailsReportViewModel> GetMovementDetails(int movementId);
        Task<List<MovementSummaryReportViewModel>> GetMovementByType(string type, ReportFilterViewModel filters);
        Task<List<MovementSummaryReportViewModel>> GetApprovedMovementsOnly(ReportFilterViewModel filters);
    }
}
