using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.ReturnOrderViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubStoreV2.Controllers
{
    //[Authorize]
    public class ReturnOrderController : Controller
    {
        private readonly IReturnOrderService _returnOrderService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ReturnOrderController> _logger;

        public ReturnOrderController(
            IReturnOrderService returnOrderService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<ReturnOrderController> logger)
        {
            _returnOrderService = returnOrderService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: ReturnOrder/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var returnOrders = await _returnOrderService.GetReturnOrders();
                return View(returnOrders ?? new List<ReturnOrderListDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading return orders");
                TempData["Error"] = "حدث خطأ عند تحميل أوامر الإرجاع";
                return View(new List<ReturnOrderListDto>());
            }
        }

        // GET: ReturnOrder/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dto = new ReturnOrderRequestDto
            {
                DocDate = DateTime.Today,
                ReturnOrderDetails = new List<ReturnOrderDetailDto>()
            };

            try
            {
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data for create form");
            }

            return View(dto);
        }

        // POST: ReturnOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReturnOrderRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                var user = await _userManager.GetUserAsync(User);
                var result = await _returnOrderService.CreateReturnOrder(dto, user?.UserName ?? "System");

                if (result > 0)
                {
                    TempData["Success"] = "تم إنشاء أمر الإرجاع بنجاح";
                    return RedirectToAction("Index");
                }

                TempData["Error"] = "فشل في إنشاء أمر الإرجاع";
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating return order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return View(dto);
            }
        }

        // GET: ReturnOrder/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // Assuming service has GetReturnOrder method, but interface doesn't have it
                // For now, placeholder
                TempData["Error"] = "تعديل أمر الإرجاع غير متاح حالياً";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form");
                TempData["Error"] = "حدث خطأ عند تحميل نموذج التعديل";
                return RedirectToAction("Index");
            }
        }

        // POST: ReturnOrder/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ReturnOrderRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", dto);
                }

                var user = await _userManager.GetUserAsync(User);
                await _returnOrderService.UpdateReturnOrder(dto, user?.UserName ?? "System");

                TempData["Success"] = "تم تحديث أمر الإرجاع بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating return order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return View("Create", dto);
            }
        }

        // POST: ReturnOrder/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Assuming service has DeleteReturnOrder, but interface doesn't have it
                TempData["Error"] = "حذف أمر الإرجاع غير متاح حالياً";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting return order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: ReturnOrder/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // Placeholder for details
                TempData["Error"] = "تفاصيل أمر الإرجاع غير متاحة حالياً";
                return RedirectToAction("Index");
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