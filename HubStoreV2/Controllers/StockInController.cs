using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;

using Core.ViewModels.StockInViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubStoreV2.Controllers
{
    //[Authorize]
    public class StockInController : Controller
    {
        private readonly IStockInService _stockInService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<StockInController> _logger;

        public StockInController(
            IStockInService stockInService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<StockInController> logger)
        {
            _stockInService = stockInService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: StockIn/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var stockIns = await _stockInService.GetStockIns();
                return View(stockIns ?? new List<StockInListDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading stock ins");
                TempData["Error"] = "حدث خطأ عند تحميل إذن الاستلام";
                return View(new List<StockInListDto>());
            }
        }

        // GET: StockIn/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? purchaseOrderId)
        {
            var dto = new StockInRequestDto
            {
                DocDate = DateTime.Today,
                StockInDetails = new List<StockInDetailDto>()
            };

            try
            {
                var suppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
                var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
                var items = await _context.Items.Where(i => !i.IsDeleted).ToListAsync();

                if (purchaseOrderId.HasValue)
                {
                    var po = await _context.PurchaseOrderHeaders
                        .Include(p => p.PurchaseOrderDetails)
                        .FirstOrDefaultAsync(p => p.PurchaseOrderId == purchaseOrderId.Value);

                    if (po != null)
                    {
                        dto.PurchaseOrderId = po.PurchaseOrderId;
                        dto.SupplierId = po.SupplierId;
                        dto.BranchId = po.BranchId;
                        dto.Remarks = "Created from PO #" + po.PurchaseOrderCode;
                        dto.TransactionTypeId = 1; // Purchase

                        foreach (var pd in po.PurchaseOrderDetails)
                        {
                            dto.StockInDetails.Add(new StockInDetailDto
                            {
                                ItemId = pd.ItemId,
                                Qty = pd.OrderedQuantity - pd.ReceivedQuantity,
                                Price = pd.Price,
                                TotalValue = (pd.OrderedQuantity - pd.ReceivedQuantity) * pd.Price
                            });
                        }
                    }
                }

                ViewBag.Suppliers = suppliers.Select(s => new { s.SupplierId, s.Name }).ToList();
                ViewBag.Branches = branches.Select(b => new { b.BranchId, b.Name }).ToList();
                ViewBag.Items = items.Select(i => new { i.ItemId, i.NameArab }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data for create form");
            }

            return View(dto);
        }

        // POST: StockIn/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockInRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadFormData(dto);
                    return View(dto);
                }

                var user = await _userManager.GetUserAsync(User);
                var result = await _stockInService.CreateStockIn(dto, user?.UserName ?? "System");

                if (result > 0)
                {
                    TempData["Success"] = "تم إنشاء إذن الاستلام بنجاح";
                    return RedirectToAction("Details", new { id = result });
                }

                TempData["Error"] = "فشل في إنشاء إذن الاستلام";
                await LoadFormData(dto);
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stock in");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                await LoadFormData(dto);
                return View(dto);
            }
        }

        // GET: StockIn/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // For now, we'll get the list and find the item
                // TODO: Implement GetStockIn method in service
                var stockIns = await _stockInService.GetStockIns();
                var stockIn = stockIns.FirstOrDefault(si => si.StockInId == id);

                if (stockIn == null)
                {
                    TempData["Error"] = "إذن الاستلام غير موجود";
                    return RedirectToAction("Index");
                }

                return View(stockIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading details");
                TempData["Error"] = "حدث خطأ عند تحميل التفاصيل";
                return RedirectToAction("Index");
            }
        }

        // POST: StockIn/Approve/{id}
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _stockInService.ApproveStockIn(id, user?.UserName ?? "System");

                TempData["Success"] = "تم الموافقة على إذن الاستلام";

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving stock in");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        // POST: StockIn/Receive/{id}
        [HttpPost]
        public async Task<IActionResult> Receive(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _stockInService.ReceiveStockIn(id, user?.UserName ?? "System");

                TempData["Success"] = "تم استلام البضائع";

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error receiving stock in");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        // GET: StockIn/GetPurchaseOrders
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            try
            {
                var rawPos = await _context.PurchaseOrderHeaders
                    .Where(p => !p.IsDeleted && p.Status == PurchaseOrderStatus.Approved)
                    .Include(p => p.Supplier)
                    .ToListAsync();

                var pos = rawPos.Select(p => new
                    {
                        purchaseOrderId = p.PurchaseOrderId,
                        purchaseOrderCode = p.PurchaseOrderCode,
                        supplierName = p.Supplier.Name,
                        docDate = p.DocDate,
                        status = p.Status.ToString()
                    })
                    .ToList();

                return Json(pos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting purchase orders");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: StockIn/GetPurchaseOrderDetails/{purchaseOrderId}
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrderDetails(int purchaseOrderId)
        {
            try
            {
                var po = await _context.PurchaseOrderHeaders
                    .Include(p => p.PurchaseOrderDetails)
                        .ThenInclude(pd => pd.Item)
                    .Include(p => p.Supplier)
                    .Include(p => p.Branch)
                    .FirstOrDefaultAsync(p => p.PurchaseOrderId == purchaseOrderId);

                if (po == null)
                    return Json(new { success = false, message = "Purchase order not found" });

                var poData = new
                {
                    purchaseOrderId = po.PurchaseOrderId,
                    supplierName = po.Supplier.Name,
                    branchName = po.Branch.Name,
                    docDate = po.DocDate,
                    status = "Approved"
                };

                var details = po.PurchaseOrderDetails.Select(pd => new
                {
                    ItemId = pd.ItemId,
                    ItemName = pd.Item.NameArab,
                    OrderedQty = pd.OrderedQuantity,
                    ReceivedQty = pd.ReceivedQuantity,
                    RemainingQty = pd.OrderedQuantity - pd.ReceivedQuantity,
                    Price = pd.Price,
                    PurchaseOrderDetailId = pd.PurchaseOrderDetailId
                }).Where(d => d.RemainingQty > 0).ToList();

                return Json(new { success = true, po = poData, items = details });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting purchase order details");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Helper method to load form data
        private async Task LoadFormData(StockInRequestDto dto)
        {
            var suppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
            var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
            var items = await _context.Items.Where(i => !i.IsDeleted).ToListAsync();

            ViewBag.Suppliers = suppliers.Select(s => new { s.SupplierId, s.Name }).ToList();
            ViewBag.Branches = branches.Select(b => new { b.BranchId, b.Name }).ToList();
            ViewBag.Items = items.Select(i => new { i.ItemId, i.NameArab }).ToList();
        }
    }
}