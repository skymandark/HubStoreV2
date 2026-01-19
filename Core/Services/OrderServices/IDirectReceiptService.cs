using System.Threading.Tasks;
using Core.ViewModels.DirectReceiptViewModels;

namespace Core.Services.OrderServices
{
    public interface IDirectReceiptService
    {
        Task<PurchaseOrderRequestVM> GetDirectReceipt(int id);
        Task<int> CreateDirectReceipt(PurchaseOrderRequestVM dto);
        Task UpdateDirectReceipt(PurchaseOrderRequestVM dto);
        Task DeleteDirectReceipt(int id);
        Task ApproveDirectReceipt(int id);
        Task<List<DirectReceiptListDto>> GetDirectReceipts();
    }
}