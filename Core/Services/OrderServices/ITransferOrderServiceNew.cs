using System.Threading.Tasks;
using System.Collections.Generic;
using Core.ViewModels.TransferOrderViewModels;

namespace Core.Services.OrderServices
{
    public interface ITransferOrderServiceNew
    {
        Task<TransferOrderRequestDto?> GetTransferOrder(int id);
        Task<int> CreateTransferOrder(TransferOrderRequestDto dto);
        Task UpdateTransferOrder(TransferOrderRequestDto dto);
        Task DeleteTransferOrder(int id);
        Task UpdateQuantities(int id, List<TransferOrderDetailDto> details);
        Task<List<TransferOrderListDto>> GetTransferOrders();

        Task ApproveTransferOrder(int id, string user);
        Task ExecuteTransferOrder(int id, List<TransferOrderDetailDto> details, string user);
        Task ReceiveTransferOrder(int id, List<TransferOrderDetailDto> details, string user);
    }
}