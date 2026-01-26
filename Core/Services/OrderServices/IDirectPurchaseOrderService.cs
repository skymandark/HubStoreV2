using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.DirectPurchaseOrderViewModels;

namespace Core.Services.OrderServices
{
    public interface IDirectPurchaseOrderService
    {
        Task<int> CreateDirectPurchaseOrder(CreateDirectPurchaseOrderViewModel dto, string user);
        Task UpdateDirectPurchaseOrder(UpdateDirectPurchaseOrderViewModel dto, string user);
        Task ApproveDirectPurchaseOrder(int id, string user);
        Task RejectDirectPurchaseOrder(int id, string user);
        Task DeleteDirectPurchaseOrder(int id, string user);
        Task<List<DirectPurchaseOrderListViewModel>> GetDirectPurchaseOrders();
        Task<DirectPurchaseOrderViewModel> GetDirectPurchaseOrder(int id);
        Task CalculateTotals(CreateDirectPurchaseOrderViewModel dto);
    }
}