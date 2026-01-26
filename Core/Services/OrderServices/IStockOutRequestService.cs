using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.StockOutRequestViewModels;
using Core.ViewModels.TransferOrderViewModels;
using Core.ViewModels.OrderViewModels;

namespace Core.Services.OrderServices
{
    public interface IStockOutRequestService
    {
        Task<int> SaveStockOutRequest(StockOutRequestDto dto, string user);
        Task<List<TransferOrderListDto>> GetTransferOrdersForStockOut(int branchId);
        Task<List<TransferOrderDetailDto>> GetTransferOrderDetailsForStockOut(int transferOrderId);
        Task<List<OrderViewModel>> GetSellOrdersForStockOut(int clientId);
        Task<List<OrderLineViewModel>> GetSellOrderDetailsForStockOut(int sellOrderId);
        Task<List<StockOutRequestListDto>> GetStockOutRequests();
        Task<StockOutRequestDto> GetStockOutRequestById(int id);
        Task DeleteStockOutRequest(int id, string user);
    }
}