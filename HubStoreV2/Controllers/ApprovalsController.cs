using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Domin;
using Core.ViewModels.ApprovalViewModels;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Core.Services.OrderServices;

namespace HubStoreV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApprovalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IStockOutReturnService _stockOutReturnService;

        public ApprovalsController(
            ApplicationDbContext context, 
            UserManager<AppUser> userManager,
            IPurchaseOrderService purchaseOrderService,
            IStockOutReturnService stockOutReturnService)
        {
            _context = context;
            _userManager = userManager;
            _purchaseOrderService = purchaseOrderService;
            _stockOutReturnService = stockOutReturnService;
        }

        public async Task<IActionResult> Index()
        {
            var approvalTypes = new List<ApprovalTypeVm>
            {
                new ApprovalTypeVm { ApproveDefId = 1, Name = "طلب شراء", Icon = "fa-shopping-cart", ControllerName = "PurchaseOrder" },
                new ApprovalTypeVm { ApproveDefId = 2, Name = "طلب استلام", Icon = "fa-download", ControllerName = "StockIn" },
                new ApprovalTypeVm { ApproveDefId = 8, Name = "مرتجع العميل", Icon = "fa-undo", ControllerName = "StockOutReturn" },
                new ApprovalTypeVm { ApproveDefId = 12, Name = "نقل مخزون", Icon = "fa-exchange-alt", ControllerName = "TransferOrder" }
            };

            // Get pending counts (simplified for now)
            foreach (var type in approvalTypes)
            {
                type.PendingCount = await GetPendingCount(type.ApproveDefId);
            }

            var vm = new ApprovalDashboardVm { ApprovalTypes = approvalTypes };
            return View(vm);
        }

        private async Task<int> GetPendingCount(int approveDefId)
        {
            switch (approveDefId)
            {
                case 1: // PurchaseOrder
                    return await _context.PurchaseOrderHeaders.CountAsync(h => h.Status == PurchaseOrderStatus.Submitted);
                case 8: // StockOutReturn
                    // Status 2 is PendingApproval in StockOutReturnHeader
                    return await _context.StockOutReturnHeaders.CountAsync(h => h.Status == 2); 
                default:
                    return 0;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStepData(int approveDefId)
        {
            var data = new List<ApprovalItemDto>();

            switch (approveDefId)
            {
                case 1: // PurchaseOrder
                    var pos = await _context.PurchaseOrderHeaders
                        .Include(h => h.Supplier)
                        .Include(h => h.Branch)
                        .Where(h => h.Status == PurchaseOrderStatus.Submitted)
                        .Select(h => new ApprovalItemDto
                        {
                            RequestId = h.PurchaseOrderId,
                            Reference = h.PurchaseOrderCode,
                            PartyName = h.Supplier.Name,
                            BranchName = h.Branch.Name,
                            DocDate = h.DocDate,
                            TotalValue = h.NetTotal,
                            Status = h.Status.ToString(),
                            ApproveDefId = 1
                        }).ToListAsync();
                    data.AddRange(pos);
                    break;

                case 8: // StockOutReturn
                    var returns = await _context.StockOutReturnHeaders
                        .Include(h => h.Branch)
                        .Where(h => h.Status == 2) // Status 2 is PendingApproval
                        .Select(h => new ApprovalItemDto
                        {
                            RequestId = h.StockOutReturnId,
                            Reference = h.DocCode,
                            PartyName = "العميل", // Placeholder or fetch from context
                            BranchName = h.Branch.Name,
                            DocDate = h.DocDate,
                            TotalValue = h.NetValue,
                            Status = "Pending",
                            ApproveDefId = 8
                        }).ToListAsync();
                    data.AddRange(returns);
                    break;
            }

            return Json(new { data });
        }

        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] BatchApprovalDto dto)
        {
            if (dto == null || dto.RequestIds == null || !dto.RequestIds.Any())
                return BadRequest("No items selected");

            var user = await _userManager.GetUserAsync(User);
            var userName = user?.UserName ?? "System";

            try
            {
                foreach (var id in dto.RequestIds)
                {
                    switch (dto.ApproveDefId)
                    {
                        case 1: // PurchaseOrder
                            if (dto.TargetStatusId == 1) // Approve
                                await _purchaseOrderService.ApprovePurchaseOrder(id, userName);
                            else if (dto.TargetStatusId == 2) // Reject
                                await _purchaseOrderService.RejectPurchaseOrder(id, userName, dto.Note);
                            break;

                        case 8: // StockOutReturn
                            if (dto.TargetStatusId == 1) // Approve
                                await _stockOutReturnService.ApproveStockOutReturn(id, userName);
                            else if (dto.TargetStatusId == 2) // Reject
                                await _stockOutReturnService.RejectStockOutReturn(id, userName, dto.Note);
                            break;
                    }
                }

                return Json(new { success = true, message = "تمت معالجة الطلبات بنجاح" });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetGridComponent(int typeId)
        {
            switch (typeId)
            {
                case 1:
                    return ViewComponent("POApproval", new { typeId = typeId });
                case 8:
                    return ViewComponent("StockOutReturnApproval", new { typeId = typeId });
                default:
                    return Content("Component not implemented");
            }
        }
    }
}
