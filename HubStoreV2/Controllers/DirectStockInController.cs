using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.OrderServices;

using Core.ViewModels.DirectStockInViewModels;
using Core.ViewModels.StockInViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HubStoreV2.Controllers
{
    public class DirectStockInController : Controller
    {
        private readonly IStockInService _stockInService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<DirectStockInController> _logger;

        public DirectStockInController(
            IStockInService stockInService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<DirectStockInController> logger)
        {
            _stockInService = stockInService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: DirectStockIn/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dto = new DirectStockInRequestDto
            {
                DocDate = DateTime.Today,
                Suppliers = new List<SupplierDto>(),
                Branches = new List<BranchDto>(),
                Items = new List<ItemDto>(),
                StockInDetails = new List<DirectStockInDetailDto>()
            };

            try
            {
                var suppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
                var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
                var items = await _context.Items.Where(i => !i.IsDeleted).ToListAsync();

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
                    Name = i.NameArab
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data for direct stock in create form");
            }

            return View(dto);
        }

        // POST: DirectStockIn/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DirectStockInRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadFormData(dto);
                    return View(dto);
                }

                // Convert to StockInRequestDto format
                var stockInDto = new StockInRequestDto
                {
                    DocDate = dto.DocDate,
                    BranchId = dto.BranchId,
                    SupplierId = dto.SupplierId,
                    InvoiceNo = dto.InvoiceNo,
                    Remarks = dto.Remarks,
                    TransactionTypeId = 4, // Direct Stock In
                    StockInDetails = dto.StockInDetails.Select(d => new StockInDetailDto
                    {
                        ItemId = d.ItemId,
                        Qty = d.Qty,
                        Price = d.Price,
                        Discount = d.Discount,
                        TotalValue = d.TotalValue,
                        BatchNo = d.BatchNo,
                        ExpiryDate = d.ExpiryDate
                    }).ToList()
                };

                var user = await _userManager.GetUserAsync(User);
                var result = await _stockInService.CreateStockIn(stockInDto, user?.UserName ?? "System");

                if (result > 0)
                {
                    TempData["Success"] = "تم إنشاء الاستلام المباشر بنجاح";
                    return RedirectToAction("Details", "StockIn", new { id = result });
                }

                TempData["Error"] = "فشل في إنشاء الاستلام المباشر";
                await LoadFormData(dto);
                return View(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating direct stock in");
                TempData["Error"] = "حدث خطأ: " + ex.Message;
                await LoadFormData(dto);
                return View(dto);
            }
        }

        // GET: DirectStockIn/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return RedirectToAction("Details", "StockIn", new { id });
        }

        // Helper method to load form data
        private async Task LoadFormData(DirectStockInRequestDto dto)
        {
            var suppliers = await _context.Suppliers.Where(s => s.IsActive).ToListAsync();
            var branches = await _context.Branches.Where(b => b.IsActive).ToListAsync();
            var items = await _context.Items.Where(i => !i.IsDeleted).ToListAsync();

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
                Name = i.NameArab
            }).ToList();
        }
    }
}