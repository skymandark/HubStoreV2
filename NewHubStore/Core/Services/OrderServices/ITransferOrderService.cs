using System;
using System.Threading.Tasks;
using Core.ViewModels.OrderViewModels;

namespace Core.Services.OrderServices
{
    public interface ITransferOrderService
    {
        Task<OrderViewModel> CreateTO(CreateTransferOrderViewModel toDto);
        Task<bool> ProcessTO(int orderId);
    }
}
