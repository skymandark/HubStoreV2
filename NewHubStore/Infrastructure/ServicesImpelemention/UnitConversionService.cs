using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.ViewModels;
using Core.ViewModels.ConversionViewModels;
using Core.Services.ConversionServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HubStoreV2.Services.ConversionServices
{
    public class UnitConversionService : IUnitConversionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UnitConversionService> _logger;

        public UnitConversionService(ApplicationDbContext context, ILogger<UnitConversionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Converts quantity from a specific unit to base unit
        /// </summary>
        public async Task<ConversionResultViewModel> ConvertToBaseAsync(int itemId, string unitCode, decimal qtyInput)
        {
            try
            {
                // Validate input
                if (qtyInput < 0)
                    throw new InvalidOperationException("Quantity cannot be negative.");

                // Get the unit and conversion factor
                var unit = await _context.ItemUnits
                    .Where(u => u.ItemId == itemId && u.UnitCode == unitCode && u.IsActive)
                    .FirstOrDefaultAsync();

                if (unit == null)
                    throw new InvalidOperationException($"Unit '{unitCode}' not found for item {itemId}.");

                // Calculate base quantity
                var qtyBase = qtyInput * unit.ConversionToBase;

                _logger.LogInformation($"Converted {qtyInput} {unitCode} to {qtyBase} base units for item {itemId}");

                return new ConversionResultViewModel
                {
                    ItemId = itemId,
                    UnitCode = unitCode,
                    QtyInput = qtyInput,
                    ConversionFactor = unit.ConversionToBase,
                    QtyBase = qtyBase
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error converting quantity for item {itemId} with unit {unitCode}");
                throw new ApplicationException($"Error converting quantity: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Breaks down base quantity to all available units for an item
        /// </summary>
        public async Task<BreakdownResultViewModel> BreakdownFromBaseAsync(int itemId, decimal baseQty)
        {
            try
            {
                // Validate item exists
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                    throw new InvalidOperationException($"Item {itemId} not found.");

                if (baseQty < 0)
                    throw new InvalidOperationException("Base quantity cannot be negative.");

                // Get all active units for this item
                var units = await _context.ItemUnits
                    .Where(u => u.ItemId == itemId && u.IsActive)
                    .OrderByDescending(u => u.IsDefaultForDisplay)
                    .ToListAsync();

                if (!units.Any())
                    throw new InvalidOperationException($"No units defined for item {itemId}.");

                var breakdown = new BreakdownResultViewModel
                {
                    ItemId = itemId,
                    BaseQuantity = baseQty,
                    BreakdownByUnit = new Dictionary<string, decimal>()
                };

                // Calculate breakdown for each unit
                foreach (var unit in units)
                {
                    if (unit.ConversionToBase > 0)
                    {
                        var qtyInUnit = baseQty / unit.ConversionToBase;
                        breakdown.BreakdownByUnit[unit.UnitCode] = qtyInUnit;
                    }
                }

                _logger.LogInformation($"Breakdown of {baseQty} base units for item {itemId} into {breakdown.BreakdownByUnit.Count} units");

                return breakdown;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error breaking down quantity for item {itemId}");
                throw new ApplicationException($"Error breaking down quantity: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets conversion factor for a unit as of a specific date
        /// </summary>
        public async Task<decimal> GetConversionFactorAsync(int itemId, string unitCode, DateTime? asOfDate = null)
        {
            try
            {
                asOfDate = asOfDate ?? DateTime.UtcNow;

                // Get the current unit
                var unit = await _context.ItemUnits
                    .Where(u => u.ItemId == itemId && u.UnitCode == unitCode && u.IsActive)
                    .FirstOrDefaultAsync();

                if (unit == null)
                    throw new InvalidOperationException($"Unit '{unitCode}' not found for item {itemId}.");

                // For current conversions, return the conversion factor
                if (asOfDate >= DateTime.UtcNow.Date)
                {
                    return unit.ConversionToBase;
                }

                // For historical dates, check ItemUnitHistory
                var historicalRecord = await _context.ItemUnitHistories
                    .Where(h => h.ItemId == itemId && 
                           h.UnitCode == unitCode && 
                           h.ChangedDate <= asOfDate)
                    .OrderByDescending(h => h.ChangedDate)
                    .FirstOrDefaultAsync();

                if (historicalRecord != null)
                {
                    return historicalRecord.OldConversionToBase;
                }

                // If no history found, return current
                return unit.ConversionToBase;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting conversion factor for item {itemId}, unit {unitCode}");
                throw new ApplicationException($"Error getting conversion factor: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validates if a conversion is valid
        /// </summary>
        public async Task<ValidationResult> ValidateConversionAsync(int itemId, string unitCode, decimal qty)
        {
            var result = new ValidationResult { IsValid = true };

            try
            {
                // Validate quantity
                if (qty < 0)
                {
                    result.IsValid = false;
                    result.Errors.Add("Quantity cannot be negative.");
                    return result;
                }

                // Get the unit
                var unit = await _context.ItemUnits
                    .Where(u => u.ItemId == itemId && u.UnitCode == unitCode && u.IsActive)
                    .FirstOrDefaultAsync();

                if (unit == null)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Unit '{unitCode}' not found for item {itemId}.");
                    return result;
                }

                // Validate conversion factor
                if (unit.ConversionToBase <= 0)
                {
                    result.IsValid = false;
                    result.Errors.Add("Conversion factor must be greater than zero.");
                    return result;
                }

                // Calculate base quantity
                var baseQty = qty * unit.ConversionToBase;

                // Check for precision issues (very large or very small numbers)
                if (baseQty > 999999999.999999m || (baseQty > 0 && baseQty < 0.000001m))
                {
                    result.IsValid = false;
                    result.Errors.Add("Conversion results in invalid precision. Please use different unit or quantity.");
                    return result;
                }

                result.Data["ItemId"] = itemId;
                result.Data["UnitCode"] = unitCode;
                result.Data["QtyInput"] = qty;
                result.Data["ConversionFactor"] = unit.ConversionToBase;
                result.Data["QtyBase"] = baseQty;

                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add($"Error validating conversion: {ex.Message}");
                return result;
            }
        }
    }
}
