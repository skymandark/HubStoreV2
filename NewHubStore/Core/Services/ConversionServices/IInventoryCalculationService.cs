using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.InventoryViewModels;

namespace Core.Services.ConversionServices
{
    public interface IInventoryCalculationService
    {
        Task<BalanceViewModel> CalculateBalanceAsync(int itemId, int branchId, DateTime? asOfDate = null);
        Task<OpeningBalanceViewModel> GetOpeningBalanceAsync(int itemId, int branchId, int fiscalYear);
        Task<Dictionary<int, BalanceViewModel>> CalculateBalanceByBranchAsync(int branchId, DateTime? asOfDate = null);
        Task<MovementImpactViewModel> GetMovementImpactAsync(int movementId);
        Task<ValidationResult> ValidateStockAvailabilityAsync(int itemId, int branchId, decimal qtyBase);
    }
}
