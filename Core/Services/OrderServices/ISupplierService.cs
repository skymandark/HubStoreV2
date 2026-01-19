using Core.ViewModels.SupplierViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services.OrderServices
{
    public interface ISupplierService
    {
        Task<List<SupplierListDto>> GetSuppliers();
        Task<SupplierRequestDto> GetSupplier(int id);
        Task<int> CreateSupplier(SupplierRequestDto dto, string user);
        Task UpdateSupplier(SupplierRequestDto dto, string user);
        Task DeleteSupplier(int id, string user);
        Task ToggleSupplierStatus(int id, string user);
    }
}
