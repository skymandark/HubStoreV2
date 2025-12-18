using System;
using System.Threading.Tasks;
using Core.ViewModels;
using Core.ViewModels.ConversionViewModels;

namespace Core.Services.ConversionServices
{
    public interface IUnitConversionService
    {
        Task<ConversionResultViewModel> ConvertToBaseAsync(int itemId, string unitCode, decimal qtyInput);
        Task<BreakdownResultViewModel> BreakdownFromBaseAsync(int itemId, decimal baseQty);
        Task<decimal> GetConversionFactorAsync(int itemId, string unitCode, DateTime? asOfDate = null);
        Task<ValidationResult> ValidateConversionAsync(int itemId, string unitCode, decimal qty);
    }
}
