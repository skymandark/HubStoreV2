using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.InventoryViewModels;

namespace Core.Services.InventoryServices
{
    public interface IInventoryCalculationService
    {
        /// <summary>
        /// Calculates current balance for an item in a branch as of a date
        /// </summary>
        Task<BalanceViewModel> CalculateBalanceAsync(int itemId, int branchId, DateTime asOfDate);

        /// <summary>
        /// Gets opening balance for an item in a branch for a fiscal year
        /// </summary>
        Task<OpeningBalanceViewModel> GetOpeningBalanceAsync(int itemId, int branchId, int fiscalYear);

        /// <summary>
        /// Gets balance for all items in a branch as of a date
        /// </summary>
        Task<List<BalanceViewModel>> CalculateBalanceByBranchAsync(int branchId, DateTime asOfDate);

        /// <summary>
        /// Calculates the impact of a movement on inventory
        /// </summary>
        Task<MovementImpactViewModel> GetMovementImpactAsync(int movementId);

        /// <summary>
        /// Validates if sufficient stock is available for a transaction
        /// </summary>
        Task<ValidationResult> ValidateStockAvailabilityAsync(int itemId, int branchId, decimal qtyBase);
    }
}
