using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.StockInViewModels;
using Core.ViewModels.PurchaseOrderViewModels;

namespace Core.Services.OrderServices
{
    public interface IStockInService
    {
        Task<int> CreateStockIn(StockInRequestDto dto, string user);
        Task UpdateStockIn(StockInRequestDto dto, string user);
        Task DeleteStockIn(int id, string user);
        Task ApproveStockIn(int id, string user);
        Task ReceiveStockIn(int id, string user);
        Task DirectSaveStockIn(int purchaseOrderId, List<StockInDetailDto> details, string user);
        Task<List<StockInListDto>> GetStockIns();
        Task<List<PurchaseOrderDetailDto>> GetPurchaseOrderDetailsForReceipt(int purchaseOrderId);
        Task CalculateValues(StockInRequestDto dto);
    }
}