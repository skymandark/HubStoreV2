using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.StockInViewModels;

namespace Core.Services.OrderServices
{
    public interface IStockInService
    {
        Task<int> CreateStockIn(StockInRequestDto dto, string user);
        Task UpdateStockIn(StockInRequestDto dto, string user);
        Task ApproveStockIn(int id, string user);
        Task ReceiveStockIn(int id, string user);
        Task<List<StockInListDto>> GetStockIns();
    }
}