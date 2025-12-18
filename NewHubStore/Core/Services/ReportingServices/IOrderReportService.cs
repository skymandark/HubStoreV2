using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.ReportViewModels;

namespace Core.Services.ReportingServices
{
    public interface IOrderReportService
    {

        Task<List<OrderStatusReportViewModel>> GetOrderStatusReport(ReportFilterViewModel filters);
        Task<List<PendingOrderReportViewModel>> GetPendingOrdersReport(ReportFilterViewModel filters);
        Task<List<OrderFulfillmentReportViewModel>> GetOrderFulfillmentReport(ReportFilterViewModel filters);
        Task<List<SLAComplianceReportViewModel>> GetSLAComplianceReport(ReportFilterViewModel filters);
    }
}
