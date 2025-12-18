using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.ItemViewModels;

namespace Core.Services.ItemServices
{
    public interface IItemService
    {
        Task<ItemViewModel> CreateItemAsync(CreateItemViewModel itemDto);
        Task<ItemViewModel> UpdateItemAsync(int itemId, UpdateItemViewModel itemDto);
        Task<ItemViewModel> GetItemAsync(int itemId);
        Task<PaginatedResult<ItemViewModel>> GetItemsAsync(ItemFilterViewModel filters, PaginationViewModel pagination);
        Task<bool> DeleteItemAsync(int itemId);
        Task<ItemViewModel> GetItemByBarcodeAsync(string barcode, bool isInternal);
        Task<ItemHierarchyViewModel> GetItemHierarchyAsync();
        Task<ValidationResult> ValidateItemForMovementAsync(int itemId);
    }
}
