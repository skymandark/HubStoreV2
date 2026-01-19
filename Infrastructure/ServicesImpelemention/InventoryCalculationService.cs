using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels;
using Core.ViewModels.InventoryViewModels;
using Core.Services.InventoryServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HubStoreV2.Services.InventoryServices
{
    public class InventoryCalculationService : IInventoryCalculationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InventoryCalculationService> _logger;

        public InventoryCalculationService(ApplicationDbContext context, ILogger<InventoryCalculationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Calculates current balance for an item in a branch as of a date
        /// </summary>
        public async Task<BalanceViewModel> CalculateBalanceAsync(int itemId, int branchId, DateTime asOfDate)
        {
            try
            {
                // Get item
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                    throw new InvalidOperationException($"Item {itemId} not found.");

                // Get branch
                var branch = await _context.Branches
                    .Where(b => b.BranchId == branchId)
                    .FirstOrDefaultAsync();

                if (branch == null)
                    throw new InvalidOperationException($"Branch {branchId} not found.");

                // Get fiscal year for the date
                var fiscalYear = asOfDate.Year; // Simplistic approach - adjust based on your fiscal year logic

                // Get opening balance
                var openingBalance = await _context.OpeningBalances
                    .Where(ob => ob.ItemId == itemId && 
                           ob.BranchId == branchId && 
                           ob.FiscalYear == fiscalYear)
                    .FirstOrDefaultAsync();

                decimal balanceQtyBase = openingBalance?.OpeningQuantityBase ?? 0;
                decimal balanceCost = openingBalance?.CostValue ?? 0;

                // Get all movements for this item/branch up to asOfDate
                var movements = await _context.MovementLines
                    .Where(ml => ml.ItemId == itemId && 
                           ml.BranchId == branchId &&
                           _context.Movements
                               .Where(m => m.MovementId == ml.MovementId && m.Date <= asOfDate)
                               .Any())
                    .Include(ml => ml.Movement)
                    .ToListAsync();

                // Calculate impact from movements
                foreach (var movement in movements)
                {
                    // Get movement type to determine if it's inbound or outbound
                    var movementType = await _context.MovementTypes
                        .Where(mt => mt.MovementTypeId == movement.Movement.MovementTypeId)
                        .FirstOrDefaultAsync();

                    if (movementType != null)
                    {
                        // Assuming movement type code indicates direction (IN/OUT)
                        bool isInbound = movementType.Code.Contains("IN") || 
                                       movementType.Code.Contains("RECEIPT") ||
                                       movementType.Code.Contains("OPENING");

                        if (isInbound)
                        {
                            balanceQtyBase += movement.QtyBase;
                            balanceCost += movement.UnitPrice * movement.QtyBase;
                        }
                        else
                        {
                            balanceQtyBase -= movement.QtyBase;
                            balanceCost -= movement.UnitPrice * movement.QtyBase;
                        }
                    }
                }

                // Calculate unit price
                var unitPrice = balanceQtyBase > 0 ? balanceCost / balanceQtyBase : 0;

                _logger.LogInformation($"Balance calculated for item {itemId} in branch {branchId}: {balanceQtyBase} units");

                return new BalanceViewModel
                {
                    ItemId = itemId,
                    ItemCode = item.ItemCode,
                    BranchId = branchId,
                    BranchName = branch.Name,
                    QuantityBase = balanceQtyBase,
                    CostValue = balanceCost,
                    UnitPrice = unitPrice,
                    BaseUnitCode = item.BaseUnitCode,
                    AsOfDate = asOfDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calculating balance for item {itemId} in branch {branchId}");
                throw new ApplicationException($"Error calculating balance: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets opening balance for an item in a branch for a fiscal year
        /// </summary>
        public async Task<OpeningBalanceViewModel> GetOpeningBalanceAsync(int itemId, int branchId, int fiscalYear)
        {
            try
            {
                var openingBalance = await _context.OpeningBalances
                    .Where(ob => ob.ItemId == itemId && 
                           ob.BranchId == branchId && 
                           ob.FiscalYear == fiscalYear)
                    .Include(ob => ob.Item)
                    .Include(ob => ob.Branch)
                    .FirstOrDefaultAsync();

                if (openingBalance == null)
                    throw new InvalidOperationException(
                        $"No opening balance found for item {itemId}, branch {branchId}, fiscal year {fiscalYear}.");

                _logger.LogInformation($"Opening balance retrieved for item {itemId}, branch {branchId}, fiscal year {fiscalYear}");

                return new OpeningBalanceViewModel
                {
                    OpeningBalanceId = openingBalance.OpeningBalanceId,
                    ItemId = openingBalance.ItemId,
                    ItemCode = openingBalance.Item.ItemCode,
                    BranchId = openingBalance.BranchId,
                    BranchName = openingBalance.Branch.Name,
                    FiscalYear = openingBalance.FiscalYear,
                    OpeningQuantityBase = openingBalance.OpeningQuantityBase,
                    CostValue = openingBalance.CostValue,
                    CreatedDate = openingBalance.CreatedDate,
                    CreatedBy = openingBalance.CreatedBy
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting opening balance for item {itemId}, branch {branchId}");
                throw new ApplicationException($"Error getting opening balance: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets balance for all items in a branch as of a date
        /// </summary>
        public async Task<List<BalanceViewModel>> CalculateBalanceByBranchAsync(int branchId, DateTime asOfDate)
        {
            try
            {
                // Get all items
                var items = await _context.Items
                    .Where(i => !i.IsDeleted && i.IsActive)
                    .ToListAsync();

                var balances = new List<BalanceViewModel>();

                // Calculate balance for each item
                foreach (var item in items)
                {
                    try
                    {
                        var balance = await CalculateBalanceAsync(item.ItemId, branchId, asOfDate);
                        if (balance.QuantityBase > 0) // Only include items with balance
                        {
                            balances.Add(balance);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Could not calculate balance for item {item.ItemId}");
                    }
                }

                _logger.LogInformation($"Calculated balances for {balances.Count} items in branch {branchId}");

                return balances.OrderByDescending(b => b.CostValue).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calculating balances by branch {branchId}");
                throw new ApplicationException($"Error calculating branch balances: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Calculates the impact of a movement on inventory
        /// </summary>
        public async Task<MovementImpactViewModel> GetMovementImpactAsync(int movementId)
        {
            try
            {
                var movement = await _context.Movements
                    .Where(m => m.MovementId == movementId)
                    .FirstOrDefaultAsync();

                if (movement == null)
                    throw new InvalidOperationException($"Movement {movementId} not found.");

                // Get movement lines
                var movementLines = await _context.MovementLines
                    .Where(ml => ml.MovementId == movementId)
                    .FirstOrDefaultAsync();

                if (movementLines == null)
                    throw new InvalidOperationException($"No lines found for movement {movementId}.");

                // Get movement type to determine impact direction
                var movementType = await _context.MovementTypes
                    .Where(mt => mt.MovementTypeId == movement.MovementTypeId)
                    .FirstOrDefaultAsync();

                if (movementType == null)
                    throw new InvalidOperationException($"Movement type not found.");

                bool isInbound = movementType.Code.Contains("IN") || 
                               movementType.Code.Contains("RECEIPT") ||
                               movementType.Code.Contains("OPENING");

                decimal impact = isInbound ? movementLines.QtyBase : -movementLines.QtyBase;

                _logger.LogInformation($"Movement {movement.MovementCode} impact calculated: {impact} units");

                return new MovementImpactViewModel
                {
                    MovementId = movementId,
                    MovementCode = movement.MovementCode,
                    ItemId = movementLines.ItemId,
                    QtyBase = movementLines.QtyBase,
                    ImpactOnBalance = impact,
                    MovementDate = movement.Date
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calculating movement impact for movement {movementId}");
                throw new ApplicationException($"Error calculating movement impact: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validates if sufficient stock is available for a transaction
        /// </summary>
        public async Task<ValidationResult> ValidateStockAvailabilityAsync(int itemId, int branchId, decimal qtyBase)
        {
            var result = new ValidationResult { IsValid = true };

            try
            {
                if (qtyBase < 0)
                {
                    result.IsValid = false;
                    result.Errors.Add("Quantity cannot be negative.");
                    return result;
                }

                // Get current balance
                var balance = await CalculateBalanceAsync(itemId, branchId, DateTime.UtcNow);

                if (balance.QuantityBase < qtyBase)
                {
                    result.IsValid = false;
                    result.Errors.Add(
                        $"Insufficient stock. Available: {balance.QuantityBase} {balance.BaseUnitCode}, " +
                        $"Requested: {qtyBase} {balance.BaseUnitCode}");
                }

                result.Data["ItemId"] = itemId;
                result.Data["BranchId"] = branchId;
                result.Data["AvailableQty"] = balance.QuantityBase;
                result.Data["RequestedQty"] = qtyBase;
                result.Data["AvailableValue"] = balance.CostValue;

                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add($"Error validating stock availability: {ex.Message}");
                return result;
            }
        }
    }
}
