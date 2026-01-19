using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Services.ConversionServices;
using Core.Services.InventoryServices;
using Core.ViewModels;
using Core.ViewModels.ConversionViewModels;
using Core.ViewModels.InventoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversionController : ControllerBase
    {
        private readonly IUnitConversionService _conversionService;
        private readonly ILogger<ConversionController> _logger;

        public ConversionController(IUnitConversionService conversionService, ILogger<ConversionController> logger)
        {
            _conversionService = conversionService;
            _logger = logger;
        }

        /// <summary>
        /// Converts quantity from a specific unit to base unit
        /// POST: api/conversion/to-base
        /// </summary>
        [HttpPost("to-base")]
        public async Task<ActionResult<ConversionResultViewModel>> ConvertToBase(
            [FromQuery] int itemId,
            [FromQuery] string unitCode,
            [FromQuery] decimal qtyInput)
        {
            try
            {
                var result = await _conversionService.ConvertToBaseAsync(itemId, unitCode, qtyInput);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid conversion request");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting to base unit");
                return StatusCode(500, new { message = "Error converting quantity." });
            }
        }

        /// <summary>
        /// Breaks down base quantity to all available units for an item
        /// GET: api/conversion/breakdown?itemId=1&baseQty=100
        /// </summary>
        [HttpGet("breakdown")]
        public async Task<ActionResult<BreakdownResultViewModel>> BreakdownFromBase(
            [FromQuery] int itemId,
            [FromQuery] decimal baseQty)
        {
            try
            {
                var result = await _conversionService.BreakdownFromBaseAsync(itemId, baseQty);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid breakdown request");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error breaking down quantity");
                return StatusCode(500, new { message = "Error breaking down quantity." });
            }
        }

        /// <summary>
        /// Gets conversion factor for a unit as of a specific date
        /// GET: api/conversion/factor?itemId=1&unitCode=BOX&asOfDate=2025-12-14
        /// </summary>
        [HttpGet("factor")]
        public async Task<ActionResult<object>> GetConversionFactor(
            [FromQuery] int itemId,
            [FromQuery] string unitCode,
            [FromQuery] DateTime? asOfDate = null)
        {
            try
            {
                var factor = await _conversionService.GetConversionFactorAsync(itemId, unitCode, asOfDate);
                return Ok(new { itemId, unitCode, conversionFactor = factor, asOfDate = asOfDate ?? DateTime.UtcNow });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Unit not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversion factor");
                return StatusCode(500, new { message = "Error getting conversion factor." });
            }
        }

        /// <summary>
        /// Validates if a conversion is valid
        /// GET: api/conversion/validate?itemId=1&unitCode=BOX&qty=5
        /// </summary>
        [HttpGet("validate")]
        public async Task<ActionResult<ValidationResult>> ValidateConversion(
            [FromQuery] int itemId,
            [FromQuery] string unitCode,
            [FromQuery] decimal qty)
        {
            try
            {
                var result = await _conversionService.ValidateConversionAsync(itemId, unitCode, qty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating conversion");
                return StatusCode(500, new { message = "Error validating conversion." });
            }
        }
    }
}
