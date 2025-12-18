using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.ReportViewModels;

namespace Core.Services.ReportingServices
{
    public interface IInventoryReportService
    {
        Task<List<StockReportViewModel>> GetStockReport(ReportFilterViewModel filters, DateTime? asOfDate);
        Task<List<StockMovementReportViewModel>> GetStockMovementReport(ReportFilterViewModel filters, DateTime? dateFrom, DateTime? dateTo);
        Task<List<StockValuationReportViewModel>> GetStockValuationReport(ReportFilterViewModel filters, DateTime? asOfDate);
        Task<List<LowStockReportViewModel>> GetLowStockReport(ReportFilterViewModel filters, decimal threshold);
        Task<List<StockAgeingReportViewModel>> GetStockAgeingReport(ReportFilterViewModel filters);
        Task<List<ABCAnalysisReportViewModel>> GetABCAnalysisReport(ReportFilterViewModel filters);
    }
}
