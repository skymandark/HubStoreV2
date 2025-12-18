using System;
using System.Threading.Tasks;
using Core.ViewModels.ReportViewModels;

namespace Core.Services.ReportingServices
{
    public interface IApprovalReportService
    {
        Task<ApprovalMetricsViewModel> GetApprovalMetrics(DateTime? dateFrom, DateTime? dateTo);
        Task<ApprovalBottlenecksViewModel> GetApprovalBottlenecks(ReportFilterViewModel filters);
        Task<UserApprovalActivityViewModel> GetUserApprovalActivity(string userId, DateTime? dateFrom, DateTime? dateTo);
    }
}
