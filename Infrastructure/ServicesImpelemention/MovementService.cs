using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services.SettingServices;
using Core.ViewModels.MovementViewModels;
using Core.ViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Services.MovementServices;

namespace Infrastructure.ServicesImpelemention
{
    public class MovementService : IMovementService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISettingService _settingService;

        public MovementService(ApplicationDbContext context, ISettingService settingService)
        {
            _context = context;
            _settingService = settingService;
        }

        public async Task<MovementViewModel> CreateMovement(CreateMovementViewModel movementDto)
        {
            // توليد كود الحركة إذا لم يكن موجوداً
            if (string.IsNullOrEmpty(movementDto.MovementCode))
            {
                movementDto.MovementCode = await GenerateMovementCode(movementDto.MovementTypeId);
            }

            var movement = new Movement
            {
                MovementCode = movementDto.MovementCode,
                MovementTypeId = movementDto.MovementTypeId,
                BranchFromId = movementDto.BranchFromId,
                BranchToId = movementDto.BranchToId,
                SupplierId = movementDto.SupplierId,
                Date = movementDto.Date,
                InternalBarcode = movementDto.InternalBarcode,
                ExternalBarcode = movementDto.ExternalBarcode,
                Notes = movementDto.Notes,
                CreatedBy = movementDto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                Status = "Draft"
            };

            _context.Movements.Add(movement);
            await _context.SaveChangesAsync();

            if (movementDto.Lines != null && movementDto.Lines.Any())
            {
                foreach (var lineDto in movementDto.Lines)
                {
                    var line = new MovementLine
                    {
                        MovementId = movement.MovementId,
                        ItemId = lineDto.ItemId,
                        BranchId = lineDto.BranchId,
                        UnitCode = lineDto.UnitCode,
                        QtyInput = lineDto.QtyInput,
                        ConversionUsedToBase = lineDto.ConversionUsedToBase,
                        QtyBase = lineDto.QtyInput * lineDto.ConversionUsedToBase,
                        UnitPrice = lineDto.UnitPrice,
                        Notes = lineDto.Notes,
                        CreatedBy = lineDto.CreatedBy,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.MovementLines.Add(line);
                }
                await _context.SaveChangesAsync();
            }

            // حساب الإجماليات
            await CalculateMovementTotals(movement.MovementId);

            return await GetMovement(movement.MovementId);
        }

        public async Task<MovementViewModel> UpdateMovement(int movementId, UpdateMovementViewModel movementDto)
        {
            var movement = await _context.Movements
                .Include(m => m.MovementLines)
                .FirstOrDefaultAsync(m => m.MovementId == movementId);

            if (movement == null)
                throw new Exception("الحركة غير موجودة");

            if (movement.Status != "Draft")
                throw new Exception("لا يمكن تعديل الحركة إلا إذا كانت في حالة مسودة");

            movement.Date = movementDto.Date;
            movement.BranchFromId = movementDto.BranchFromId;
            movement.BranchToId = movementDto.BranchToId;
            movement.SupplierId = movementDto.SupplierId;
            movement.InternalBarcode = movementDto.InternalBarcode;
            movement.ExternalBarcode = movementDto.ExternalBarcode;
            movement.Notes = movementDto.Notes;
            movement.CreatedBy = movementDto.ModifiedBy;
            movement.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // إعادة حساب الإجماليات
            await CalculateMovementTotals(movementId);

            return await GetMovement(movementId);
        }

        public async Task<MovementViewModel> GetMovement(int movementId)
        {
            var movement = await _context.Movements
                .Include(m => m.MovementType)
                .Include(m => m.BranchFrom)
                .Include(m => m.BranchTo)
                .Include(m => m.Supplier)
                .Include(m => m.MovementLines)
                    .ThenInclude(l => l.Item)
                .FirstOrDefaultAsync(m => m.MovementId == movementId);

            if (movement == null)
                throw new Exception("الحركة غير موجودة");

            // حساب الإجماليات بشكل مباشر من السطور
            var totalQuantityBase = movement.MovementLines.Sum(l => l.QtyBase);
            var totalValueCost = movement.MovementLines.Sum(l => l.QtyBase * l.UnitPrice);

            // تحديث القيم في الكيان إذا كانت مختلفة (اختياري)
            if (movement.TotalQuantityBase != totalQuantityBase ||
                movement.TotalValueCost != totalValueCost)
            {
                movement.TotalQuantityBase = totalQuantityBase;
                movement.TotalValueCost = totalValueCost;
                await _context.SaveChangesAsync();
            }

            var movementViewModel = new MovementViewModel
            {
                MovementId = movement.MovementId,
                MovementCode = movement.MovementCode,
                MovementTypeId = movement.MovementTypeId,
                MovementTypeName = movement.MovementType?.Name,
                BranchFromId = movement.BranchFromId,
                BranchFromName = movement.BranchFrom?.Name,
                BranchToId = movement.BranchToId,
                BranchToName = movement.BranchTo?.Name,
                SupplierId = movement.SupplierId,
                SupplierName = movement.Supplier?.Name,
                Date = movement.Date,
                Status = movement.Status,
                TotalQuantityBase = totalQuantityBase,
                TotalValueCost = totalValueCost,
                InternalBarcode = movement.InternalBarcode,
                ExternalBarcode = movement.ExternalBarcode,
                Notes = movement.Notes,
                CreatedAt = movement.CreatedAt,
                CreatedBy = movement.CreatedBy,
                Lines = movement.MovementLines.Select(line => new MovementLineViewModel
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
                }).ToList()
            };

            return movementViewModel;
        }

