using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.TransferOrderViewModels;

namespace Core.Services.OrderServices
{
    public interface ITransferOrderService
    {
        Task<int> CreateTransferOrder(TransferOrderRequestDto dto, string user);
        Task UpdateTransferOrder(TransferOrderRequestDto dto, string user);
        Task ApproveTransferOrder(int id, string user);
        Task ExecuteTransferOrder(int id, string user);
        Task<List<TransferOrderListDto>> GetTransferOrders();
    }
}
