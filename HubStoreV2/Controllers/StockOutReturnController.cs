using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services;
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
        private readonly IBranchService _branchService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<StockOutReturnController> _logger;

        public StockOutReturnController(
            IStockOutReturnService stockOutReturnService,
            IBranchService branchService,
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<StockOutReturnController> logger)
        {
            _stockOutReturnService = stockOutReturnService;
            _branchService = branchService;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ReadDx()
        {
            var data = await _stockOutReturnService.GetStockOutReturns();
            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> Save(int? id)
        {
            var vm = new StockOutReturnVm();
            if (id.HasValue)
            {
                vm = await _stockOutReturnService.GetStockOutReturn(id.Value);
                if (vm == null) return NotFound();
            }
            else
            {
                vm.DocDate = DateTime.Today;
                vm.EntryDate = DateTime.Now;
            }

            var branches = await _branchService.GetAllBranches();
            vm.BranchList = branches.Select(b => new BranchDto { BranchId = b.BranchId, Name = b.Name }).ToList();
            
            // Example transaction types
            vm.TransactionTypeList = new List<TransactionTypeDto>
            {
                new TransactionTypeDto { TransactionTypeId = 6, Name = "أمر بيع" },
                new TransactionTypeDto { TransactionTypeId = 9, Name = "أمر إرجاع" }
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(StockOutReturnVm vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var branches = await _branchService.GetAllBranches();
                    vm.BranchList = branches.Select(b => new BranchDto { BranchId = b.BranchId, Name = b.Name }).ToList();
                    return View(vm);
                }

                var user = await _userManager.GetUserAsync(User);
                var userName = user?.UserName ?? "System";

                if (vm.RequestId == 0)
                {
                    await _stockOutReturnService.CreateStockOutReturn(vm, userName);
                }
                else
                {
                    await _stockOutReturnService.UpdateStockOutReturn(vm, userName);
                }

                return Json(new { success = true, message = "تم الحفظ بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> SubmitForApproval(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _stockOutReturnService.SubmitStockOutReturn(id, user?.UserName ?? "System");
                return Json(new { success = true, message = "تم إرسال الطلب للموافقة" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReadCo(int clientId, int branchId)
        {
            var orders = await _stockOutReturnService.GetSalesOrdersForClient(clientId, branchId);
            return Json(new { data = orders });
        }

        [HttpGet]
        public async Task<IActionResult> ReadSo(int orderId)
        {
            // Placeholder for order summary logic
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> ReadSoDetail(int orderId)
        {
            var details = await _stockOutReturnService.GetSalesOrderDetailsForReturn(orderId);
            return Json(new { data = details });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Implementation for deletion
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetClientName(string term)
        {
            // Autocomplete logic for clients
            var clients = await _context.Users // Or Client entity
                .Where(u => u.UserName.Contains(term))
                .Select(u => new { id = u.Id, name = u.UserName })
                .Take(10)
                .ToListAsync();
            return Json(clients);
        }
    }
}