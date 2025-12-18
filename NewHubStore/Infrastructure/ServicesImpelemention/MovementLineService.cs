using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.MovementServices;
using Core.ViewModels.MovementViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class MovementLineService : IMovementLineService
    {
        private readonly ApplicationDbContext _context;

        public MovementLineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MovementLineViewModel> AddMovementLine(int movementId, CreateMovementLineViewModel lineDto)
        {
            // التحقق من وجود الحركة
            var movement = await _context.Movements.FindAsync(movementId);
            if (movement == null)
                throw new Exception("الحركة غير موجودة");

            if (movement.Status != "Draft")
                throw new Exception("لا يمكن إضافة سطور إلا لحركة في حالة مسودة");

            // التحقق من صحة السطر
            var isValid = await ValidateMovementLine(lineDto);
            if (!isValid)
                throw new Exception("بيانات السطر غير صالحة");

            var line = new MovementLine
            {
                MovementId = movementId,
                ItemId = lineDto.ItemId,
                BranchId = lineDto.BranchId,
                UnitCode = lineDto.UnitCode,
                QtyInput = lineDto.QtyInput,
                ConversionUsedToBase = lineDto.ConversionUsedToBase,
                QtyBase = lineDto.QtyInput * lineDto.ConversionUsedToBase,
                UnitPrice = lineDto.UnitPrice,
                Notes = lineDto.Notes,
                CreatedBy = lineDto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                Status = "Draft"
            };

            _context.MovementLines.Add(line);
            await _context.SaveChangesAsync();

            // تحديث إجماليات الحركة
            await UpdateMovementTotals(movementId);

            return await GetMovementLineViewModel(line.MovementLineId);
        }

        public async Task<MovementLineViewModel> UpdateMovementLine(int lineId, UpdateMovementLineViewModel lineDto)
        {
            var line = await _context.MovementLines
                .Include(l => l.Movement)
                .FirstOrDefaultAsync(l => l.MovementLineId == lineId);

            if (line == null)
                throw new Exception("سطر الحركة غير موجود");

            if (line.Movement.Status != "Draft")
                throw new Exception("لا يمكن تعديل سطور حركة إلا إذا كانت الحركة في حالة مسودة");

            line.QtyInput = lineDto.QtyInput;
            line.ConversionUsedToBase = lineDto.ConversionUsedToBase;
            line.QtyBase = lineDto.QtyInput * lineDto.ConversionUsedToBase;
            line.UnitPrice = lineDto.UnitPrice;
            line.Notes = lineDto.Notes;
            line.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // تحديث إجماليات الحركة
            await UpdateMovementTotals(line.MovementId);

            return await GetMovementLineViewModel(lineId);
        }

        public async Task<bool> DeleteMovementLine(int lineId)
        {
            var line = await _context.MovementLines
                .Include(l => l.Movement)
                .FirstOrDefaultAsync(l => l.MovementLineId == lineId);

            if (line == null)
                throw new Exception("سطر الحركة غير موجود");

            if (line.Movement.Status != "Draft")
                throw new Exception("لا يمكن حذف سطور حركة إلا إذا كانت الحركة في حالة مسودة");

            _context.MovementLines.Remove(line);
            await _context.SaveChangesAsync();

            // تحديث إجماليات الحركة
            await UpdateMovementTotals(line.MovementId);

            return true;
        }

        public async Task<IEnumerable<MovementLineViewModel>> GetMovementLines(int movementId)
        {
            var lines = await _context.MovementLines
                .Include(l => l.Item)
                .Where(l => l.MovementId == movementId)
                .ToListAsync();

            return lines.Select(line => new MovementLineViewModel
            {
                MovementLineId = line.MovementLineId,
                MovementId = line.MovementId,
                ItemId = line.ItemId,
                ItemCode = line.Item?.ItemCode,
                BranchId = line.BranchId,
                UnitCode = line.UnitCode,
                QtyInput = line.QtyInput,
                ConversionUsedToBase = line.ConversionUsedToBase,
                QtyBase = line.QtyBase,
                UnitPrice = line.UnitPrice,
                Status = line.Status,
                ApprovedDate = line.ApprovedDate,
                Notes = line.Notes
            }).ToList();
        }

        public async Task<MovementLineTotalsViewModel> CalculateLineTotals(int lineId)
        {
            var line = await _context.MovementLines.FindAsync(lineId);
            if (line == null)
                throw new Exception("سطر الحركة غير موجود");

            return new MovementLineTotalsViewModel
            {
                MovementLineId = line.MovementLineId,
                QtyBase = line.QtyBase,
                UnitPrice = line.UnitPrice,
                TotalValue = line.QtyBase * line.UnitPrice
            };
        }

        public async Task<bool> ValidateMovementLine(CreateMovementLineViewModel lineDto)
        {
            // التحقق من وجود الصنف
            var item = await _context.Items.FindAsync(lineDto.ItemId);
            if (item == null)
                return false;

            // التحقق من وجود الفرع
            var branch = await _context.Branches.FindAsync(lineDto.BranchId);
            if (branch == null)
                return false;

            // التحقق من أن الكمية أكبر من صفر
            if (lineDto.QtyInput <= 0)
                return false;

            // التحقق من أن عامل التحويل أكبر من صفر
            if (lineDto.ConversionUsedToBase <= 0)
                return false;

            // التحقق من أن سعر الوحدة أكبر من صفر
            if (lineDto.UnitPrice <= 0)
                return false;

            return true;
        }

        private async Task<MovementLineViewModel> GetMovementLineViewModel(int lineId)
        {
            var line = await _context.MovementLines
                .Include(l => l.Item)
                .FirstOrDefaultAsync(l => l.MovementLineId == lineId);

            if (line == null)
                throw new Exception("سطر الحركة غير موجود");

            return new MovementLineViewModel
            {
                MovementLineId = line.MovementLineId,
                MovementId = line.MovementId,
                ItemId = line.ItemId,
                ItemCode = line.Item?.ItemCode,
                BranchId = line.BranchId,
                UnitCode = line.UnitCode,
                QtyInput = line.QtyInput,
                ConversionUsedToBase = line.ConversionUsedToBase,
                QtyBase = line.QtyBase,
                UnitPrice = line.UnitPrice,
                Status = line.Status,
                ApprovedDate = line.ApprovedDate,
                Notes = line.Notes
            };
        }

        private async Task UpdateMovementTotals(int movementId)
        {
            var lines = await _context.MovementLines
                .Where(l => l.MovementId == movementId)
                .ToListAsync();

            var movement = await _context.Movements.FindAsync(movementId);
            if (movement != null)
            {
                movement.TotalQuantityBase = lines.Sum(l => l.QtyBase);
                movement.TotalValueCost = lines.Sum(l => l.QtyBase * l.UnitPrice);
                await _context.SaveChangesAsync();
            }
        }
    }
}