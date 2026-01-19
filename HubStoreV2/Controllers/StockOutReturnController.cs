using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.StockOutReturnViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubStoreV2.Controllers
{
    //[Authorize]
    public class StockOutReturnController : Controller
    {
        private readonly IStockOutReturnService _stockOutReturnService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<StockOutReturnController> _logger;

        public StockOutReturnController(
            IStockOutReturnService stockOutReturnService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<StockOutReturnController> logger)
        {
            _stockOutReturnService = stockOutReturnService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: StockOutReturn/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var stockOutReturns = await _stockOutReturnService.GetStockOutReturns();
                return View(stockOutReturns ?? new List<StockOutReturnListDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading stock out returns");
                TempData["Error"] = "حدث خطأ عند تحميل صرف أوامر الإرجاع";
                return View(new List<StockOutReturnListDto>());
            }
        }

        // GET: StockOutReturn/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dto = new StockOutReturnRequestDto
            {
                DocDate = DateTime.Today,
                StockOutReturnDetails = new List<StockOutReturnDetailDto>()
            };

            try
            {
                // Load return orders for selection
                ViewBag.ReturnOrders = await _stockOutReturnService.GetReturnOrdersForStockOut();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data for create form");
            }

            return View(dto);
        }

        // POST: StockOutReturn/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockOutReturnRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ReturnOrders = await _stockOutReturnService.GetReturnOrdersForStockOut();
                    return View(dto);
                }

                var user = await _userManager.GetUserAsync(User);
                var result = await _stockOutReturnService.CreateStockOutReturn(dto, user?.UserName ?? "System");

                if (result > 0)
                {
                    TempData["Success"] = "تم إنشاء صرف من أمر الإرجاع بنجاح";
                    return RedirectToAction("Index");
                }

                TempData["Error"] = "فشل في إنشاء صرف من أمر الإرجاع";
                ViewBag.ReturnOrders = await _stockOutReturnService.GetReturnOrdersForStockOut();
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating stock out return");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                ViewBag.ReturnOrders = await _stockOutReturnService.GetReturnOrdersForStockOut();
                return View(dto);
            }
        }

        // GET: StockOutReturn/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var dto = await _stockOutReturnService.GetStockOutReturn(id);
                if (dto == null)
                {
                    TempData["Error"] = "لم يتم العثور على السجل";
                    return RedirectToAction("Index");
                }

                ViewBag.ReturnOrders = await _stockOutReturnService.GetReturnOrdersForStockOut();
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form");
                TempData["Error"] = "حدث خطأ عند تحميل نموذج التعديل";
                return RedirectToAction("Index");
            }
        }

        // POST: StockOutReturn/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(StockOutReturnRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ReturnOrders = await _stockOutReturnService.GetReturnOrdersForStockOut();
                    return View("Create", dto);
                }

                var user = await _userManager.GetUserAsync(User);
                await _stockOutReturnService.UpdateStockOutReturn(dto, user?.UserName ?? "System");

                TempData["Success"] = "تم تحديث صرف من أمر الإرجاع بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock out return");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                ViewBag.ReturnOrders = await _stockOutReturnService.GetReturnOrdersForStockOut();
                return View("Create", dto);
            }
        }

        // POST: StockOutReturn/Approve/{id}
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _stockOutReturnService.ApproveStockOutReturn(id, user?.UserName ?? "System");

                TempData["Success"] = "تم اعتماد صرف من أمر الإرجاع بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving stock out return");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: StockOutReturn/Execute/{id}
        [HttpPost]
        public async Task<IActionResult> Execute(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _stockOutReturnService.ExecuteStockOutReturn(id, user?.UserName ?? "System");

                TempData["Success"] = "تم تنفيذ صرف من أمر الإرجاع بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing stock out return");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: StockOutReturn/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var dto = await _stockOutReturnService.GetStockOutReturn(id);
                if (dto == null)
                {
                    TempData["Error"] = "لم يتم العثور على السجل";
                    return RedirectToAction("Index");
                }

                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading details");
                TempData["Error"] = "حدث خطأ عند تحميل التفاصيل";
                return RedirectToAction("Index");
            }
        }
    }
}