        public async Task<IEnumerable<MovementViewModel>> GetMovements(MovementFilterViewModel filters, PaginationViewModel pagination)
        {
            var query = _context.Movements
                .Include(m => m.MovementType)
                .Include(m => m.BranchFrom)
                .Include(m => m.BranchTo)
                .Include(m => m.Supplier)
                .AsQueryable();

            // تطبيق الفلاتر
            if (!string.IsNullOrEmpty(filters.MovementCode))
                query = query.Where(m => m.MovementCode.Contains(filters.MovementCode));

            if (filters.MovementTypeId.HasValue)
                query = query.Where(m => m.MovementTypeId == filters.MovementTypeId);

            if (filters.BranchId.HasValue)
                query = query.Where(m => m.BranchFromId == filters.BranchId || m.BranchToId == filters.BranchId);

            if (filters.DateFrom.HasValue)
                query = query.Where(m => m.Date >= filters.DateFrom);

            if (filters.DateTo.HasValue)
                query = query.Where(m => m.Date <= filters.DateTo);

            if (!string.IsNullOrEmpty(filters.Status))
                query = query.Where(m => m.Status == filters.Status);

            if (!string.IsNullOrEmpty(filters.Barcode))
                query = query.Where(m => m.InternalBarcode == filters.Barcode || m.ExternalBarcode == filters.Barcode);

            // التصفح
            query = query.OrderByDescending(m => m.Date)
                        .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                        .Take(pagination.PageSize);

            var movements = await query.ToListAsync();

            // تحويل كل حركة إلى MovementViewModel بدون تحميل السطور للكفاءة
            var result = new List<MovementViewModel>();
            foreach (var movement in movements)
            {
                result.Add(new MovementViewModel
                {
                    MovementId = movement.MovementId,
                    MovementCode = movement.MovementCode,
                    MovementTypeId = movement.MovementTypeId,
                    MovementTypeName = movement.MovementType?.Name,
                    BranchFromId = movement.BranchFromId,
                    BranchFromName = movement.BranchFrom?.Name,
                    BranchToId = movement.BranchToId,
                    BranchToName = movement.BranchTo?.Name,
                    SupplierId = movement.SupplierId,
                    SupplierName = movement.Supplier?.Name,
                    Date = movement.Date,
                    Status = movement.Status,
                    TotalQuantityBase = movement.TotalQuantityBase,
                    TotalValueCost = movement.TotalValueCost,
                    InternalBarcode = movement.InternalBarcode,
                    ExternalBarcode = movement.ExternalBarcode,
                    Notes = movement.Notes,
                    CreatedAt = movement.CreatedAt,
                    CreatedBy = movement.CreatedBy,
                    Lines = new List<MovementLineViewModel>()
                });
            }

            return result;
        }

