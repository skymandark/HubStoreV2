using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.ItemServices;
using Core.ViewModels;
using Core.ViewModels.ItemViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HubStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IItemUnitService _itemUnitService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(
            IItemService itemService,
            IItemUnitService itemUnitService,
            ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _itemUnitService = itemUnitService;
            _logger = logger;
        }

        #region Item Methods

        /// <summary>
        /// Creates a new item
        /// POST: api/item/create
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<ItemViewModel>> CreateItem([FromBody] CreateItemViewModel itemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                itemDto.CreatedBy = User?.Identity?.Name ?? "System";
                var result = await _itemService.CreateItemAsync(itemDto);
                return CreatedAtAction(nameof(GetItem), new { itemId = result.ItemId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error creating item");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating item");
                return StatusCode(500, new { message = "An error occurred while creating the item." });
            }
        }

        /// <summary>
        /// Updates an existing item
        /// PUT: api/item/update/{itemId}
        /// </summary>
        [HttpPut("update/{itemId}")]
        public async Task<ActionResult<ItemViewModel>> UpdateItem(int itemId, [FromBody] UpdateItemViewModel itemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                itemDto.ModifiedBy = User?.Identity?.Name ?? "System";
                var result = await _itemService.UpdateItemAsync(itemId, itemDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error updating item");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating item");
                return StatusCode(500, new { message = "An error occurred while updating the item." });
            }
        }

        /// <summary>
        /// Gets a specific item by ID
        /// GET: api/item/get/{itemId}
        /// </summary>
        [HttpGet("get/{itemId}")]
        public async Task<ActionResult<ItemViewModel>> GetItem(int itemId)
        {
            try
            {
                var result = await _itemService.GetItemAsync(itemId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Item not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving item");
                return StatusCode(500, new { message = "An error occurred while retrieving the item." });
            }
        }

        /// <summary>
        /// Gets paginated list of items with optional filters
        /// GET: api/item/list?pageNumber=1&pageSize=10&itemCode=&itemName=&isActive=true
        /// </summary>
        [HttpGet("list")]
        public async Task<ActionResult<PaginatedResult<ItemViewModel>>> GetItems(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? itemCode = null,
            [FromQuery] string? itemName = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] bool? isParent = null,
            [FromQuery] string? barcode = null)
        {
            try
            {
                var filters = new ItemFilterViewModel
                {
                    ItemCode = itemCode ?? "",
                    ItemName = itemName ?? "",
                    IsActive = isActive,
                    IsParent = isParent,
                    Barcode = barcode ?? ""
                };

                var pagination = new PaginationViewModel
                {
                    PageNumber = Math.Max(1, pageNumber),
                    PageSize = Math.Max(1, Math.Min(pageSize, 100))
                };

                var result = await _itemService.GetItemsAsync(filters, pagination);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving items");
                return StatusCode(500, new { message = "An error occurred while retrieving items." });
            }
        }

        /// <summary>
        /// Soft deletes an item
        /// DELETE: api/item/delete/{itemId}
        /// </summary>
        [HttpDelete("delete/{itemId}")]
        public async Task<ActionResult<object>> DeleteItem(int itemId)
        {
            try
            {
                var result = await _itemService.DeleteItemAsync(itemId);
                return Ok(new { success = result, message = "Item deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error deleting item");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting item");
                return StatusCode(500, new { message = "An error occurred while deleting the item." });
            }
        }

        /// <summary>
        /// Gets item by barcode (internal or external)
        /// GET: api/item/barcode/{barcode}?isInternal=true
        /// </summary>
        [HttpGet("barcode/{barcode}")]
        [AllowAnonymous]
        public async Task<ActionResult<ItemViewModel>> GetItemByBarcode(string barcode, [FromQuery] bool isInternal = true)
        {
            try
            {
                var result = await _itemService.GetItemByBarcodeAsync(barcode, isInternal);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid barcode");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Item not found by barcode");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving item by barcode");
                return StatusCode(500, new { message = "An error occurred while retrieving the item." });
            }
        }

        /// <summary>
        /// Gets item hierarchy (tree structure)
        /// GET: api/item/hierarchy
        /// </summary>
        [HttpGet("hierarchy")]
        public async Task<ActionResult<ItemHierarchyViewModel>> GetItemHierarchy()
        {
            try
            {
                var result = await _itemService.GetItemHierarchyAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving item hierarchy");
                return StatusCode(500, new { message = "An error occurred while retrieving the item hierarchy." });
            }
        }

        /// <summary>
        /// Validates if an item can be used in movements
        /// GET: api/item/validate-movement/{itemId}
        /// </summary>
        [HttpGet("validate-movement/{itemId}")]
        public async Task<ActionResult<ValidationResult>> ValidateItemForMovement(int itemId)
        {
            try
            {
                var result = await _itemService.ValidateItemForMovementAsync(itemId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating item for movement");
                return StatusCode(500, new { message = "An error occurred while validating the item." });
            }
        }

        #endregion

        #region ItemUnit Methods

        /// <summary>
        /// Creates a new unit for an item
        /// POST: api/item/{itemId}/unit/create
        /// </summary>
        [HttpPost("{itemId}/unit/create")]
        public async Task<ActionResult<ItemUnitViewModel>> CreateItemUnit(int itemId, [FromBody] CreateItemUnitViewModel unitDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                unitDto.CreatedBy = User?.Identity?.Name ?? "System";
                var result = await _itemUnitService.CreateItemUnitAsync(itemId, unitDto);
                return CreatedAtAction(nameof(GetItemUnits), new { itemId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error creating item unit");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating item unit");
                return StatusCode(500, new { message = "An error occurred while creating the item unit." });
            }
        }

        /// <summary>
        /// Updates an existing item unit
        /// PUT: api/item/unit/update/{itemUnitId}
        /// </summary>
        [HttpPut("unit/update/{itemUnitId}")]
        public async Task<ActionResult<ItemUnitViewModel>> UpdateItemUnit(int itemUnitId, [FromBody] UpdateItemUnitViewModel unitDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                unitDto.ModifiedBy = User?.Identity?.Name ?? "System";
                var result = await _itemUnitService.UpdateItemUnitAsync(itemUnitId, unitDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation error updating item unit");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating item unit");
                return StatusCode(500, new { message = "An error occurred while updating the item unit." });
            }
        }

        /// <summary>
        /// Gets all units for a specific item
        /// GET: api/item/{itemId}/units
        /// </summary>
        [HttpGet("{itemId}/units")]
        public async Task<ActionResult<List<ItemUnitViewModel>>> GetItemUnits(int itemId)
        {
            try
            {
                var result = await _itemUnitService.GetItemUnitsAsync(itemId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Item not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving item units");
                return StatusCode(500, new { message = "An error occurred while retrieving item units." });
            }
        }

        /// <summary>
        /// Deletes an item unit
        /// DELETE: api/item/unit/delete/{itemUnitId}
        /// </summary>
        [HttpDelete("unit/delete/{itemUnitId}")]
        public async Task<ActionResult<object>> DeleteItemUnit(int itemUnitId)
        {
            try
            {
                var result = await _itemUnitService.DeleteItemUnitAsync(itemUnitId);
                return Ok(new { success = result, message = "Item unit deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error deleting item unit");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting item unit");
                return StatusCode(500, new { message = "An error occurred while deleting the item unit." });
            }
        }

        /// <summary>
        /// Gets conversion history for a unit
        /// GET: api/item/{itemId}/unit/{unitCode}/history
        /// </summary>
        [HttpGet("{itemId}/unit/{unitCode}/history")]
        public async Task<ActionResult<List<UnitConversionHistoryViewModel>>> GetUnitConversionHistory(int itemId, string unitCode)
        {
            try
            {
                var result = await _itemUnitService.GetUnitConversionHistoryAsync(itemId, unitCode);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Item not found");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unit conversion history");
                return StatusCode(500, new { message = "An error occurred while retrieving the conversion history." });
            }
        }

        /// <summary>
        /// Validates unit conversion values
        /// GET: api/item/{itemId}/unit/{unitCode}/validate
        /// </summary>
        [HttpGet("{itemId}/unit/{unitCode}/validate")]
        public async Task<ActionResult<ValidationResult>> ValidateUnitConversion(int itemId, string unitCode)
        {
            try
            {
                var result = await _itemUnitService.ValidateUnitConversionAsync(itemId, unitCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating unit conversion");
                return StatusCode(500, new { message = "An error occurred while validating the unit conversion." });
            }
        }

        #endregion
    }
}
