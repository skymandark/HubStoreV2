using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.AuditServices;
using Core.ViewModels.AuditViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public  class AttemptLogService:IAttemptLogService
    {

        private readonly ApplicationDbContext _context;

        public AttemptLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogFailedAttempt(CreateAttemptLogViewModel attemptDto)
        {
            var attemptLog = new AttemptLog
            {
                UserId = attemptDto.UserId,
                DocumentId = attemptDto.DocumentId,
                DocumentType = attemptDto.DocumentType,
                ActionAttempted = attemptDto.ActionAttempted,
                ValidationMessage = attemptDto.ValidationMessage,
                Reason = attemptDto.Reason,
                ClientIp = attemptDto.ClientIp,
                ClientDevice = attemptDto.ClientDevice,
                Timestamp = DateTime.UtcNow
            };

            await _context.AttemptLogs.AddAsync(attemptLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AttemptLogViewModel>> GetAttemptLogs(AttemptLogFilterViewModel filters)
        {
            var query = _context.AttemptLogs
                .Include(a => a.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filters.UserId))
                query = query.Where(a => a.UserId == filters.UserId);

            if (!string.IsNullOrEmpty(filters.ActionAttempted))
                query = query.Where(a => a.ActionAttempted == filters.ActionAttempted);

            if (filters.DateFrom.HasValue)
                query = query.Where(a => a.Timestamp >= filters.DateFrom.Value);

            if (filters.DateTo.HasValue)
                query = query.Where(a => a.Timestamp <= filters.DateTo.Value);

            query = query.OrderByDescending(a => a.Timestamp);

            return await query.Select(a => new AttemptLogViewModel
            {
                AttemptLogId = a.AttemptLogId,
                UserId = a.UserId,
                UserName = a.User.UserName,
                DocumentId = a.DocumentId,
                DocumentType = a.DocumentType,
                ActionAttempted = a.ActionAttempted,
                ValidationMessage = a.ValidationMessage,
                Reason = a.Reason,
                ClientIp = a.ClientIp,
                ClientDevice = a.ClientDevice,
                Timestamp = a.Timestamp
            }).ToListAsync();
        }

        public async Task<FailureAnalysisViewModel> AnalyzeFailurePatterns(DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.AttemptLogs.AsQueryable();

            if (dateFrom.HasValue)
                query = query.Where(a => a.Timestamp >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(a => a.Timestamp <= dateTo.Value);

            var failures = await query.ToListAsync();
            var totalFailures = failures.Count;

            if (totalFailures == 0)
            {
                return new FailureAnalysisViewModel
                {
                    TotalFailures = 0,
                    TopPatterns = new List<FailurePatternViewModel>(),
                    FailuresByType = new Dictionary<string, int>(),
                    FailuresByUser = new Dictionary<string, int>()
                };
            }

            // تحليل أنماط الفشل الشائعة من ValidationMessage
            var patternGroups = failures
                .Where(f => !string.IsNullOrEmpty(f.ValidationMessage))
                .GroupBy(f => f.ValidationMessage)
                .Select(g => new FailurePatternViewModel
                {
                    Pattern = g.Key,
                    Occurrences = g.Count(),
                    Percentage = Math.Round((double)g.Count() / totalFailures * 100, 2)
                })
                .OrderByDescending(p => p.Occurrences)
                .Take(10)
                .ToList();

            // الفشل حسب نوع الإجراء
            var failuresByType = failures
                .GroupBy(f => f.ActionAttempted)
                .ToDictionary(g => g.Key, g => g.Count());

            // الفشل حسب المستخدم
            var failuresByUser = failures
                .GroupBy(f => f.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            return new FailureAnalysisViewModel
            {
                TotalFailures = totalFailures,
                TopPatterns = patternGroups,
                FailuresByType = failuresByType,
                FailuresByUser = failuresByUser
            };
        }
    }
}

