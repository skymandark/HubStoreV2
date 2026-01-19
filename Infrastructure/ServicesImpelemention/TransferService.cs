using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.MovementServices;
using Core.ViewModels.MovementViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class TransferService : ITransferService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovementService _movementService;

        public TransferService(ApplicationDbContext context, IMovementService movementService)
        {
            _context = context;
            _movementService = movementService;
        }

        public async Task<MovementViewModel> CreateTransferOut(CreateTransferViewModel transferDto)
        {
            var conversionFactor = await GetConversionFactor(transferDto.ItemId, transferDto.UnitCode);
            var baseQuantity = transferDto.Quantity * conversionFactor;

            // التحقق من توفر الرصيد
            var isAvailable = await ValidateTransferAvailability(transferDto.ItemId, transferDto.BranchFromId, baseQuantity);
            if (!isAvailable)
                throw new Exception("الرصيد غير كافٍ للتحويل");

            // إنشاء حركة التحويل الخارجي
            var movementDto = new CreateMovementViewModel
            {
                MovementCode = await GenerateTransferCode("TO"),
                MovementTypeId = await GetMovementTypeIdByCode("TO"),
                BranchFromId = transferDto.BranchFromId,
                BranchToId = transferDto.BranchToId,
                Date = DateTime.UtcNow,
                InternalBarcode = Guid.NewGuid().ToString("N").Substring(0, 12),
                CreatedBy = transferDto.CreatedBy,
                Lines = new List<CreateMovementLineViewModel>
                {
                    new CreateMovementLineViewModel
                    {
                        ItemId = transferDto.ItemId,
                        BranchId = transferDto.BranchFromId,
                        UnitCode = transferDto.UnitCode,
                        QtyInput = transferDto.Quantity,
                        ConversionUsedToBase = conversionFactor,
                        UnitPrice = await GetItemCost(transferDto.ItemId),
                        CreatedBy = transferDto.CreatedBy
                    }
                }
            };

            return await _movementService.CreateMovement(movementDto);
        }

        public async Task<MovementViewModel> CreateTransferIn(CreateTransferViewModel transferDto)
        {
            var conversionFactor = await GetConversionFactor(transferDto.ItemId, transferDto.UnitCode);

            // إنشاء حركة التحويل الداخلي
            var movementDto = new CreateMovementViewModel
            {
                MovementCode = await GenerateTransferCode("TI"),
                MovementTypeId = await GetMovementTypeIdByCode("TI"),
                BranchFromId = transferDto.BranchFromId,
                BranchToId = transferDto.BranchToId,
                Date = DateTime.UtcNow,
                InternalBarcode = Guid.NewGuid().ToString("N").Substring(0, 12),
                CreatedBy = transferDto.CreatedBy,
                Lines = new List<CreateMovementLineViewModel>
                {
                    new CreateMovementLineViewModel
                    {
                        ItemId = transferDto.ItemId,
                        BranchId = transferDto.BranchToId,
                        UnitCode = transferDto.UnitCode,
                        QtyInput = transferDto.Quantity,
                        ConversionUsedToBase = conversionFactor,
                        UnitPrice = await GetItemCost(transferDto.ItemId),
                        CreatedBy = transferDto.CreatedBy
                    }
                }
            };

            return await _movementService.CreateMovement(movementDto);
        }

        public async Task<bool> ValidateTransferAvailability(int itemId, int branchFrom, decimal qtyBase)
        {
            try
            {
                // حساب الرصيد المتاح من OpeningBalances والحركات
                var openingBalance = await _context.OpeningBalances
                    .FirstOrDefaultAsync(ob => ob.ItemId == itemId && ob.BranchId == branchFrom);

                var totalIn = await _context.MovementLines
                    .Include(ml => ml.Movement)
                    .Where(ml => ml.ItemId == itemId &&
                                 ml.BranchId == branchFrom &&
                                 ml.Movement.Status == "Completed" &&
                                 ml.Movement.MovementType.Code == "TI")
                    .SumAsync(ml => ml.QtyBase);

                var totalOut = await _context.MovementLines
                    .Include(ml => ml.Movement)
                    .Where(ml => ml.ItemId == itemId &&
                                 ml.BranchId == branchFrom &&
                                 ml.Movement.Status == "Completed" &&
                                 ml.Movement.MovementType.Code == "TO")
                    .SumAsync(ml => ml.QtyBase);

                var availableQty = (openingBalance?.Quantity ?? 0) + totalIn - totalOut;

                return availableQty >= qtyBase;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CompleteTransfer(int transferOutId, int transferInId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var transferOut = await _context.Movements
                    .Include(m => m.MovementType)
                    .Include(m => m.MovementLines)
                    .FirstOrDefaultAsync(m => m.MovementId == transferOutId);

                var transferIn = await _context.Movements
                    .Include(m => m.MovementType)
                    .Include(m => m.MovementLines)
                    .FirstOrDefaultAsync(m => m.MovementId == transferInId);

                if (transferOut == null || transferIn == null)
                    throw new Exception("أحد الحركتين غير موجودة");

                if (transferOut.Status != "Approved")
                    throw new Exception("يجب الموافقة على التحويل الخارجي أولاً");

                if (transferIn.Status != "Pending")
                    throw new Exception("يجب أن يكون التحويل الداخلي في حالة انتظار");

                var outLine = transferOut.MovementLines.FirstOrDefault();
                var inLine = transferIn.MovementLines.FirstOrDefault();

                if (outLine == null || inLine == null)
                    throw new Exception("أحد الحركتين لا تحتوي على سطور");

                if (outLine.ItemId != inLine.ItemId || outLine.QtyBase != inLine.QtyBase)
                    throw new Exception("بيانات السطور غير متطابقة");

                // تحديث حالة الحركتين إلى مكتمل
                transferOut.Status = "Completed";
                transferIn.Status = "Completed";
                transferOut.CreatedAt = DateTime.UtcNow;
                transferIn.CreatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<decimal> GetConversionFactor(int itemId, string unitCode)
        {
            var itemUnit = await _context.ItemUnits
                .FirstOrDefaultAsync(iu => iu.ItemId == itemId && iu.UnitCode == unitCode);

            if (itemUnit == null)
                throw new Exception($"وحدة القياس {unitCode} غير موجودة للصنف {itemId}");

            return itemUnit.ConversionFactor ?? 1;
        }

        private async Task<decimal> GetItemCost(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            return item?.AverageCost ?? item?.Cost ?? 0;
        }

        private async Task<int> GetMovementTypeIdByCode(string movementTypeCode)
        {
            var movementType = await _context.MovementTypes
                .FirstOrDefaultAsync(mt => mt.Code == movementTypeCode);

            if (movementType == null)
                throw new Exception($"نوع الحركة {movementTypeCode} غير موجود");

            return movementType.MovementTypeId;
        }

        private async Task<string> GenerateTransferCode(string prefix)
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = await _context.Movements
                .CountAsync(m => m.MovementCode.StartsWith(prefix) &&
                                m.CreatedAt.Date == DateTime.Today) + 1;

            return $"{prefix}-{date}-{count:D4}";
        }
    }
}