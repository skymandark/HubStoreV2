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
using Microsoft.Extensions.Logging;

namespace Infrastructure.ServicesImpelemention
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly ApplicationDbContext _context;

        public AuditTrailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAction(string userId, string actionType, string documentType, int documentId,
                                  string changes = null, string oldValues = null, string newValues = null,
                                  string notes = null, string clientIp = null, string clientDevice = null)
        {
            var documentTypeEntity = await _context.DocumentTypes
                .FirstOrDefaultAsync(dt => dt.Name == documentType);

            if (documentTypeEntity == null)
            {
                throw new ArgumentException($"Document type '{documentType}' not found");
            }

            var auditTrail = new AuditTrail
            {
                UserId = userId,
                ActionType = actionType,
                DocumentTypeId = documentTypeEntity.DocumentTypeId,
                DocumentId = documentId,
                OldValues = oldValues,
                NewValues = newValues,
                Notes = notes ?? changes,
                ClientIp = clientIp,
                ClientDevice = clientDevice,
                ActionDate = DateTime.UtcNow
            };

            await _context.AuditTrails.AddAsync(auditTrail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditTrailViewModel>> GetAuditTrail(int documentId, string documentType)
        {
            var query = _context.AuditTrails
                .Include(a => a.User)
                .Include(a => a.DocumentType)
                .Where(a => a.DocumentId == documentId && a.DocumentType.Name == documentType)
                .OrderByDescending(a => a.ActionDate);

            return await query.Select(a => new AuditTrailViewModel
            {
                LogId = a.LogId,
                UserId = a.UserId,
                UserName = a.User.UserName,
                ActionType = a.ActionType,
                DocumentType = a.DocumentType.Name,
                DocumentId = a.DocumentId,
                ActionDate = a.ActionDate,
                OldValues = a.OldValues,
                NewValues = a.NewValues,
                Notes = a.Notes,
                ClientIp = a.ClientIp,
                ClientDevice = a.ClientDevice
            }).ToListAsync();
        }

        public async Task<UserActivityViewModel> GetUserActivity(string userId, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = _context.AuditTrails
                .Include(a => a.User)
                .Where(a => a.UserId == userId);

            if (dateFrom.HasValue)
                query = query.Where(a => a.ActionDate >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(a => a.ActionDate <= dateTo.Value);

            var activities = await query.ToListAsync();

            var actionSummaries = activities
                .GroupBy(a => a.ActionType)
                .Select(g => new ActionSummaryViewModel
                {
                    ActionType = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var user = await _context.Users.FindAsync(userId);

            return new UserActivityViewModel
            {
                UserId = userId,
                UserName = user?.UserName,
                TotalActions = activities.Count,
                Actions = actionSummaries
            };
        }

        public async Task<List<AuditTrailViewModel>> SearchAuditLogs(AuditSearchFilterViewModel filters)
        {
            var query = _context.AuditTrails
                .Include(a => a.User)
                .Include(a => a.DocumentType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filters.UserId))
                query = query.Where(a => a.UserId == filters.UserId);

            if (!string.IsNullOrEmpty(filters.ActionType))
                query = query.Where(a => a.ActionType == filters.ActionType);

            if (!string.IsNullOrEmpty(filters.DocumentType))
                query = query.Where(a => a.DocumentType.Name == filters.DocumentType);

            if (filters.DocumentId.HasValue)
                query = query.Where(a => a.DocumentId == filters.DocumentId.Value);

            if (filters.DateFrom.HasValue)
                query = query.Where(a => a.ActionDate >= filters.DateFrom.Value);

            if (filters.DateTo.HasValue)
                query = query.Where(a => a.ActionDate <= filters.DateTo.Value);

            if (!string.IsNullOrEmpty(filters.SearchText))
            {
                var searchText = filters.SearchText.ToLower();
                query = query.Where(a =>
                    a.User.UserName.ToLower().Contains(searchText) ||
                    a.Notes.ToLower().Contains(searchText) ||
                    a.OldValues.ToLower().Contains(searchText) ||
                    a.NewValues.ToLower().Contains(searchText));
            }

            query = query.OrderByDescending(a => a.ActionDate);

            return await query.Select(a => new AuditTrailViewModel
            {
                LogId = a.LogId,
                UserId = a.UserId,
                UserName = a.User.UserName,
                ActionType = a.ActionType,
                DocumentType = a.DocumentType.Name,
                DocumentId = a.DocumentId,
                ActionDate = a.ActionDate,
                OldValues = a.OldValues,
                NewValues = a.NewValues,
                Notes = a.Notes,
                ClientIp = a.ClientIp,
                ClientDevice = a.ClientDevice
            }).ToListAsync();
        }
    }
}

