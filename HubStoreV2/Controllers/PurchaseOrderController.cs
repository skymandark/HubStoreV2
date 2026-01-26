using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;

using Core.ViewModels.PurchaseOrderViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubStoreV2.Controllers
{
    //[Authorize]
    public class PurchaseOrderController : BaseController
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<PurchaseOrderController> _logger;

        public PurchaseOrderController(
            IPurchaseOrderService purchaseOrderService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<PurchaseOrderController> logger)
        {
            _purchaseOrderService = purchaseOrderService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: PurchaseOrder/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var purchaseOrders = await _purchaseOrderService.GetPurchaseOrders();
                return View(purchaseOrders ?? new List<PurchaseOrderListDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading purchase orders");
                TempData["Error"] = "حدث خطأ عند تحميل طلبات الشراء";
                return View(new List<PurchaseOrderListDto>());
            }
        }

        // GET: PurchaseOrder/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dto = new PurchaseOrderRequestDto
            {
                DocDate = DateTime.Today,
                Suppliers = new List<SupplierDto>(),
                Branches = new List<BranchDto>(),
                Items = new List<ItemDto>(),
                ItemPackages = new List<ItemUnitDto>(),
                ApprovalSystemOptions = new List<ApprovalSystemOptionDto>(),
                PurchaseOrderDetails = new List<PurchaseOrderDetailDto>()
            };

            try
            {
                var suppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
                var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
                // Load all items - filtering will be done on client side
                var items = await _context.Items
                    .Where(i => !i.IsDeleted)
                    .ToListAsync();

                dto.Suppliers = suppliers.Select(s => new SupplierDto
                {
                    SupplierId = s.SupplierId,
                    Name = s.Name
                }).ToList();
                dto.Branches = branches.Select(b => new BranchDto
                {
                    BranchId = b.BranchId,
                    Name = b.Name
                }).ToList();
                dto.Items = items.Select(i => new ItemDto
                {
                    ItemId = i.ItemId,
                    Name = i.NameArab,
                    NameArab = i.NameArab,
                    Code = i.ItemCode,
                    BranchId = i.BranchId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data for create form, using empty lists");
                // Continue with empty lists
            }

            return View(dto);
        }

        // POST: PurchaseOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseOrderRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadFormData(dto);
                    return View(dto);
                }

                var result = await _purchaseOrderService.CreatePurchaseOrder(dto);

                if (result > 0)
                {
                    TempData["Success"] = "تم إنشاء طلب الشراء بنجاح";
                    return RedirectToAction("Details", new { id = result });
                }

                TempData["Error"] = "فشل في إنشاء طلب الشراء";
                await LoadFormData(dto);
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating purchase order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                await LoadFormData(dto);
                return View(dto);
            }
        }

        // GET: PurchaseOrder/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var purchaseOrder = await _purchaseOrderService.GetPurchaseOrder(id);

                if (purchaseOrder == null)
                {
                    TempData["Error"] = "طلب الشراء غير موجود";
                    return RedirectToAction("Index");
                }

                // Prevent editing if approved
                if (purchaseOrder.Status == "Approved")
                {
                    TempData["Error"] = "لا يمكن تعديل طلب الشراء بعد الموافقة عليه";
                    return RedirectToAction("Details", new { id });
                }

                await LoadFormData(purchaseOrder);
                return View("Create", purchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form");
                TempData["Error"] = "حدث خطأ عند تحميل نموذج التعديل";
                return RedirectToAction("Index");
            }
        }

        // POST: PurchaseOrder/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PurchaseOrderRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadFormData(dto);
                    return View("Create", dto);
                }

                await _purchaseOrderService.UpdatePurchaseOrder(dto);

                TempData["Success"] = "تم تحديث طلب الشراء بنجاح";
                return RedirectToAction("Details", new { id = dto.PurchaseOrderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating purchase order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                await LoadFormData(dto);
                return View("Create", dto);
            }
        }

        // GET: PurchaseOrder/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var dto = await _purchaseOrderService.GetPurchaseOrder(id);

                if (dto == null)
                {
                    TempData["Error"] = "طلب الشراء غير موجود";
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

        // POST: PurchaseOrder/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _purchaseOrderService.DeletePurchaseOrder(id);
                TempData["Success"] = "تم حذف طلب الشراء بنجاح";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting purchase order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: PurchaseOrder/SubmitForApproval/{id}
        [HttpPost]
        public async Task<IActionResult> SubmitForApproval(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _purchaseOrderService.SubmitForApproval(id, user?.UserName ?? "System");

                TempData["Success"] = "تم إرسال طلب الشراء للموافقة";

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting for approval");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        // POST: PurchaseOrder/Approve/{id}
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _purchaseOrderService.ApprovePurchaseOrder(id, user?.UserName ?? "System");

                TempData["Success"] = "تم الموافقة على طلب الشراء";

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving purchase order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        // Helper method to load form data
        private async Task LoadFormData(PurchaseOrderRequestDto dto)
        {
            var suppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
            var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
            // Load all items - filtering will be done on client side
            var items = await _context.Items
                .Where(i => !i.IsDeleted)
                .ToListAsync();

            dto.Suppliers = suppliers.Select(s => new SupplierDto 
            { 
                SupplierId = s.SupplierId, 
                Name = s.Name 
            }).ToList();
            
            dto.Branches = branches.Select(b => new BranchDto 
            { 
                BranchId = b.BranchId, 
                Name = b.Name 
            }).ToList();
            
            dto.Items = items.Select(i => new ItemDto 
            { 
                ItemId = i.ItemId, 
                Name = i.NameArab,
                NameArab = i.NameArab,
                Code = i.ItemCode,
                BranchId = i.BranchId
            }).ToList();
            
            // Load approval system options
            dto.ApprovalSystemOptions = new List<ApprovalSystemOptionDto>
            {
                new ApprovalSystemOptionDto 
                { 
                    StatusId = 1, 
                    Name = "موافقة تلقائية", 
                    Description = "الموافقة التلقائية على طلب الشراء بدون الحاجة لموافقة يدوية",
                    RequiresApproval = false
                },
                new ApprovalSystemOptionDto 
                { 
                    StatusId = 2, 
                    Name = "موافقة يدوية", 
                    Description = "يتطلب موافقة يدوية من المدير المختص",
                    RequiresApproval = true
                },
                new ApprovalSystemOptionDto 
                { 
                    StatusId = 3, 
                    Name = "موافقة مشروطة", 
                    Description = "موافقة تلقائية للطلبات الأقل من مبلغ معين، وموافقة يدوية للطلبات الأكبر",
                    RequiresApproval = true
                }
            };
        }
    }
}
