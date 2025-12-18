using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Services.InventoryServices;
using Core.ViewModels;
using Core.ViewModels.InventoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryCalculationService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryCalculationService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        /// <summary>
        /// Calculates current balance for an item in a branch as of a date
        /// GET: api/inventory/balance?itemId=1&branchId=1&asOfDate=2025-12-14
        /// </summary>
        [HttpGet("balance")]
        public async Task<ActionResult<BalanceViewModel>> CalculateBalance(
            [FromQuery] int itemId,
            [FromQuery] int branchId,
            [FromQuery] DateTime? asOfDate = null)
        {
            try
            {
                var date = asOfDate ?? DateTime.UtcNow;
                var result = await _inventoryService.CalculateBalanceAsync(itemId, branchId, date);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Item or branch not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating balance");
                return StatusCode(500, new { message = "Error calculating balance." });
            }
        }

        /// <summary>
        /// Gets opening balance for an item in a branch for a fiscal year
        /// GET: api/inventory/opening-balance?itemId=1&branchId=1&fiscalYear=2025
        /// </summary>
        [HttpGet("opening-balance")]
        public async Task<ActionResult<OpeningBalanceViewModel>> GetOpeningBalance(
            [FromQuery] int itemId,
            [FromQuery] int branchId,
            [FromQuery] int fiscalYear)
        {
            try
            {
                var result = await _inventoryService.GetOpeningBalanceAsync(itemId, branchId, fiscalYear);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Opening balance not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting opening balance");
                return StatusCode(500, new { message = "Error getting opening balance." });
            }
        }

        /// <summary>
        /// Gets balance for all items in a branch as of a date
        /// GET: api/inventory/branch-balance?branchId=1&asOfDate=2025-12-14
        /// </summary>
        [HttpGet("branch-balance")]
        public async Task<ActionResult<List<BalanceViewModel>>> CalculateBalanceByBranch(
            [FromQuery] int branchId,
            [FromQuery] DateTime? asOfDate = null)
        {
            try
            {
                var date = asOfDate ?? DateTime.UtcNow;
                var result = await _inventoryService.CalculateBalanceByBranchAsync(branchId, date);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Branch not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating branch balance");
                return StatusCode(500, new { message = "Error calculating branch balance." });
            }
        }

        /// <summary>
        /// Calculates the impact of a movement on inventory
        /// GET: api/inventory/movement-impact?movementId=1
        /// </summary>
        [HttpGet("movement-impact")]
        public async Task<ActionResult<MovementImpactViewModel>> GetMovementImpact(
            [FromQuery] int movementId)
        {
            try
            {
                var result = await _inventoryService.GetMovementImpactAsync(movementId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Movement not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating movement impact");
                return StatusCode(500, new { message = "Error calculating movement impact." });
            }
        }

        /// <summary>
        /// Validates if sufficient stock is available for a transaction
        /// GET: api/inventory/validate-availability?itemId=1&branchId=1&qtyBase=10
        /// </summary>
        [HttpGet("validate-availability")]
        public async Task<ActionResult<ValidationResult>> ValidateStockAvailability(
            [FromQuery] int itemId,
            [FromQuery] int branchId,
            [FromQuery] decimal qtyBase)
        {
            try
            {
                var result = await _inventoryService.ValidateStockAvailabilityAsync(itemId, branchId, qtyBase);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating stock availability");
                return StatusCode(500, new { message = "Error validating stock availability." });
            }
        }
    }
}
