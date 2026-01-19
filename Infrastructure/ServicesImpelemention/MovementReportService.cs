using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services.ReportingServices;
using Core.ViewModels.ReportViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class MovementReportService:IMovementReportService
    {
        private readonly ApplicationDbContext _context;

        public MovementReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovementSummaryReportViewModel>> GetMovementSummary(
            ReportFilterViewModel filters, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = from movement in _context.Movements
                        join movementType in _context.MovementTypes on movement.MovementTypeId equals movementType.MovementTypeId
                        join status in _context.ApprovalStatuses on movement.ApprovalStatusId equals status.ApprovalStatusId
                        where (filters.BranchId == null ||
                               movement.BranchFromId == filters.BranchId ||
                               movement.BranchToId == filters.BranchId) &&
                              (dateFrom == null || movement.Date >= dateFrom) &&
                              (dateTo == null || movement.Date <= dateTo) &&
                              (string.IsNullOrEmpty(filters.Status) || status.Name == filters.Status)
                        select new
                        {
                            movement,
                            movementType,
                            status,
                            Lines = _context.MovementLines
                                .Where(ml => ml.MovementId == movement.MovementId)
                                .Include(ml => ml.Item)
                                .ToList()
                        };

            var movements = await query.ToListAsync();

            return movements.Select(m => new MovementSummaryReportViewModel
            {
                MovementId = m.movement.MovementId,
                MovementCode = m.movement.MovementCode,
                MovementType = m.movementType.Name,
                MovementDate = m.movement.Date,
                LineCount = m.Lines.Count,
                TotalQuantity = m.Lines.Sum(l => l.Quantity),
                TotalValue = (decimal)m.Lines.Sum(l => l.Quantity * l.Item.UnitPrice),
                Status = m.status.Name
            }).OrderByDescending(m => m.MovementDate).ToList();
        }

        public async Task<MovementDetailsReportViewModel> GetMovementDetails(int movementId)
        {
            var movement = await _context.Movements
                .Include(m => m.MovementType)
                .Include(m => m.ApprovalStatus)
                .FirstOrDefaultAsync(m => m.MovementId == movementId);

            if (movement == null)
                throw new ArgumentException($"Movement with ID {movementId} not found");

            var lines = await _context.MovementLines
                .Where(ml => ml.MovementId == movementId)
                .Include(ml => ml.Item)
                .ToListAsync();

            var lineDetails = lines.Select(l => new MovementLineDetailViewModel
            {
                LineNumber = l.LineNumber,
                ItemCode = l.Item.ItemCode,
                ItemName = l.Item.ItemName,
                Quantity = l.Quantity,
                UnitCode = l.UnitCode,
                UnitPrice = (decimal)l.Item.UnitPrice,
                LineValue = (decimal)(l.Quantity * l.Item.UnitPrice)
            }).ToList();

            return new MovementDetailsReportViewModel
            {
                MovementId = movement.MovementId,
                MovementCode = movement.MovementCode,
                MovementType = movement.MovementType.Name,
                MovementDate = movement.Date,
                Lines = lineDetails,
                TotalQuantity = lineDetails.Sum(l => l.Quantity),
                TotalValue = lineDetails.Sum(l => l.LineValue)
            };
        }

        public async Task<List<MovementSummaryReportViewModel>> GetMovementByType(
            string type, ReportFilterViewModel filters)
        {
            var query = from movement in _context.Movements
                        join movementType in _context.MovementTypes on movement.MovementTypeId equals movementType.MovementTypeId
                        join status in _context.ApprovalStatuses on movement.ApprovalStatusId equals status.ApprovalStatusId
                        where movementType.Name == type &&
                              (filters.BranchId == null ||
                               movement.BranchFromId == filters.BranchId ||
                               movement.BranchToId == filters.BranchId) &&
                              (filters.DateFrom == null || movement.Date >= filters.DateFrom) &&
                              (filters.DateTo == null || movement.Date <= filters.DateTo) &&
                              (string.IsNullOrEmpty(filters.Status) || status.Name == filters.Status)
                        select new
                        {
                            movement,
                            movementType,
                            status,
                            Lines = _context.MovementLines
                                .Where(ml => ml.MovementId == movement.MovementId)
                                .Include(ml => ml.Item)
                                .ToList()
                        };

            var movements = await query.ToListAsync();

            return movements.Select(m => new MovementSummaryReportViewModel
            {
                MovementId = m.movement.MovementId,
                MovementCode = m.movement.MovementCode,
                MovementType = m.movementType.Name,
                MovementDate = m.movement.Date,
                LineCount = m.Lines.Count,
                TotalQuantity = m.Lines.Sum(l => l.Quantity),
                TotalValue = (decimal)m.Lines.Sum(l => l.Quantity * l.Item.UnitPrice),
                Status = m.status.Name
            }).OrderByDescending(m => m.MovementDate).ToList();
        }

        public async Task<List<MovementSummaryReportViewModel>> GetApprovedMovementsOnly(
            ReportFilterViewModel filters)
        {
            var query = from movement in _context.Movements
                        join movementType in _context.MovementTypes on movement.MovementTypeId equals movementType.MovementTypeId
                        join status in _context.ApprovalStatuses on movement.ApprovalStatusId equals status.ApprovalStatusId
                        where (status.Name == "Approved" || status.Name == "Completed") &&
                              (filters.BranchId == null ||
                               movement.BranchFromId == filters.BranchId ||
                               movement.BranchToId == filters.BranchId) &&
                              (filters.DateFrom == null || movement.Date >= filters.DateFrom) &&
                              (filters.DateTo == null || movement.Date <= filters.DateTo)
                        select new
                        {
                            movement,
                            movementType,
                            status,
                            Lines = _context.MovementLines
                                .Where(ml => ml.MovementId == movement.MovementId)
                                .Include(ml => ml.Item)
                                .ToList()
                        };

            var movements = await query.ToListAsync();

            return movements.Select(m => new MovementSummaryReportViewModel
            {
                MovementId = m.movement.MovementId,
                MovementCode = m.movement.MovementCode,
                MovementType = m.movementType.Name,
                MovementDate = m.movement.Date,
                LineCount = m.Lines.Count,
                TotalQuantity = m.Lines.Sum(l => l.Quantity),
                TotalValue = (decimal)m.Lines.Sum(l => l.Quantity * l.Item.UnitPrice),
                Status = m.status.Name
            }).OrderByDescending(m => m.MovementDate).ToList();
        }
    }
}
