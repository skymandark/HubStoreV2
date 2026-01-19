using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.StockOutReturnViewModels;

namespace Core.Services.OrderServices
{
    public interface IStockOutReturnService
    {
        Task<int> CreateStockOutReturn(StockOutReturnRequestDto dto, string user);
        Task UpdateStockOutReturn(StockOutReturnRequestDto dto, string user);
        Task ApproveStockOutReturn(int id, string user);
        Task ExecuteStockOutReturn(int id, string user);
        Task<List<StockOutReturnListDto>> GetStockOutReturns();
        Task<StockOutReturnRequestDto> GetStockOutReturn(int id);
        Task<List<ReturnOrderForStockOutDto>> GetReturnOrdersForStockOut();
    }
}