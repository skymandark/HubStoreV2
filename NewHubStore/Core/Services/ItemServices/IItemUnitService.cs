using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.ItemViewModels;

namespace Core.Services.ItemServices
{
    public interface IItemUnitService
    {
        Task<ItemUnitViewModel> CreateItemUnitAsync(int itemId, CreateItemUnitViewModel unitDto);
        Task<ItemUnitViewModel> UpdateItemUnitAsync(int itemUnitId, UpdateItemUnitViewModel unitDto);
        Task<List<ItemUnitViewModel>> GetItemUnitsAsync(int itemId);
        Task<bool> DeleteItemUnitAsync(int itemUnitId);
        Task<List<UnitConversionHistoryViewModel>> GetUnitConversionHistoryAsync(int itemId, string unitCode);
        Task<ValidationResult> ValidateUnitConversionAsync(int itemId, string unitCode);
    }
}
