using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;
using Core.ViewModels.TransferOrderViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubStoreV2.Controllers
{
    //[Authorize]
    public class TransferOrderController : BaseController
    {
        private readonly ITransferOrderServiceNew _transferOrderService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<TransferOrderController> _logger;

        public TransferOrderController(
            ITransferOrderServiceNew transferOrderService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<TransferOrderController> logger)
        {
            _transferOrderService = transferOrderService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: TransferOrderRequest/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var transferOrders = await GetTransferOrdersList();
                return View(transferOrders ?? new List<TransferOrderListDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading transfer order requests");
                TempData["Error"] = "حدث خطأ عند تحميل طلبات النقل";
                return View(new List<TransferOrderListDto>());
            }
        }

        // GET: TransferOrder/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dto = new TransferOrderRequestDto
            {
                DocDate = DateTime.Today,
                EntryDate = DateTime.Now,
                Branches = new List<BranchDto>(),
                ShipmentTypes = new List<ShipmentTypeDto>(),
                AvailableItems = new List<ItemSelectionDto>(),
                SelectedItems = new List<SelectedItemDto>(),
                TransferOrderDetails = new List<TransferOrderDetailDto>()
            };

            try
            {
                var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
                var shipmentTypes = await _context.ShipmentTypes.Where(s => s.IsActive).ToListAsync();
                // Load all items - filtering will be done on client side
                var items = await _context.Items
                    .Where(i => !i.IsDeleted)
                    .ToListAsync();

                dto.Branches = branches.Select(b => new BranchDto
                {
                    BranchId = b.BranchId,
                    Name = b.Name
                }).ToList();

                dto.ShipmentTypes = shipmentTypes.Select(s => new ShipmentTypeDto
                {
                    ShipmentTypeId = s.ShipmentTypeId,
                    Name = s.Name
                }).ToList();

                dto.AvailableItems = items.Select(i => new ItemSelectionDto
                {
                    ItemId = i.ItemId,
                    ItemNameEng = i.ItemName,
                    StockAccount = "Default", // TODO: Get actual stock account
                    QuantityOnWay = 0, // TODO: Calculate quantity on way
                    ConsumerPrice = 0, // TODO: Get actual consumer price
                    AvailableStock = 0, // TODO: Calculate available stock
                    Quantity = 0
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data for save form, using empty lists");
            }

            return View(dto);
        }

        // POST: TransferOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransferOrderRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadFormData(dto);
                    return View(dto);
                }

                // Convert SelectedItems to TransferOrderDetails
                if (dto.SelectedItems != null && dto.SelectedItems.Count > 0)
                {
                    dto.TransferOrderDetails = dto.SelectedItems.Select(item => new TransferOrderDetailDto
                    {
                        ItemId = item.ItemId,
                        RequestedQty = item.Quantity,
                        Notes = "",
                        TempOrder = item.TempOrder
                    }).ToList();
                }

                var result = await _transferOrderService.CreateTransferOrder(dto);

                if (result > 0)
                {
                    TempData["Success"] = "تم إنشاء طلب النقل بنجاح";
                    return RedirectToAction("Index");
                }

                TempData["Error"] = "فشل في إنشاء طلب النقل";
                await LoadFormData(dto);
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transfer order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                await LoadFormData(dto);
                return View(dto);
            }
        }

        // GET: TransferOrderRequest/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var transferOrder = await _transferOrderService.GetTransferOrder(id);

                if (transferOrder == null)
                {
                    TempData["Error"] = "طلب النقل غير موجود";
                    return RedirectToAction("Index");
                }

                await LoadFormData(transferOrder);
                return View("Create", transferOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form");
                TempData["Error"] = "حدث خطأ عند تحميل نموذج التعديل";
                return RedirectToAction("Index");
            }
        }

        // POST: TransferOrderRequest/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TransferOrderRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadFormData(dto);
                    return View("Create", dto);
                }

                // Convert SelectedItems to TransferOrderDetails
                if (dto.SelectedItems != null && dto.SelectedItems.Count > 0)
                {
                    dto.TransferOrderDetails = dto.SelectedItems.Select(item => new TransferOrderDetailDto
                    {
                        ItemId = item.ItemId,
                        RequestedQty = item.Quantity,
                        Notes = "",
                        TempOrder = item.TempOrder
                    }).ToList();
                }

                await _transferOrderService.UpdateTransferOrder(dto);

                TempData["Success"] = "تم تحديث طلب النقل بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transfer order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                await LoadFormData(dto);
                return View("Save", dto);
            }
        }

        // POST: TransferOrderRequest/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _transferOrderService.DeleteTransferOrder(id);
                TempData["Success"] = "تم حذف طلب النقل بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transfer order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: TransferOrderRequest/EditQuantity/{id}
        [HttpGet]
        public async Task<IActionResult> EditQuantity(int id)
        {
            try
            {
                var transferOrder = await _transferOrderService.GetTransferOrder(id);

                if (transferOrder == null)
                {
                    TempData["Error"] = "طلب النقل غير موجود";
                    return RedirectToAction("Index");
                }

                // Convert details to selected items format
                transferOrder.SelectedItems = transferOrder.TransferOrderDetails.Select(d => new SelectedItemDto
                {
                    ItemId = d.ItemId,
                    ItemNameEng = GetItemName(d.ItemId), // TODO: Implement
                    Quantity = d.RequestedQty,
                    TempOrder = d.TempOrder
                }).ToList();

                await LoadFormData(transferOrder);
                return View("Create", transferOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit quantity form");
                TempData["Error"] = "حدث خطأ عند تحميل نموذج تعديل الكميات";
                return RedirectToAction("Index");
            }
        }

        // POST: TransferOrderRequest/UpdateQuantities
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantities(int id, List<TransferOrderDetailDto> details)
        {
            try
            {
                await _transferOrderService.UpdateQuantities(id, details);
                TempData["Success"] = "تم تحديث الكميات بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quantities");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Helper method to load form data
        private async Task LoadFormData(TransferOrderRequestDto dto)
        {
            var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
            var shipmentTypes = await _context.ShipmentTypes.Where(s => s.IsActive).ToListAsync();
            // Load all items - filtering will be done on client side
            var items = await _context.Items
                .Where(i => !i.IsDeleted)
                .ToListAsync();

            dto.Branches = branches.Select(b => new BranchDto
            {
                BranchId = b.BranchId,
                Name = b.Name
            }).ToList();

            dto.ShipmentTypes = shipmentTypes.Select(s => new ShipmentTypeDto
            {
                ShipmentTypeId = s.ShipmentTypeId,
                Name = s.Name
            }).ToList();

            dto.AvailableItems = items.Select(i => new ItemSelectionDto
            {
                ItemId = i.ItemId,
                ItemNameEng = i.ItemName,
                StockAccount = "Default",
                QuantityOnWay = 0,
                ConsumerPrice = 0, // TODO: Get actual consumer price
                AvailableStock = 0,
                Quantity = 0
            }).ToList();
        }

        // Helper method to get transfer orders list with required fields
        private async Task<List<TransferOrderListDto>> GetTransferOrdersList()
        {
            var transferOrders = await _transferOrderService.GetTransferOrders();

            return transferOrders.Select(to => new TransferOrderListDto
            {
                RequestId = to.RequestId,
                Reference = to.Reference,
                ToBranchName = to.ToBranchName,
                FromBranchName = to.FromBranchName,
                ShipmentTypeName = to.ShipmentTypeName,
                EntryDate = to.EntryDate,
                DocDate = to.DocDate,
                StepName = to.StepName,
                StatusName = to.StatusName,
                Notes = to.Notes
            }).ToList();
        }

        // GET: TransferOrder/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var dto = await _transferOrderService.GetTransferOrder(id);

                if (dto == null)
                {
                    TempData["Error"] = "طلب النقل غير موجود";
                    return RedirectToAction("Index");
                }
                
                // Load branch names if not populated (though service does includes)
                // Service maps IDs but maybe names are needed? 
                // dto.TransferOrderDetails already has data.
                // We might need item names if they are not fully populated in DTO, but let's assume Service does it or View resolves it.
                // Update: The service mapping I added earlier didn't map ItemNameEng in details. I should fix that in Service or here.
                // Let's populate names here for safety or rely on View resolving them if possible. 
                // Better: Update Service to map Item Name. 
                // For now, let's load viewbag data.
                
                var fromBranch = await _context.Branches.FindAsync(dto.FromBranchId);
                var toBranch = await _context.Branches.FindAsync(dto.ToBranchId);
                var shipmentType = await _context.ShipmentTypes.FindAsync(dto.ShipmentTypeId);

                ViewBag.FromBranchName = fromBranch?.Name;
                ViewBag.ToBranchName = toBranch?.Name;
                ViewBag.ShipmentTypeName = shipmentType?.Name;
                
                // Populate Item Names (since DTO in service usually just has ID)
                // The service query included Item, but DTO mapping might have missed name.
                // Let's check Service mapping again... I missed mapping ItemName in the previous update.
                // It's okay, I can fix it by fetching names here or updating service. 
                // Updating service is cleaner but let's do it here for quickness or update service in next step if critical.
                // Actually, I'll fetch distinct items names here.
                var itemIds = dto.TransferOrderDetails.Select(d => d.ItemId).Distinct().ToList();
                var items = await _context.Items.Where(i => itemIds.Contains(i.ItemId)).ToDictionaryAsync(i => i.ItemId, i => i.NameArab);
                
                ViewBag.ItemNames = items;

                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading details");
                TempData["Error"] = "حدث خطأ عند تحميل التفاصيل";
                return RedirectToAction("Index");
            }
        }

        // POST: TransferOrder/Approve/{id}
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _transferOrderService.ApproveTransferOrder(id, user?.UserName ?? "System");
                TempData["Success"] = "تم اعتماد طلب النقل";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving transfer order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        // POST: TransferOrder/Execute/{id}
        [HttpPost]
        public async Task<IActionResult> Execute(int id, List<TransferOrderDetailDto> details)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                 // Filter out zero quantities
                var toShip = details.Where(d => d.RequestedQty > 0).ToList();
                
                if (!toShip.Any()) 
                {
                     TempData["Error"] = "يجب تحديد كميات للشحن";
                     return RedirectToAction("Details", new { id });
                }

                await _transferOrderService.ExecuteTransferOrder(id, toShip, user?.UserName ?? "System");
                TempData["Success"] = "تم تنفيذ إذن الصرف (شحن البضاعة)";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing transfer order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        // POST: TransferOrder/Receive/{id}
        [HttpPost]
        public async Task<IActionResult> Receive(int id, List<TransferOrderDetailDto> details)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                 // Filter out zero quantities
                var toReceive = details.Where(d => d.RequestedQty > 0).ToList();
                
                if (!toReceive.Any()) 
                {
                     TempData["Error"] = "يجب تحديد كميات للاستلام";
                     return RedirectToAction("Details", new { id });
                }

                await _transferOrderService.ReceiveTransferOrder(id, toReceive, user?.UserName ?? "System");
                TempData["Success"] = "تم استلام البضاعة";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error receiving transfer order");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        private string GetItemName(int itemId)
        {
            var item = _context.Items.Find(itemId);
            return item?.NameArab ?? "Unknown";
        }

        [HttpPost]
        public async Task<IActionResult> GetItemBalance(int itemId, int branchId)
        {
            // Placeholder: In a real system, this would query a balance table or inventory service
            // For now, returning a dummy value or querying context if possible
            return Json(new { balance = 150 }); 
        }
    }
}