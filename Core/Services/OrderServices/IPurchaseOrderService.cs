using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.PurchaseOrderViewModels;

namespace Core.Services.OrderServices
{
    public interface IPurchaseOrderService
    {
        Task<PurchaseOrderRequestDto?> GetPurchaseOrder(int id);
        Task<int> CreatePurchaseOrder(PurchaseOrderRequestDto dto);
        Task UpdatePurchaseOrder(PurchaseOrderRequestDto dto);
        Task DeletePurchaseOrder(int id);
        Task UpdateQuantities(int id, List<PurchaseOrderDetailDto> details);
        Task<List<PurchaseOrderListDto>> GetPurchaseOrders();
        Task SubmitForApproval(int id, string user);
        Task ApprovePurchaseOrder(int id, string user);
        Task RejectPurchaseOrder(int id, string user, string reason);
        Task ClosePurchaseOrder(int id, string user);
        Task CancelPurchaseOrder(int id, string user);
    }
}
