using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.ReturnOrderViewModels;

namespace Core.Services.OrderServices
{
    public interface IReturnOrderService
    {
        Task<int> CreateReturnOrder(ReturnOrderRequestDto dto, string user);
        Task UpdateReturnOrder(ReturnOrderRequestDto dto, string user);
        Task ApproveReturnOrder(int id, string user);
        Task ExecuteReturnOrder(int id, string user);
        Task<List<ReturnOrderListDto>> GetReturnOrders();
    }
}