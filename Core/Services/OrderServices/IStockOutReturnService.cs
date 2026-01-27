using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.StockOutReturnViewModels;

namespace Core.Services.OrderServices
{
    public interface IStockOutReturnService
    {
        Task<int> CreateStockOutReturn(StockOutReturnVm dto, string user);
        Task UpdateStockOutReturn(StockOutReturnVm dto, string user);
        Task ApproveStockOutReturn(int id, string user);
        Task RejectStockOutReturn(int id, string user, string reason);
        Task SubmitStockOutReturn(int id, string user);
        Task ExecuteStockOutReturn(int id, string user);
        Task<List<StockOutReturnListDto>> GetStockOutReturns();
        Task<StockOutReturnVm> GetStockOutReturn(int id);
        
        // New methods for the spec
        Task<IQueryable<StockOutReturnListDto>> GetStockOutReturnsQuery();
        Task<List<ReturnOrderForStockOutDto>> GetSalesOrdersForClient(int clientId, int branchId);
        Task<List<StockOutReturnDetailVm>> GetSalesOrderDetailsForReturn(int orderId);
    }
}