        public async Task<bool> DeleteMovement(int movementId)
        {
            var movement = await _context.Movements
                .Include(m => m.MovementLines)
                .FirstOrDefaultAsync(m => m.MovementId == movementId);

            if (movement == null)
                throw new Exception("الحركة غير موجودة");

            if (movement.Status != "Draft")
                throw new Exception("لا يمكن حذف الحركة إلا إذا كانت في حالة مسودة");

            // حذف السطور أولاً
            _context.MovementLines.RemoveRange(movement.MovementLines);

            // ثم حذف الحركة
            _context.Movements.Remove(movement);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubmitMovement(int movementId)
        {
            var movement = await _context.Movements
                .Include(m => m.MovementLines)
                .FirstOrDefaultAsync(m => m.MovementId == movementId);

            if (movement == null)
                throw new Exception("الحركة غير موجودة");

            if (movement.Status != "Draft")
                throw new Exception("لا يمكن إرسال الحركة إلا إذا كانت في حالة مسودة");

            if (!movement.MovementLines.Any())
                throw new Exception("لا يمكن إرسال حركة بدون سطور");

            // Check approval workflow mode
            var approvalModeEnabled = await _settingService.GetApprovalWorkflowModeAsync();

            if (approvalModeEnabled)
            {
                // Require approvals
                movement.Status = "Pending";
                movement.ApprovalStatusId = 2; // PendingApproval
            }
            else
            {
                // Auto-approve
                movement.Status = "Approved";
                movement.ApprovalStatusId = 1; // Approved
            }

            movement.CreatedAt = DateTime.UtcNow;
            movement.CreatedBy = "System";

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MovementViewModel>> GetMovementsByBarcode(string barcode)
        {
            var movements = await _context.Movements
                .Include(m => m.MovementType)
                .Include(m => m.BranchFrom)
                .Include(m => m.BranchTo)
                .Include(m => m.Supplier)
                .Where(m => m.InternalBarcode == barcode || m.ExternalBarcode == barcode)
                .ToListAsync();

            var result = new List<MovementViewModel>();
            foreach (var movement in movements)
            {
                result.Add(new MovementViewModel
                {
                    MovementId = movement.MovementId,
                    MovementCode = movement.MovementCode,
                    MovementTypeId = movement.MovementTypeId,
                    MovementTypeName = movement.MovementType?.Name,
                    BranchFromId = movement.BranchFromId,
                    BranchFromName = movement.BranchFrom?.Name,
                    BranchToId = movement.BranchToId,
                    BranchToName = movement.BranchTo?.Name,
                    SupplierId = movement.SupplierId,
                    SupplierName = movement.Supplier?.Name,
                    Date = movement.Date,
                    Status = movement.Status,
                    TotalQuantityBase = movement.TotalQuantityBase,
                    TotalValueCost = movement.TotalValueCost,
                    InternalBarcode = movement.InternalBarcode,
                    ExternalBarcode = movement.ExternalBarcode,
                    Notes = movement.Notes,
                    CreatedAt = movement.CreatedAt,
                    CreatedBy = movement.CreatedBy,
                    Lines = new List<MovementLineViewModel>()
                });
            }

            return result;
        }

        private async Task CalculateMovementTotals(int movementId)
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

        private async Task<string> GenerateMovementCode(int movementTypeId)
        {
            var movementType = await _context.MovementTypes.FindAsync(movementTypeId);
            var prefix = movementType?.Code ?? "MOV";
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = await _context.Movements
                .CountAsync(m => m.MovementTypeId == movementTypeId &&
                                m.CreatedAt.Date == DateTime.Today) + 1;

            return $"{prefix}-{date}-{count:D4}";
        }
    }
}