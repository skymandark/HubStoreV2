using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.AuditViewModels;

namespace Core.Services.AuditServices
{
    public interface IAttemptLogService
    {
        Task LogFailedAttempt(CreateAttemptLogViewModel attemptDto);
        Task<List<AttemptLogViewModel>> GetAttemptLogs(AttemptLogFilterViewModel filters);
        Task<FailureAnalysisViewModel> AnalyzeFailurePatterns(DateTime? dateFrom, DateTime? dateTo);
    }
}
