using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;

namespace Core.Services.PerformanceServices
{
    public interface IBatchProcessingService
    {
        Task<BatchResultViewModel> BatchApproveAsync(List<int> documentIds, string userId);
        Task<BatchResultViewModel> BatchImportItemsAsync(List<object> itemsList);
        Task<BatchResultViewModel> BatchUpdatePricesAsync(List<object> priceList);
    }
}
