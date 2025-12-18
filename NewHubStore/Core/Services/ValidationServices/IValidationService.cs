using System;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.MovementViewModels;
using Core.ViewModels.OrderViewModels;

namespace Core.Services.ValidationServices
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateMovementAsync(CreateMovementViewModel movementDto);
        Task<ValidationResult> ValidateOrderAsync(CreateOrderViewModel orderDto);
        Task<ValidationResult> ValidateStockAvailabilityAsync(int itemId, int branchId, decimal qty);
        Task<ValidationResult> ValidateApprovalEligibilityAsync(int documentId, string userId);
        Task<ValidationResult> ValidateUnitConversionAsync(int itemId, string unitCode);
        Task<ValidationResult> ValidateBusinessRulesAsync(object entity, string action);
    }
}
