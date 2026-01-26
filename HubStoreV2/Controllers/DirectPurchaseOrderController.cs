using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.DirectPurchaseOrderViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubStoreV2.Controllers
{
    //[Authorize]
    public class DirectPurchaseOrderController : BaseController
    {
        private readonly IDirectPurchaseOrderService _directPurchaseOrderService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<DirectPurchaseOrderController> _logger;

        public DirectPurchaseOrderController(
            IDirectPurchaseOrderService directPurchaseOrderService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<DirectPurchaseOrderController> _logger)
        {
            _directPurchaseOrderService = directPurchaseOrderService;
            _context = context;
            _userManager = userManager;
            this._logger = _logger;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _directPurchaseOrderService.GetDirectPurchaseOrders();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateDirectPurchaseOrderViewModel
            {
                DocDate = DateTime.Today,
                DeliveryDate = DateTime.Today.AddDays(7),
                CreditPayment = true
            };

            await LoadFormData();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDirectPurchaseOrderViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadFormData();
                    return View(viewModel);
                }

                var user = await _userManager.GetUserAsync(User);
                var result = await _directPurchaseOrderService.CreateDirectPurchaseOrder(viewModel, user?.UserName ?? "System");

                if (result > 0)
                {
                    TempData["Success"] = "تم إنشاء طلب الشراء المباشر بنجاح";
                    return RedirectToAction("Details", new { id = result });
                }

                TempData["Error"] = "فشل في إنشاء طلب الشراء المباشر";
                await LoadFormData();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating direct purchase order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                await LoadFormData();
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _directPurchaseOrderService.GetDirectPurchaseOrder(id);
            if (order == null)
            {
                TempData["Error"] = "طلب الشراء غير موجود";
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _directPurchaseOrderService.ApproveDirectPurchaseOrder(id, user?.UserName ?? "System");
                TempData["Success"] = "تمت الموافقة على طلب الشراء المباشر";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ: " + ex.Message;
            }
            return RedirectToAction("Details", new { id });
        }

        private async Task LoadFormData()
        {
            var suppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
            var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
            // Load all items - filtering will be done on client side
            var items = await _context.Items
                .Where(i => !i.IsDeleted)
                .ToListAsync();
            var shipmentTypes = await _context.ShipmentTypes.ToListAsync();

            ViewBag.Suppliers = suppliers.Select(s => new { s.SupplierId, s.Name }).ToList();
            ViewBag.Branches = branches.Select(b => new { b.BranchId, b.Name }).ToList();
            ViewBag.Items = items.Select(i => new { i.ItemId, i.NameArab }).ToList();
            ViewBag.ShipmentTypes = shipmentTypes.Select(t => new { t.ShipmentTypeId, t.Name }).ToList();
        }
    }
}
