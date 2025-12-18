using System;
using System.Threading.Tasks;
using Core.ViewModels.OrderViewModels;

namespace Core.Services.OrderServices
{
    public interface IPurchaseOrderService
    {
        Task<OrderViewModel> CreatePO(CreatePurchaseOrderViewModel poDto);
        Task<OrderViewModel> ReceivePO(int orderId, ReceiveOrderViewModel receivingDto);
        Task<POStatusViewModel> GetPOStatus(int orderId);
    }
}
