using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.DirectReturnStockOutViewModels;

namespace Core.Services.OrderServices
{
    public interface IDirectReturnStockOutService
    {
        Task<int> CreateDirectReturnStockOut(DirectReturnStockOutRequestDto dto, string user);
        Task UpdateDirectReturnStockOut(DirectReturnStockOutRequestDto dto, string user);
        Task ApproveDirectReturnStockOut(int id, string user);
        Task ExecuteDirectReturnStockOut(int id, string user);
        Task<List<DirectReturnStockOutListDto>> GetDirectReturnStockOuts();
        Task<DirectReturnStockOutRequestDto> GetDirectReturnStockOut(int id);
    }
}