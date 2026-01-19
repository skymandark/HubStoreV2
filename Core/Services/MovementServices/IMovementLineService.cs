using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.MovementViewModels;

namespace Core.Services.MovementServices
{
    public interface IMovementLineService
    {
        Task<MovementLineViewModel> AddMovementLine(int movementId, CreateMovementLineViewModel lineDto);
        Task<MovementLineViewModel> UpdateMovementLine(int lineId, UpdateMovementLineViewModel lineDto);
        Task<bool> DeleteMovementLine(int lineId);
        Task<IEnumerable<MovementLineViewModel>> GetMovementLines(int movementId);
        Task<MovementLineTotalsViewModel> CalculateLineTotals(int lineId);
        Task<bool> ValidateMovementLine(CreateMovementLineViewModel lineDto);
    }
}
