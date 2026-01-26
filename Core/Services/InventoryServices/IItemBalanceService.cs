using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.InventoryViewModels;

namespace Core.Services.InventoryServices
{
    public interface IItemBalanceService
    {
        Task<List<ItemTransactionViewModel>> ItemTransactionSheet(int itemId, int branchId, DateTime fromDate, DateTime toDate);
        Task<decimal> GetItemOpenBalance(int itemId, int branchId, DateTime fromDate);
        Task<ItemBalanceSummaryViewModel> GetItemBalanceSummary(int itemId, int branchId, DateTime asOfDate);
    }
}
