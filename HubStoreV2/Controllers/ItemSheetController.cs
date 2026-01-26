using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Services.InventoryServices;
using Core.Services.ItemServices;
using Core.Services;
using Core.ViewModels.InventoryViewModels;
using Core.Domin;

namespace HubStoreV2.Controllers
{
    //[Authorize]
    public class ItemSheetController : Controller
    {
        private readonly IItemBalanceService _itemBalanceService;
        private readonly IItemService _itemService;
        private readonly IBranchService _branchService;
        private readonly ISystemUtilityService _systemUtilityService;

        public ItemSheetController(
            IItemBalanceService itemBalanceService,
            IItemService itemService,
            IBranchService branchService,
            ISystemUtilityService systemUtilityService)
        {
            _itemBalanceService = itemBalanceService;
            _itemService = itemService;
            _branchService = branchService;
            _systemUtilityService = systemUtilityService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "كشف حركة الصنف";

            // Get items for filter
            var itemsResult = await _itemService.GetItemsAsync(new Core.ViewModels.ItemViewModels.ItemFilterViewModel(), new Core.ViewModels.PaginationViewModel { PageSize = 1000 });
            var items = itemsResult.Data;

            // Get branches for filter
            var branches = await _branchService.GetAllBranches();

            var model = new
            {
                Items = items,
                Branches = branches
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(int itemId, int branchId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var transactions = await _itemBalanceService.ItemTransactionSheet(itemId, branchId, fromDate, toDate);
                return Json(new { success = true, data = transactions });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemOpenBalance(int itemId, int branchId, DateTime fromDate)
        {
            try
            {
                var balance = await _itemBalanceService.GetItemOpenBalance(itemId, branchId, fromDate);
                return Json(new { success = true, data = balance });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemName(string term)
        {
            try
            {
                var items = await _itemService.SearchItemsByName(term);
                var result = items.Select(i => new { i.ItemId, i.ItemName, i.ItemCode });
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemId(string term)
        {
            try
            {
                var items = await _itemService.SearchItemsByCode(term);
                var result = items.Select(i => new { i.ItemId, i.ItemName, i.ItemCode });
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBranchName(string term)
        {
            try
            {
                var branches = await _branchService.SearchBranchesByName(term);
                var result = branches.Select(b => new { b.BranchId, b.Name, b.Code });
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBranchId(string term)
        {
            try
            {
                var branches = await _branchService.SearchBranchesByCode(term);
                var result = branches.Select(b => new { b.BranchId, b.Name, b.Code });
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int itemId, int branchId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var transactions = await _itemBalanceService.ItemTransactionSheet(itemId, branchId, fromDate, toDate);
                var item = await _itemService.GetItemById(itemId);
                var branch = await _branchService.GetBranchById(branchId);
                
                ViewBag.Item = item;
                ViewBag.Branch = branch;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.CompanyInfo = await _systemUtilityService.GetCompanyInfo();
                
                return View(transactions);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
