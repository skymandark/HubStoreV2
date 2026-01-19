using System;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.MovementViewModels;

namespace Core.Services.MovementServices
{
    public interface ITransferService
    {
        Task<MovementViewModel> CreateTransferOut(CreateTransferViewModel transferDto);
        Task<MovementViewModel> CreateTransferIn(CreateTransferViewModel transferDto);
        Task<bool> ValidateTransferAvailability(int itemId, int branchFrom, decimal qtyBase);
        Task<bool> CompleteTransfer(int transferOutId, int transferInId);
    }
}
