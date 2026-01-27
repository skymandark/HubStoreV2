using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.InventoryServices;
using Core.ViewModels.InventoryViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.ServicesImpelemention
{
    public class ItemBalanceService : IItemBalanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ItemBalanceService> _logger;

        public ItemBalanceService(ApplicationDbContext context, ILogger<ItemBalanceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ItemTransactionViewModel>> ItemTransactionSheet(int itemId, int branchId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var transactions = new List<ItemTransactionViewModel>();

                // Get opening balance
                var openingBalance = await GetItemOpenBalance(itemId, branchId, fromDate);
                
                // Get all movements for the item in the specified period and branch
                var movements = await _context.MovementLines
                    .Where(ml => ml.ItemId == itemId && 
                           ml.BranchId == branchId &&
                           ml.Movement.Date >= fromDate && 
                           ml.Movement.Date <= toDate &&
                           !ml.Movement.IsDeleted &&
                           ml.Movement.ApprovalStatusId == 2)
                    .Include(ml => ml.Movement)
                        .ThenInclude(m => m.MovementType)
                    .Include(ml => ml.Item)
                    .Include(ml => ml.Branch)
                    .OrderBy(ml => ml.Movement.Date)
                    .ToListAsync();

                decimal runningBalance = openingBalance;

                foreach (var movement in movements)
                {
                    var transaction = new ItemTransactionViewModel
                    {
                        BaseId = movement.MovementId,
                        TransactionType = movement.Movement.MovementType.Name,
                        TransactionTypeId = movement.Movement.MovementTypeId,
                        DocDate = movement.Movement.Date,
                        Remarks = movement.Movement.Notes ?? movement.Movement.MovementType.Name,
                        InQuantity = movement.QtyInput > 0 ? movement.QtyInput : (decimal?)null,
                        OutQuantity = movement.QtyInput < 0 ? Math.Abs(movement.QtyInput) : (decimal?)null,
                        Price = movement.UnitPrice,
                        EntryDate = movement.Movement.CreatedDate,
                        UserData = movement.Movement.CreatedBy,
                        BranchId = movement.BranchId,
                        BranchName = movement.Branch.Name,
                        ItemPackageId = null, // MovementLine doesn't have ItemPackageId
                        ItemPackageName = "وحدة أساسية" // Default value
                    };

                    // Update running balance
                    runningBalance += movement.QtyInput;
                    transaction.Balance = runningBalance;
                    transaction.OpenBalance = openingBalance;

                    // Get supplier/client information if applicable
                    if (movement.Movement.SupplierId.HasValue)
                    {
                        transaction.SupplierClientId = movement.Movement.SupplierId;
                        // You might need to join with a suppliers table to get the name
                    }

                    transactions.Add(transaction);
                }

                _logger.LogInformation($"Retrieved {transactions.Count} transactions for item {itemId} in branch {branchId}");
                return transactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving item transactions for item {itemId}, branch {branchId}");
                throw new ApplicationException($"Error retrieving item transactions: {ex.Message}", ex);
            }
        }

        public async Task<decimal> GetItemOpenBalance(int itemId, int branchId, DateTime fromDate)
        {
            try
            {
                // Get opening balance from OpeningBalances table
                var fiscalYear = fromDate.Year;
                var openingBalance = await _context.OpeningBalances
                    .Where(ob => ob.ItemId == itemId && 
                           ob.BranchId == branchId && 
                           ob.FiscalYear == fiscalYear)
                    .FirstOrDefaultAsync();

                decimal balance = openingBalance?.OpeningQuantityBase ?? 0;

                // Calculate movements before the from date
                var previousMovements = await _context.MovementLines
                    .Where(ml => ml.ItemId == itemId && 
                           ml.BranchId == branchId &&
                           ml.Movement.Date < fromDate &&
                           !ml.Movement.IsDeleted &&
                           ml.Movement.ApprovalStatusId == 2)
                    .SumAsync(ml => ml.QtyInput);

                balance += previousMovements;

                _logger.LogInformation($"Opening balance for item {itemId} in branch {branchId} as of {fromDate}: {balance}");
                return balance;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calculating opening balance for item {itemId}, branch {branchId}");
                throw new ApplicationException($"Error calculating opening balance: {ex.Message}", ex);
            }
        }

        public async Task<ItemBalanceSummaryViewModel> GetItemBalanceSummary(int itemId, int branchId, DateTime asOfDate)
        {
            try
            {
                var item = await _context.Items.FindAsync(itemId);
                var branch = await _context.Branches.FindAsync(branchId);

                if (item == null)
                    throw new InvalidOperationException($"Item {itemId} not found");
                if (branch == null)
                    throw new InvalidOperationException($"Branch {branchId} not found");

                var openingBalance = await GetItemOpenBalance(itemId, branchId, asOfDate);

                // Get movements for the day
                var movements = await _context.MovementLines
                    .Where(ml => ml.ItemId == itemId && 
                           ml.BranchId == branchId &&
                           ml.Movement.Date.Date == asOfDate.Date &&
                           !ml.Movement.IsDeleted &&
                           ml.Movement.ApprovalStatusId == 2)
                    .ToListAsync();

                var totalIn = movements.Where(m => m.QtyInput > 0).Sum(m => m.QtyInput);
                var totalOut = Math.Abs(movements.Where(m => m.QtyInput < 0).Sum(m => m.QtyInput));

                var closingBalance = openingBalance + totalIn - totalOut;

                return new ItemBalanceSummaryViewModel
                {
                    ItemId = itemId,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    BranchId = branchId,
                    BranchName = branch.Name,
                    OpeningBalance = openingBalance,
                    TotalIn = totalIn,
                    TotalOut = totalOut,
                    ClosingBalance = closingBalance,
                    AsOfDate = asOfDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting balance summary for item {itemId}, branch {branchId}");
                throw new ApplicationException($"Error getting balance summary: {ex.Message}", ex);
            }
        }
    }
}
