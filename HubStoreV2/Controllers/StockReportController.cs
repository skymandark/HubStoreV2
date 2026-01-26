using Core.Domin;
using Core.Services.ReportingServices;
using Core.Services.OrderServices;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace HubStoreV2.Controllers
{
    public class StockReportController : Controller
    {
        private readonly IInventoryReportService _inventoryReportService;
        private readonly ISupplierService _supplierService;
        private readonly ApplicationDbContext _context;

        public StockReportController(
            IInventoryReportService inventoryReportService,
            ISupplierService supplierService,
            ApplicationDbContext context)
        {
            _inventoryReportService = inventoryReportService;
            _supplierService = supplierService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "تقرير أرصدة المخزون";

            // Get branches for filter
            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .Select(b => new { b.BranchId, b.Name, b.Name_Arab })
                .ToListAsync();

            // Get vendors for filter
            var vendors = await _supplierService.GetSuppliers();

            var model = new
            {
                Branches = branches,
                Vendors = vendors
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ReadItemOpenBalances(
            int[] branchIds,
            int? vendorId = null,
            bool isSummary = true)
        {
            try
            {
                if (branchIds == null || branchIds.Length == 0)
                {
                    return Json(new { success = false, message = "يجب اختيار مخزن واحد على الأقل" });
                }

                // Get stock report data
                var filters = new Core.ViewModels.ReportViewModels.ReportFilterViewModel
                {
                    BranchId = branchIds.First(), // Use first branch for now
                    SupplierId = vendorId
                };

                var stockData = await _inventoryReportService.GetStockReport(filters, DateTime.Now);

                // Enhance data with additional fields
                var enhancedData = new List<object>();

                foreach (var item in stockData)
                {
                    // Get item details
                    var itemDetails = await _context.Items
                        .Include(i => i.MainItem)
                        .Include(i => i.SubItem)
                        .Include(i => i.Brand)
                        .Include(i => i.ItemType)
                        .Include(i => i.BuyMainPackage)
                        .Include(i => i.Section)
                        .FirstOrDefaultAsync(i => i.ItemId == item.ItemId);

                    if (itemDetails != null)
                    {
                        var enhancedItem = new
                        {
                            ItemId = item.ItemId,
                            ItemCode = item.ItemCode,
                            ItemName = itemDetails.NameArab,
                            ItemNameEng = itemDetails.NameEng,
                            MainItemName = itemDetails.MainItem?.NameArab,
                            SubItemName = itemDetails.SubItem?.NameArab,
                            BrandName = itemDetails.Brand?.NameArab,
                            ItemTypeName = itemDetails.ItemType?.NameArab,
                            PackageName = itemDetails.BuyMainPackage?.NameArab,
                            SectionName = itemDetails.Section?.NameArab,
                            BranchId = item.BranchId,
                            BranchName = item.BranchName,
                            OpenQuantity = 0, // TODO: Calculate from opening balances
                            BalanceQuantity = item.QuantityBase,
                            CostPrice = item.UnitPrice,
                            ConsumerPrice = itemDetails.CustomerPrice,
                            CostValue = item.CostValue,
                            VendorId = vendorId,
                            VendorName = vendorId.HasValue ?
                                (await _supplierService.GetSupplier(vendorId.Value))?.Name : null,
                            StockDate = DateTime.Now,
                            ExpireDate = (DateTime?)null, // Not available in Item model
                            BatchNumber = (string)null // Not available in Item model
                        };

                        enhancedData.Add(enhancedItem);
                    }
                }

                // Group by item if summary view
                if (isSummary)
                {
                    var groupedData = enhancedData
                        .GroupBy(x => new { ItemId = (int)x.GetType().GetProperty("ItemId").GetValue(x) })
                        .Select(g =>
                        {
                            var first = g.First();
                            return new
                            {
                                ItemId = g.Key.ItemId,
                                ItemCode = first.GetType().GetProperty("ItemCode").GetValue(first),
                                ItemName = first.GetType().GetProperty("ItemName").GetValue(first),
                                ItemNameEng = first.GetType().GetProperty("ItemNameEng").GetValue(first),
                                MainItemName = first.GetType().GetProperty("MainItemName").GetValue(first),
                                SubItemName = first.GetType().GetProperty("SubItemName").GetValue(first),
                                BrandName = first.GetType().GetProperty("BrandName").GetValue(first),
                                ItemTypeName = first.GetType().GetProperty("ItemTypeName").GetValue(first),
                                PackageName = first.GetType().GetProperty("PackageName").GetValue(first),
                                SectionName = first.GetType().GetProperty("SectionName").GetValue(first),
                                OpenQuantity = g.Sum(x => (decimal)x.GetType().GetProperty("OpenQuantity").GetValue(x)),
                                BalanceQuantity = g.Sum(x => (decimal)x.GetType().GetProperty("BalanceQuantity").GetValue(x)),
                                CostPrice = first.GetType().GetProperty("CostPrice").GetValue(first),
                                ConsumerPrice = first.GetType().GetProperty("ConsumerPrice").GetValue(first),
                                CostValue = g.Sum(x => (decimal)x.GetType().GetProperty("CostValue").GetValue(x)),
                                VendorId = first.GetType().GetProperty("VendorId").GetValue(first),
                                VendorName = first.GetType().GetProperty("VendorName").GetValue(first),
                                StockDate = first.GetType().GetProperty("StockDate").GetValue(first),
                                ExpireDate = first.GetType().GetProperty("ExpireDate").GetValue(first),
                                BatchNumber = first.GetType().GetProperty("BatchNumber").GetValue(first)
                            };
                        })
                        .ToList();

                    return Json(new { success = true, data = groupedData });
                }

                return Json(new { success = true, data = enhancedData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Details(int[] branchIds, int? vendorId = null, bool isSummary = true)
        {
            ViewData["Title"] = "تقرير أرصدة المخزون - تفاصيل";

            // Get the data for printing
            var result = await ReadItemOpenBalances(branchIds, vendorId, isSummary) as JsonResult;
            var data = result?.Value as dynamic;

            if (data?.success != true)
            {
                TempData["Error"] = data?.message ?? "حدث خطأ في جلب البيانات";
                return RedirectToAction("Index");
            }

            var model = new
            {
                ReportData = data.data,
                BranchIds = branchIds,
                VendorId = vendorId,
                IsSummary = isSummary,
                ReportDate = DateTime.Now,
                CompanyName = "HubStore ERP"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetItemBalanceValue(int itemId, int branchId)
        {
            try
            {
                // Calculate balance for specific item and branch
                var filters = new Core.ViewModels.ReportViewModels.ReportFilterViewModel
                {
                    BranchId = branchId,
                    ItemId = itemId
                };

                var stockData = await _inventoryReportService.GetStockReport(filters, DateTime.Now);
                var balance = stockData.FirstOrDefault()?.QuantityBase ?? 0;

                return Json(new { success = true, balance = balance });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}