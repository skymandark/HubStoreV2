using System.Threading.Tasks;
using Core.ViewModels.TransferOrderViewModels;

namespace Core.Services.OrderServices
{
    public interface ITransferOrderServiceNew
    {
        Task<TransferOrderRequestDto> GetTransferOrder(int id);
        Task<int> CreateTransferOrder(TransferOrderRequestDto dto);
        Task UpdateTransferOrder(TransferOrderRequestDto dto);
        Task DeleteTransferOrder(int id);
        Task UpdateQuantities(int id, List<TransferOrderDetailDto> details);
        Task<List<TransferOrderListDto>> GetTransferOrders();
    }
}