using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels.InventoryViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class NewInventoryCalculationService
    {
        private readonly ApplicationDbContext _context;

        public NewInventoryCalculationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BalanceViewModel> CalculateBalanceAsync(int itemId, int branchId, DateTime asOfDate)
        {
            // Get all layers for the item/branch
            var layers = await _context.InventoryLayers
                .Where(il => il.ItemId == itemId && il.BranchId == branchId && !il.IsDeleted)
                .OrderBy(il => il.ReceiptDate)
                .ToListAsync();

            if (!layers.Any())
                return new BalanceViewModel { ItemId = itemId, BranchId = branchId, QuantityBase = 0, CostValue = 0, UnitPrice = 0 };

            // Calculate remaining for each layer
            var layerBalances = new List<(decimal Remaining, decimal Cost)>();

            foreach (var layer in layers)
            {
                // Calculate total consumed from this layer up to asOfDate
                var totalConsumed = await _context.MovementLines
                    .Where(ml => ml.ItemId == itemId && ml.BranchId == branchId &&
                           ml.MovementDate <= asOfDate &&
                           ml.Movement.MovementType.Code.Contains("ISSUE") || ml.Movement.MovementType.Code.Contains("SALE"))
                    .SumAsync(ml => Math.Abs(ml.QtyInput));

                // For FIFO, we need to distribute consumption to layers in order
                // This is simplified; in real implementation, use cumulative logic
                var remaining = Math.Max(0, layer.QuantityReceived - totalConsumed);
                layerBalances.Add((remaining, layer.UnitCost));
            }

            // Calculate total balance
            var totalQuantity = layerBalances.Sum(l => l.Remaining);
            var totalCostValue = layerBalances.Sum(l => l.Remaining * l.Cost);
            var avgUnitPrice = totalQuantity > 0 ? totalCostValue / totalQuantity : 0;

            return new BalanceViewModel
            {
                ItemId = itemId,
                BranchId = branchId,
                QuantityBase = totalQuantity,
                CostValue = totalCostValue,
                UnitPrice = avgUnitPrice,
                AsOfDate = asOfDate
            };
        }

        // Improved version using SQL for better performance
        public async Task<BalanceViewModel> CalculateBalanceOptimizedAsync(int itemId, int branchId, DateTime asOfDate)
        {
            var query = @"
                WITH LayerConsumption AS (
                    SELECT il.InventoryLayerId,
                           il.QuantityReceived,
                           il.UnitCost,
                           il.ReceiptDate,
                           COALESCE(SUM(ABS(ml.QtyInput)) OVER (
                               PARTITION BY il.ItemId, il.BranchId
                               ORDER BY il.ReceiptDate
                               ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
                           ), 0) as CumulativeConsumed
                    FROM InventoryLayers il
                    LEFT JOIN MovementLines ml ON ml.ItemId = il.ItemId
                                              AND ml.BranchId = il.BranchId
                                              AND ml.MovementDate <= {0}
                                              AND (ml.Movement.MovementType.Code LIKE '%ISSUE%' OR ml.Movement.MovementType.Code LIKE '%SALE%')
                    WHERE il.ItemId = {1} AND il.BranchId = {2} AND il.IsDeleted = 0
                )
                SELECT SUM(GREATEST(0, QuantityReceived - CumulativeConsumed)) as TotalQuantity,
                       SUM(GREATEST(0, QuantityReceived - CumulativeConsumed) * UnitCost) as TotalCostValue
                FROM LayerConsumption";

            var parameters = new object[] { asOfDate, itemId, branchId };
            var result = await _context.Database.SqlQueryRaw<BalanceResult>(query, parameters).FirstOrDefaultAsync();

            return new BalanceViewModel
            {
                ItemId = itemId,
                BranchId = branchId,
                QuantityBase = result?.TotalQuantity ?? 0,
                CostValue = result?.TotalCostValue ?? 0,
                UnitPrice = result?.TotalQuantity > 0 ? result.TotalCostValue / result.TotalQuantity : 0,
                AsOfDate = asOfDate
            };
        }

        private class BalanceResult
        {
            public decimal TotalQuantity { get; set; }
            public decimal TotalCostValue { get; set; }
        }
    }
}