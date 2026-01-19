using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.MovementViewModels;

namespace Core.Services.MovementServices
{
    public interface IMovementService
    {
        Task<MovementViewModel> CreateMovement(CreateMovementViewModel movementDto);
        Task<MovementViewModel> UpdateMovement(int movementId, UpdateMovementViewModel movementDto);
        Task<MovementViewModel> GetMovement(int movementId);
        Task<IEnumerable<MovementViewModel>> GetMovements(MovementFilterViewModel filters, PaginationViewModel pagination);
        Task<bool> DeleteMovement(int movementId);
        Task<bool> SubmitMovement(int movementId);
        Task<IEnumerable<MovementViewModel>> GetMovementsByBarcode(string barcode);
    }
}
