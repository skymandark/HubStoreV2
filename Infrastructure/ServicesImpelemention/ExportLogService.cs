using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels.AuditViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public  class ExportLogService
    {

        private readonly ApplicationDbContext _context;

        public ExportLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogExport(string userId, string exportType, string filtersJson, int rowCount, string fileReference, DateTime? expiryDate = null)
        {
            var exportLog = new ExportLog
            {
                UserId = userId,
                ExportType = exportType,
                FiltersJson = filtersJson,
                RowCount = rowCount,
                FileReference = fileReference,
                ExpiryDate = expiryDate,
                Timestamp = DateTime.UtcNow
            };

            await _context.ExportLogs.AddAsync(exportLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ExportLogViewModel>> GetExportLogs(ExportLogFilterViewModel filters)
        {
            var query = _context.ExportLogs
                .Include(e => e.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filters.UserId))
                query = query.Where(e => e.UserId == filters.UserId);

            if (!string.IsNullOrEmpty(filters.ExportType))
                query = query.Where(e => e.ExportType == filters.ExportType);

            if (filters.DateFrom.HasValue)
                query = query.Where(e => e.Timestamp >= filters.DateFrom.Value);

            if (filters.DateTo.HasValue)
                query = query.Where(e => e.Timestamp <= filters.DateTo.Value);

            if (filters.Expired.HasValue)
            {
                if (filters.Expired.Value)
                {
                    query = query.Where(e => e.ExpiryDate.HasValue && e.ExpiryDate.Value < DateTime.UtcNow);
                }
                else
                {
                    query = query.Where(e => !e.ExpiryDate.HasValue || e.ExpiryDate.Value >= DateTime.UtcNow);
                }
            }

            query = query.OrderByDescending(e => e.Timestamp);

            return await query.Select(e => new ExportLogViewModel
            {
                ExportLogId = e.ExportLogId,
                UserId = e.UserId,
                UserName = e.User.UserName,
                ExportType = e.ExportType,
                FiltersJson = e.FiltersJson,
                RowCount = e.RowCount,
                FileReference = e.FileReference,
                Timestamp = e.Timestamp,
                ExpiryDate = e.ExpiryDate
            }).ToListAsync();
        }

        public async Task<int> CleanupExpiredExports()
        {
            var expiredLogs = await _context.ExportLogs
                .Where(e => e.ExpiryDate.HasValue && e.ExpiryDate.Value < DateTime.UtcNow)
                .ToListAsync();

            if (!expiredLogs.Any())
                return 0;

            _context.ExportLogs.RemoveRange(expiredLogs);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateExportPermission(string userId, string exportType)
        {
            // هنا يمكنك إضافة منطق التحقق من الصلاحيات
            // مثلاً: التحقق من عدد عمليات التصدير في آخر 24 ساعة

            var last24Hours = DateTime.UtcNow.AddHours(-24);
            var exportCount = await _context.ExportLogs
                .CountAsync(e => e.UserId == userId &&
                               e.ExportType == exportType &&
                               e.Timestamp >= last24Hours);

            // مثال: الحد الأقصى 10 عمليات تصدير في 24 ساعة
            var maxExportsPerDay = 10;

            return exportCount < maxExportsPerDay;
        }
    }
}

