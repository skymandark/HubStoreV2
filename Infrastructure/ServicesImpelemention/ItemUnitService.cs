using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels;
using Core.ViewModels.ItemViewModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.ItemServices
{
    public class ItemUnitService : IItemUnitService
    {
        private readonly ApplicationDbContext _context;

        public ItemUnitService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new unit for an item
        /// </summary>
        public async Task<ItemUnitViewModel> CreateItemUnitAsync(int itemId, CreateItemUnitViewModel unitDto)
        {
            try
            {
                // Verify item exists
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                    throw new InvalidOperationException($"Item with ID '{itemId}' not found.");

                // Check if unit code already exists for this item
                var existingUnit = await _context.ItemUnits
                    .Where(u => u.ItemId == itemId && u.UnitCode == unitDto.UnitCode && u.IsActive)
                    .FirstOrDefaultAsync();

                if (existingUnit != null)
                    throw new InvalidOperationException($"Unit code '{unitDto.UnitCode}' already exists for this item.");

                // Validate ConversionToBase
                if (unitDto.ConversionToBase <= 0)
                    throw new InvalidOperationException("ConversionToBase must be greater than zero.");

                // Validate ParentUnitId if provided
                if (unitDto.ParentUnitId.HasValue)
                {
                    var parentUnit = await _context.ItemUnits
                        .Where(u => u.ItemUnitId == unitDto.ParentUnitId && u.IsActive)
                        .FirstOrDefaultAsync();

                    if (parentUnit == null)
                        throw new InvalidOperationException($"Parent unit with ID '{unitDto.ParentUnitId}' does not exist.");

                    if (parentUnit.ItemId != itemId)
                        throw new InvalidOperationException("Parent unit must belong to the same item.");
                }

                var itemUnit = new ItemUnit
                {
                    ItemId = itemId,
                    UnitCode = unitDto.UnitCode,
                    UnitName = unitDto.UnitName,
                    ConversionToBase = unitDto.ConversionToBase,
                    ParentUnitId = unitDto.ParentUnitId,
                    IsDefaultForDisplay = unitDto.IsDefaultForDisplay,
                    CreatedBy = unitDto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.ItemUnits.Add(itemUnit);
                await _context.SaveChangesAsync();

                return MapToViewModel(itemUnit);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error creating item unit: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing item unit
        /// </summary>
        public async Task<ItemUnitViewModel> UpdateItemUnitAsync(int itemUnitId, UpdateItemUnitViewModel unitDto)
        {
            try
            {
                var itemUnit = await _context.ItemUnits
                    .Where(u => u.ItemUnitId == itemUnitId)
                    .FirstOrDefaultAsync();

                if (itemUnit == null)
                    throw new InvalidOperationException($"Item unit with ID '{itemUnitId}' not found.");

                // Validate ConversionToBase
                if (unitDto.ConversionToBase <= 0)
                    throw new InvalidOperationException("ConversionToBase must be greater than zero.");

                // Log the change to ItemUnitHistory
                var history = new ItemUnitHistory
                {
                    ItemId = itemUnit.ItemId,
                    UnitCode = itemUnit.UnitCode,
                    OldConversionToBase = itemUnit.ConversionToBase,
                    NewConversionToBase = unitDto.ConversionToBase,
                    ChangedByUserId = unitDto.ModifiedBy,
                    ChangedDate = DateTime.UtcNow,
                    Notes = $"Updated unit conversion from {itemUnit.ConversionToBase} to {unitDto.ConversionToBase}"
                };

                itemUnit.UnitName = unitDto.UnitName ?? itemUnit.UnitName;
                itemUnit.ConversionToBase = unitDto.ConversionToBase;
                itemUnit.IsDefaultForDisplay = unitDto.IsDefaultForDisplay;
                itemUnit.IsActive = unitDto.IsActive;
                itemUnit.ModifiedBy = unitDto.ModifiedBy;
                itemUnit.ModifiedAt = DateTime.UtcNow;

                _context.ItemUnits.Update(itemUnit);
                _context.ItemUnitHistories.Add(history);
                await _context.SaveChangesAsync();

                return MapToViewModel(itemUnit);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating item unit: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all units for a specific item
        /// </summary>
        public async Task<List<ItemUnitViewModel>> GetItemUnitsAsync(int itemId)
        {
            try
            {
                // Verify item exists
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                    throw new InvalidOperationException($"Item with ID '{itemId}' not found.");

                var units = await _context.ItemUnits
                    .Where(u => u.ItemId == itemId && u.IsActive)
                    .OrderByDescending(u => u.IsDefaultForDisplay)
                    .ThenBy(u => u.CreatedAt)
                    .ToListAsync();

                return units.Select(MapToViewModel).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving item units: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes an item unit
        /// </summary>
        public async Task<bool> DeleteItemUnitAsync(int itemUnitId)
        {
            try
            {
                var itemUnit = await _context.ItemUnits
                    .Where(u => u.ItemUnitId == itemUnitId)
                    .FirstOrDefaultAsync();

                if (itemUnit == null)
                    throw new InvalidOperationException($"Item unit with ID '{itemUnitId}' not found.");

                // Check if unit is used in movements
                var hasMovementLines = await _context.MovementLines
                    .Where(ml => ml.ItemId == itemUnit.ItemId)
                    .AnyAsync();

                if (hasMovementLines)
                    throw new InvalidOperationException("Cannot delete unit that has associated movements.");

                // Check if unit is used in orders
                var hasOrderLines = await _context.OrderLines
                    .Where(ol => ol.ItemId == itemUnit.ItemId)
                    .AnyAsync();

                if (hasOrderLines)
                    throw new InvalidOperationException("Cannot delete unit that has associated orders.");

                // Check if there are child units
                var hasChildUnits = await _context.ItemUnits
                    .Where(u => u.ParentUnitId == itemUnitId)
                    .AnyAsync();

                if (hasChildUnits)
                    throw new InvalidOperationException("Cannot delete unit that has child units.");

                itemUnit.IsActive = false;
                itemUnit.ModifiedAt = DateTime.UtcNow;

                _context.ItemUnits.Update(itemUnit);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting item unit: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets conversion history for a unit
        /// </summary>
        public async Task<List<UnitConversionHistoryViewModel>> GetUnitConversionHistoryAsync(int itemId, string unitCode)
        {
            try
            {
                // Verify item exists
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                    throw new InvalidOperationException($"Item with ID '{itemId}' not found.");

                var histories = await _context.ItemUnitHistories
                    .Where(h => h.ItemId == itemId && h.UnitCode == unitCode)
                    .OrderByDescending(h => h.ChangedDate)
                    .ToListAsync();

                return histories.Select(MapHistoryToViewModel).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving unit conversion history: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validates unit conversion values
        /// </summary>
        public async Task<ValidationResult> ValidateUnitConversionAsync(int itemId, string unitCode)
        {
            var result = new ValidationResult { IsValid = true };

            try
            {
                // Verify item exists
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Item with ID '{itemId}' not found.");
                    return result;
                }

                // Find unit
                var unit = await _context.ItemUnits
                    .Where(u => u.ItemId == itemId && u.UnitCode == unitCode && u.IsActive)
                    .FirstOrDefaultAsync();

                if (unit == null)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Unit with code '{unitCode}' not found for this item.");
                    return result;
                }

                // Validation checks
                if (unit.ConversionToBase <= 0)
                {
                    result.IsValid = false;
                    result.Errors.Add("Conversion value must be greater than zero.");
                }

                // Check for circular parent-child relationships
                var parentUnitId = unit.ParentUnitId;
                var visitedIds = new HashSet<int> { unit.ItemUnitId };

                while (parentUnitId.HasValue)
                {
                    if (visitedIds.Contains(parentUnitId.Value))
                    {
                        result.IsValid = false;
                        result.Errors.Add("Circular reference detected in unit hierarchy.");
                        break;
                    }

                    var parentUnit = await _context.ItemUnits
                        .Where(u => u.ItemUnitId == parentUnitId && u.IsActive)
                        .FirstOrDefaultAsync();

                    if (parentUnit == null)
                    {
                        result.IsValid = false;
                        result.Errors.Add("Parent unit not found in hierarchy.");
                        break;
                    }

                    visitedIds.Add(parentUnit.ItemUnitId);
                    parentUnitId = parentUnit.ParentUnitId;
                }

                // Add unit details to result data
                result.Data["ItemUnitId"] = unit.ItemUnitId;
                result.Data["UnitCode"] = unit.UnitCode;
                result.Data["UnitName"] = unit.UnitName;
                result.Data["ConversionToBase"] = unit.ConversionToBase;

                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add($"Error validating unit conversion: {ex.Message}");
                return result;
            }
        }

        // Helper Methods
        private ItemUnitViewModel MapToViewModel(ItemUnit itemUnit)
        {
            return new ItemUnitViewModel
            {
                ItemUnitId = itemUnit.ItemUnitId,
                ItemId = itemUnit.ItemId,
                UnitCode = itemUnit.UnitCode,
                UnitName = itemUnit.UnitName,
                ConversionToBase = itemUnit.ConversionToBase,
                ParentUnitId = itemUnit.ParentUnitId,
                IsDefaultForDisplay = itemUnit.IsDefaultForDisplay,
                IsActive = itemUnit.IsActive,
                CreatedAt = itemUnit.CreatedAt,
                CreatedBy = itemUnit.CreatedBy
            };
        }

        private UnitConversionHistoryViewModel MapHistoryToViewModel(ItemUnitHistory history)
        {
            return new UnitConversionHistoryViewModel
            {
                ItemUnitHistoryId = history.ItemUnitHistoryId,
                ItemId = history.ItemId,
                UnitCode = history.UnitCode,
                OldConversionToBase = history.OldConversionToBase,
                NewConversionToBase = history.NewConversionToBase,
                ChangedDate = history.ChangedDate,
                ChangedByUser = history.ChangedByUser?.UserName ?? history.ChangedByUserId,
                Notes = history.Notes
            };
        }
    }
}
