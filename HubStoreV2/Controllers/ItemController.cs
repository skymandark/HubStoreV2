using Core.Domin;
using Core.Services.ItemServices;
using Core.ViewModels;
using Core.ViewModels.ItemViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HubStoreV2.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ItemController> _logger;

        public ItemController(
            IItemService itemService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // For the elegant grid, we'll fetch a reasonable amount or implement pagination in the grid
                var result = await _itemService.GetItemsAsync(new ItemFilterViewModel(), new PaginationViewModel { PageNumber = 1, PageSize = 1000 });
                return View(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading items");
                return View(new System.Collections.Generic.List<ItemViewModel>());
            }
        }

        public async Task<IActionResult> Create()
        {
            await LoadLookups();
            return View(new CreateItemViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateItemViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookups();
                return View(dto);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                dto.CreatedBy = user?.UserName ?? "System";
                await _itemService.CreateItemAsync(dto);
                TempData["Success"] = "تم إضافة الصنف بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating item");
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات: " + ex.Message);
                await LoadLookups();
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _itemService.GetItemAsync(id);
                if (item == null) return NotFound();

                var updateDto = new UpdateItemViewModel
                {
                    NameArab = item.NameArab,
                    NameEng = item.NameEng,
                    SectionId = item.SectionId,
                    MainItemId = item.MainItemId,
                    SubItemId = item.SubItemId,
                    BrandId = item.BrandId,
                    ItemTypeId = item.ItemTypeId,
                    BuyMainPackageId = item.BuyMainPackageId,
                    PackageCount = item.PackageCount,
                    BuySubPackageCount = item.BuySubPackageCount,
                    MainUnitCode = item.MainUnitCode,
                    BaseUnitCode = item.BaseUnitCode,
                    ExternalBarcode = item.ExternalBarcode,
                    InternalBarcode = item.InternalBarcode,
                    IsActive = item.IsActive,
                    Notes = item.Notes
                };
                ViewBag.ItemId = id;
                ViewBag.ItemCode = item.ItemCode;
                await LoadLookups();
                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading item for edit");
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateItemViewModel dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ItemId = id;
                await LoadLookups();
                return View(dto);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                dto.ModifiedBy = user?.UserName ?? "System";
                await _itemService.UpdateItemAsync(id, dto);
                TempData["Success"] = "تم تحديث بيانات الصنف بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating item");
                ModelState.AddModelError("", "حدث خطأ أثناء التحديث: " + ex.Message);
                ViewBag.ItemId = id;
                await LoadLookups();
                return View(dto);
            }
        }

        private async Task LoadLookups()
        {
            ViewBag.Sections = await _context.Sections.Where(s => !s.IsDeleted && s.IsActive).Select(s => new { s.SectionId, s.NameArab }).ToListAsync();
            ViewBag.MainItems = await _context.MainItems.Where(s => !s.IsDeleted && s.IsActive).Select(s => new { s.MainItemId, s.SectionId, s.NameArab }).ToListAsync();
            ViewBag.SubItems = await _context.SubItems.Where(s => !s.IsDeleted && s.IsActive).Select(s => new { s.SubItemId, s.MainItemId, s.NameArab }).ToListAsync();
            ViewBag.Brands = await _context.Brands.Where(s => !s.IsDeleted && s.IsActive).Select(s => new { s.BrandId, s.NameArab }).ToListAsync();
            ViewBag.ItemTypes = await _context.ItemTypes.Where(s => !s.IsDeleted && s.IsActive).Select(s => new { s.ItemTypeId, s.NameArab }).ToListAsync();
            ViewBag.Packages = await _context.Packages.Where(s => !s.IsDeleted && s.IsActive).Select(s => new { s.PackageId, s.NameArab }).ToListAsync();
        }
    }
}
