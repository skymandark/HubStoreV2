using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.SupplierViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HubStoreV2.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(
            ISupplierService supplierService,
            UserManager<AppUser> userManager,
            ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var suppliers = await _supplierService.GetSuppliers();
                return View(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading suppliers");
                return View(new System.Collections.Generic.List<SupplierListDto>());
            }
        }

        public IActionResult Create()
        {
            return View(new SupplierRequestDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierRequestDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _supplierService.CreateSupplier(dto, user?.UserName ?? "System");
                TempData["Success"] = "تم إضافة المورد بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating supplier");
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات");
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetSupplier(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupplierRequestDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _supplierService.UpdateSupplier(dto, user?.UserName ?? "System");
                TempData["Success"] = "تم تحديث بيانات المورد بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating supplier");
                ModelState.AddModelError("", "حدث خطأ أثناء التحديث");
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _supplierService.ToggleSupplierStatus(id, user?.UserName ?? "System");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
