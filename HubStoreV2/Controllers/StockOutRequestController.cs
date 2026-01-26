using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels.StockOutRequestViewModels;
using Core.ViewModels.TransferOrderViewModels;
using Core.ViewModels.OrderViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HubStoreV2.Controllers
{
    //[Authorize]
    public class StockOutRequestController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<StockOutRequestController> _logger;

        public StockOutRequestController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<StockOutRequestController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: StockOutRequest/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // POST: StockOutRequest/ReadDx (Pending Requests)
        [HttpPost]
        public async Task<IActionResult> ReadDx()
        {
            try
            {
                var requests = await _context.StockOutHeaders
                    .Where(h => h.Status == 0) // 0 means Draft
                    .Select(h => new StockOutRequestListDto
                    {
                        RequestId = h.StockOutId,
                        DocCode = h.DocCode,
                        DocDate = h.DocDate,
                        BranchName = h.Branch.Name,
                        Status = h.Status.ToString(),
                        TotalValue = h.TotalValue
                    }).ToListAsync();
                return Json(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading pending stock out requests");
                return Json(new { error = "حدث خطأ" });
            }
        }

        // POST: StockOutRequest/ReadAllSellOrders
        [HttpPost]
        public async Task<IActionResult> ReadAllSellOrders()
        {
            try
            {
                // Logic to fetch sell orders that are approved but not yet fully spent
                var sellOrders = await _context.Orders
                    .Where(o => o.OrderTypeId == 1 && o.Status == "Approved")
                    .Select(o => new OrderViewModel
                    {
                        OrderId = o.OrderId,
                        OrderCode = o.OrderCode,
                        SupplierName = o.Supplier.Name,
                        RequestedDate = o.CreatedAt,
                        TotalQuantityBase = o.OrderLines.Sum(l => l.QtyOrdered)
                    }).ToListAsync();
                return Json(sellOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading available sell orders");
                return Json(new { error = "حدث خطأ" });
            }
        }

        // GET: StockOutRequest/Save
        [HttpGet]
        public async Task<IActionResult> Save(int RequestId = 0, int SoId = 0, bool IsSellOrder = false, bool IsTransferApprove = false)
        {
            try
            {
                var vm = new StockOutVm
                {
                    RequestId = RequestId,
                    SoId = SoId,
                    IsSellOrder = IsSellOrder,
                    IsTransferApprove = IsTransferApprove,
                    DocDate = DateTime.Today
                };

                if (RequestId > 0)
                {
                    var header = await _context.StockOutHeaders
                        .Include(h => h.StockOutDetails)
                        .FirstOrDefaultAsync(h => h.StockOutId == RequestId);
                    
                    if (header != null)
                    {
                        vm.BranchId = header.BranchId;
                        vm.DocDate = header.DocDate;
                        vm.Notes = header.Remarks;
                        vm.TransactionTypeId = 1; // Direct
                        vm.StockOutDetailRequests = header.StockOutDetails.Select(d => new StockOutDetailRequestDto
                        {
                            Id = d.StockOutDetailId,
                            ItemId = d.ItemId,
                            Quantity = d.Qty,
                            Price = d.Price,
                            NetValue = d.TotalValue
                        }).ToList();
                    }
                }
                else if (SoId > 0 && IsSellOrder)
                {
                    var order = await _context.Orders.Include(o => o.OrderLines).FirstOrDefaultAsync(o => o.OrderId == SoId);
                    if (order != null)
                    {
                        vm.ClientId = order.SupplierId; // Assuming Supplier is used for client here
                        vm.TransactionTypeId = 6; // Sell
                        vm.StockOutDetailRequests = order.OrderLines.Select(l => new StockOutDetailRequestDto
                        {
                            ItemId = l.ItemId,
                            Quantity = l.QtyOrdered,
                            Price = l.UnitPrice,
                            NetValue = l.QtyOrdered * l.UnitPrice
                        }).ToList();
                    }
                }

                vm.Branches = await _context.Branches.Where(b => b.IsActive).Select(b => new Core.ViewModels.StockOutRequestViewModels.BranchDto
                {
                    BranchId = b.BranchId,
                    BranchName = b.Name
                }).ToListAsync();

                vm.Items = await _context.Items.Where(i => !i.IsDeleted).Select(i => new ItemDto
                {
                    ItemId = i.ItemId,
                    ItemName = i.NameArab
                }).ToListAsync();

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading save form");
                return Content("Error loading form: " + ex.Message);
            }
        }

        // POST: StockOutRequest/Save
        [HttpPost]
        public async Task<IActionResult> Save([FromForm] StockOutVm stocksOutVm)
        {
            try
            {
                if (!ModelState.IsValid) return Json(new { success = false, message = "بيانات غير صالحة" });

                // Business Logic for saving StockOutHeader and Details
                // Placeholder for actual service call
                
                return Json(new { success = true, message = "تم حفظ إذن الصرف بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving stock out");
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // AJAX Helpers
        [HttpPost]
        public async Task<IActionResult> GetItemOpenBalanceIndex(int itemId, int branchId)
        {
            // Placeholder logic to return current item balance
            return Json(new { balance = 100 });
        }

        [HttpPost]
        public async Task<IActionResult> GetClientName(string term)
        {
            var clients = await _context.Suppliers
                .Where(s => s.Name.Contains(term))
                .Select(s => new { id = s.SupplierId, text = s.Name })
                .ToListAsync();
            return Json(clients);
        }

        [HttpPost]
        public async Task<IActionResult> GetToBranches(int branchId)
        {
            var branches = await _context.Branches
                .Where(b => b.BranchId != branchId && b.IsActive)
                .Select(b => new { id = b.BranchId, text = b.Name })
                .ToListAsync();
            return Json(branches);
        }

        [HttpPost]
        public async Task<IActionResult> ReadCoDetail(int sellOrderId)
        {
            var details = await _context.OrderLines
                .Where(l => l.OrderId == sellOrderId)
                .Select(l => new StockOutDetailRequestDto
                {
                    ItemId = l.ItemId,
                    Quantity = l.QtyOrdered,
                    Price = l.UnitPrice,
                    NetValue = l.QtyOrdered * l.UnitPrice
                }).ToListAsync();
            return Json(details);
        }
    }
}