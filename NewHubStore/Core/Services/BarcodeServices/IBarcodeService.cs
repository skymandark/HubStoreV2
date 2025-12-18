using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Core.ViewModels.BarcodeViewModels;

namespace Core.Services.BarcodeServices
{
    public interface IBarcodeService
    {
        Task<BarcodeViewModel> GenerateInternalBarcodeAsync(int itemId);
        Task<ValidationResult> ValidateBarcodeAsync(string barcode, string type);
        Task<BarcodeDetailsViewModel> SearchByBarcodeAsync(string barcode, string searchType);
        Task<BarcodeDetailsViewModel> GetBarcodeDetailsAsync(string barcode);
        Task<bool> PrintBarcodeAsync(int itemId, int quantity);
        Task<List<BarcodeViewModel>> BulkGenerateBarcodesAsync(List<int> itemIds);
    }
}
