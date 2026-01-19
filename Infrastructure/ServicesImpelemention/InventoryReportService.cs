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
    public class InventoryReportService: IInventoryReportService
    {
        private readonly ApplicationDbContext _context;

        public InventoryReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockReportViewModel>> GetStockReport(ReportFilterViewModel filters, DateTime? asOfDate)
        {
            // حساب المخزون من الرصيد الافتتاحي والحركات
            var query = from item in _context.Items
                        join opening in _context.OpeningBalances on item.ItemId equals opening.ItemId into openingGroup
                        from opening in openingGroup.DefaultIfEmpty()
                        join movementLine in _context.MovementLines on item.ItemId equals movementLine.ItemId into movementGroup
                        from movementLine in movementGroup.DefaultIfEmpty()
                        join movement in _context.Movements on movementLine.MovementId equals movement.MovementId into movementDetails
                        from movement in movementDetails.DefaultIfEmpty()
                        where (filters.BranchId == null || opening.BranchId == filters.BranchId)
                        group new { opening, movementLine, movement } by new
                        {
                            item.ItemId,
                            item.ItemCode,
                            item.ItemName,
                            item.AverageCost,
                            item.UnitPrice,
                            BranchId = opening.BranchId,
                            BranchName = opening.Branch.Name
                        } into g
                        select new StockReportViewModel
                        {
                            ItemId = g.Key.ItemId,
                            ItemCode = g.Key.ItemCode,
                            ItemName = g.Key.ItemName,
                            BranchId = g.Key.BranchId,
                            BranchName = g.Key.BranchName,
                            QuantityBase = g.Sum(x => x.opening != null ? x.opening.Quantity : 0) +
                                         g.Sum(x => x.movementLine != null &&
                                                  x.movement.MovementTypeId == 1 ? x.movementLine.Quantity : 0) -
                                         g.Sum(x => x.movementLine != null &&
                                                  x.movement.MovementTypeId == 2 ? x.movementLine.Quantity : 0),
                        
                            CostValue = (decimal)((g.Sum(x => x.opening != null ? x.opening.Quantity : 0) +
                                        g.Sum(x => x.movementLine != null &&
                                                 x.movement.MovementTypeId == 1 ? x.movementLine.Quantity : 0) -
                                        g.Sum(x => x.movementLine != null &&
                                                 x.movement.MovementTypeId == 2 ? x.movementLine.Quantity : 0))
                                        * g.Key.AverageCost),
                            UnitPrice = (decimal)g.Key.UnitPrice,
                            AsOfDate = asOfDate ?? DateTime.UtcNow
                        };

            if (filters.ItemId.HasValue)
                query = query.Where(x => x.ItemId == filters.ItemId.Value);

            return await query.ToListAsync();
        }

        public async Task<List<StockMovementReportViewModel>> GetStockMovementReport(
            ReportFilterViewModel filters, DateTime? dateFrom, DateTime? dateTo)
        {
            var query = from ml in _context.MovementLines
                        join item in _context.Items on ml.ItemId equals item.ItemId
                        join movement in _context.Movements on ml.MovementId equals movement.MovementId
                        join movementType in _context.MovementTypes on movement.MovementTypeId equals movementType.MovementTypeId
                        join status in _context.ApprovalStatuses on movement.ApprovalStatusId equals status.ApprovalStatusId
                        where (filters.BranchId == null || ml.BranchId == filters.BranchId) &&
                              (filters.ItemId == null || ml.ItemId == filters.ItemId) &&
                              (dateFrom == null || movement.Date >= dateFrom) &&
                              (dateTo == null || movement.Date <= dateTo) &&
                              (string.IsNullOrEmpty(filters.Status) || status.Name == filters.Status)
                        select new StockMovementReportViewModel
                        {
                            ItemId = item.ItemId,
                            ItemCode = item.ItemCode,
                            MovementId = movement.MovementId,
                            MovementCode = movement.MovementCode,
                            MovementType = movementType.Name,
                            Quantity = ml.Quantity,
                            MovementDate = movement.Date,
                            Status = status.Name
                        };

            return await query.OrderByDescending(x => x.MovementDate).ToListAsync();
        }

        public async Task<List<StockValuationReportViewModel>> GetStockValuationReport(
            ReportFilterViewModel filters, DateTime? asOfDate)
        {
            var stockReport = await GetStockReport(filters, asOfDate);

            return stockReport.Select(s => new StockValuationReportViewModel
            {
                ItemId = s.ItemId,
                ItemCode = s.ItemCode,
                Quantity = s.QuantityBase,
                UnitCost = s.CostValue / (s.QuantityBase > 0 ? s.QuantityBase : 1),
                TotalValue = s.CostValue,
                AsOfDate = s.AsOfDate
            }).ToList();
        }

        public async Task<List<LowStockReportViewModel>> GetLowStockReport(
            ReportFilterViewModel filters, decimal threshold)
        {
            var stockReport = await GetStockReport(filters, DateTime.UtcNow);

            return stockReport.Select(s => new LowStockReportViewModel
            {
                ItemId = s.ItemId,
                ItemCode = s.ItemCode,
                ItemName = s.ItemName,
                BranchId = s.BranchId,
                CurrentQuantity = s.QuantityBase,
                MinimumThreshold = threshold,
                IsLowStock = s.QuantityBase <= threshold
            }).Where(s => s.IsLowStock).ToList();
        }

        public async Task<List<StockAgeingReportViewModel>> GetStockAgeingReport(
            ReportFilterViewModel filters)
        {
            var query = from ml in _context.MovementLines
                        join item in _context.Items on ml.ItemId equals item.ItemId
                        join movement in _context.Movements on ml.MovementId equals movement.MovementId
                        where (filters.BranchId == null || ml.BranchId == filters.BranchId) &&
                              (filters.ItemId == null || ml.ItemId == filters.ItemId)
                        group new { ml, movement } by new
                        {
                            item.ItemId,
                            item.ItemCode
                        } into g
                        select new StockAgeingReportViewModel
                        {
                            ItemId = g.Key.ItemId,
                            ItemCode = g.Key.ItemCode,
                            DaysInStock = (int)(DateTime.UtcNow - g.Max(x => x.movement.Date)).TotalDays,
                            Quantity = g.Sum(x => x.ml.Quantity),
                            AgeCategory = GetAgeCategory((int)(DateTime.UtcNow - g.Max(x => x.movement.Date)).TotalDays),
                            LastMovementDate = g.Max(x => x.movement.Date)
                        };

            return await query.OrderByDescending(x => x.DaysInStock).ToListAsync();
        }

        private string GetAgeCategory(int days)
        {
            if (days <= 30) return "Fresh";
            if (days <= 90) return "Old";
            return "Very Old";
        }

        public async Task<List<ABCAnalysisReportViewModel>> GetABCAnalysisReport(
            ReportFilterViewModel filters)
        {
            var currentYear = DateTime.UtcNow.Year;

            var query = from ml in _context.MovementLines
                        join item in _context.Items on ml.ItemId equals item.ItemId
                        join movement in _context.Movements on ml.MovementId equals movement.MovementId
                        where movement.Date.Year == currentYear &&
                              (filters.BranchId == null || ml.BranchId == filters.BranchId) &&
                              (filters.ItemId == null || ml.ItemId == filters.ItemId)
                        group new { ml, item } by new
                        {
                            item.ItemId,
                            item.ItemCode
                        } into g
                        select new
                        {
                            ItemId = g.Key.ItemId,
                            ItemCode = g.Key.ItemCode,
                            AnnualValue = g.Sum(x => x.ml.Quantity * x.item.UnitPrice)
                        };

            var annualValues = await query.ToListAsync();
            var totalValue = annualValues.Sum(av => av.AnnualValue);

            var result = annualValues
                .OrderByDescending(av => av.AnnualValue)
                .Select((av, index) => new ABCAnalysisReportViewModel
                {
                    ItemId = av.ItemId,
                    ItemCode = av.ItemCode,
                    AnnualValue = (decimal)av.AnnualValue,
                    PercentageOfTotal = (decimal)(totalValue > 0 ? (av.AnnualValue / totalValue) * 100 : 0),
                    Classification = GetABCClassification(index + 1, annualValues.Count)
                }).ToList();

            return result;
        }

        private string GetABCClassification(int rank, int totalItems)
        {
            var percentage = (double)rank / totalItems * 100;

            if (percentage <= 20) return "A";
            if (percentage <= 50) return "B";
            return "C";
        }
    }
}
