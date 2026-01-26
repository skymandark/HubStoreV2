using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.InventoryServices;
using System;

namespace HubStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemBalanceService _itemBalanceService;

        public BranchesController(ApplicationDbContext context, IItemBalanceService itemBalanceService)
        {
            _context = context;
            _itemBalanceService = itemBalanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var branches = await _context.Branches
                .Where(b => b.IsActive)
                .Select(b => new
                {
                    b.BranchId,
                    nameArab = b.Name_Arab,
                    b.Name
                })
                .ToListAsync();

            return Ok(branches);
        }

        [HttpGet("items/{branchId}")]
        public async Task<IActionResult> GetItemsByBranch(int branchId)
        {
            var items = await _context.Items
                .Where(i => !i.IsDeleted && i.BranchId == branchId)
                .Select(i => new
                {
                    i.ItemId,
                    i.NameArab,
                    i.NameEng,
                    i.ItemCode
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("items/{branchId}/{itemId}")]
        public async Task<IActionResult> GetItemDetailsForBranch(int branchId, int itemId)
        {
            var item = await _context.Items
                .Where(i => i.ItemId == itemId && !i.IsDeleted)
                .Select(i => new
                {
                    i.ItemId,
                    i.ItemCode,
                    i.NameArab,
                    i.NameEng,
                    i.Cost,
                    i.UnitPrice,
                    i.CustomerPrice
                })
                .FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound(new { message = "Item not found" });
            }

            var balance = await _itemBalanceService.GetItemBalanceSummary(itemId, branchId, DateTime.UtcNow);

            return Ok(new
            {
                item.ItemId,
                item.ItemCode,
                item.NameArab,
                item.NameEng,
                price = item.UnitPrice ?? item.CustomerPrice ?? item.Cost ?? 0m,
                stockBalance = balance.ClosingBalance
            });
        }
    }
}