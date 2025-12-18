# Quick Start Guide - ItemService & ItemUnitService

## 1. Basic Usage in Controllers

### Using ItemService

```csharp
public class YourController : ControllerBase
{
    private readonly IItemService _itemService;
    
    public YourController(IItemService itemService)
    {
        _itemService = itemService;
    }
    
    // Create item
    public async Task<IActionResult> CreateProduct()
    {
        var itemDto = new CreateItemViewModel
        {
            ItemCode = "PROD-001",
            ItemName = "Laptop",
            IsParent = false,
            MainUnitCode = "PCE",
            BaseUnitCode = "PCE",
            InternalBarcode = "INT-PROD-001",
            ExternalBarcode = "1234567890",
            CreatedBy = User?.Identity?.Name ?? "System"
        };
        
        var result = await _itemService.CreateItemAsync(itemDto);
        return Ok(result);
    }
    
    // Get item
    public async Task<IActionResult> GetProduct(int itemId)
    {
        var item = await _itemService.GetItemAsync(itemId);
        return Ok(item);
    }
}
```

### Using ItemUnitService

```csharp
public class YourController : ControllerBase
{
    private readonly IItemUnitService _itemUnitService;
    
    public YourController(IItemUnitService itemUnitService)
    {
        _itemUnitService = itemUnitService;
    }
    
    // Add unit to item
    public async Task<IActionResult> AddUnit(int itemId)
    {
        var unitDto = new CreateItemUnitViewModel
        {
            UnitCode = "BOX",
            UnitName = "Box of 10",
            ConversionToBase = 10m,
            IsDefaultForDisplay = false,
            CreatedBy = User?.Identity?.Name ?? "System"
        };
        
        var result = await _itemUnitService.CreateItemUnitAsync(itemId, unitDto);
        return Ok(result);
    }
    
    // Get all units for item
    public async Task<IActionResult> GetUnits(int itemId)
    {
        var units = await _itemUnitService.GetItemUnitsAsync(itemId);
        return Ok(units);
    }
}
```

## 2. Common API Requests

### Create Item
```bash
POST /api/item/create
Content-Type: application/json

{
  "itemCode": "SKU-001",
  "itemName": "Sample Product",
  "isParent": false,
  "mainUnitCode": "PCE",
  "baseUnitCode": "PCE",
  "internalBarcode": "INT-001",
  "externalBarcode": "EXT-001",
  "notes": "Sample product for testing"
}
```

### Get Items with Pagination
```bash
GET /api/item/list?pageNumber=1&pageSize=20&isActive=true
```

### Search by Barcode
```bash
GET /api/item/barcode/INT-001?isInternal=true
```

### Get Item Hierarchy
```bash
GET /api/item/hierarchy
```

### Validate Item for Movement
```bash
GET /api/item/validate-movement/1
```

### Create Unit for Item
```bash
POST /api/item/1/unit/create
Content-Type: application/json

{
  "unitCode": "BOX",
  "unitName": "Box",
  "conversionToBase": 10,
  "isDefaultForDisplay": false
}
```

## 3. Error Handling Examples

### Try-Catch Pattern
```csharp
try
{
    var item = await _itemService.CreateItemAsync(itemDto);
}
catch (InvalidOperationException ex)
{
    // Business logic validation error
    return BadRequest(new { error = ex.Message });
}
catch (ApplicationException ex)
{
    // Database or system error
    return StatusCode(500, new { error = ex.Message });
}
```

### Validation Result Handling
```csharp
var validation = await _itemService.ValidateItemForMovementAsync(itemId);

if (!validation.IsValid)
{
    var errors = string.Join("; ", validation.Errors);
    return BadRequest(new { errors });
}

// Item is valid, proceed with movement
```

## 4. Filtering Examples

### Filter by Active Items Only
```csharp
var filters = new ItemFilterViewModel
{
    IsActive = true
};

var pagination = new PaginationViewModel { PageNumber = 1, PageSize = 10 };
var results = await _itemService.GetItemsAsync(filters, pagination);
```

### Filter by Item Code
```csharp
var filters = new ItemFilterViewModel
{
    ItemCode = "PROD"  // Partial match, case-insensitive
};

var results = await _itemService.GetItemsAsync(filters, pagination);
```

### Filter by Multiple Criteria
```csharp
var filters = new ItemFilterViewModel
{
    ItemCode = "PROD",
    IsActive = true,
    IsParent = false,
    Barcode = "INT"
};

var results = await _itemService.GetItemsAsync(filters, pagination);
```

## 5. Working with Hierarchies

### Create Parent Item
```csharp
var parentItem = new CreateItemViewModel
{
    ItemCode = "CATEGORY-ELECTRONICS",
    ItemName = "Electronics",
    IsParent = true,  // Set to true for parent
    MainUnitCode = "CAT",
    BaseUnitCode = "CAT",
    InternalBarcode = "INT-CAT-001"
};

var parent = await _itemService.CreateItemAsync(parentItem);
```

### Create Child Item
```csharp
var childItem = new CreateItemViewModel
{
    ItemCode = "LAPTOP-001",
    ItemName = "Dell Laptop",
    ParentItemId = parent.ItemId,  // Reference parent
    IsParent = false,
    MainUnitCode = "PCE",
    BaseUnitCode = "PCE",
    InternalBarcode = "INT-LAPTOP-001"
};

var child = await _itemService.CreateItemAsync(childItem);
```

### Get Full Hierarchy
```csharp
var hierarchy = await _itemService.GetItemHierarchyAsync();

// hierarchy.ChildItems contains all root items
// Each item has ChildItems collection with sub-items
foreach (var rootItem in hierarchy.ChildItems)
{
    Console.WriteLine($"{rootItem.ItemCode}: {rootItem.ItemName}");
    foreach (var childItem in rootItem.ChildItems)
    {
        Console.WriteLine($"  └── {childItem.ItemCode}: {childItem.ItemName}");
    }
}
```

## 6. Unit Conversion Management

### Create Units with Conversions
```csharp
// Base unit (conversion = 1)
var baseUnit = new CreateItemUnitViewModel
{
    UnitCode = "PCE",
    UnitName = "Piece",
    ConversionToBase = 1m,
    IsDefaultForDisplay = true,
    CreatedBy = "admin"
};
var unit1 = await _itemUnitService.CreateItemUnitAsync(itemId, baseUnit);

// Secondary unit (conversion = 10)
var secondaryUnit = new CreateItemUnitViewModel
{
    UnitCode = "BOX",
    UnitName = "Box of 10",
    ConversionToBase = 10m,
    IsDefaultForDisplay = false,
    CreatedBy = "admin"
};
var unit2 = await _itemUnitService.CreateItemUnitAsync(itemId, secondaryUnit);
```

### Update Unit Conversion
```csharp
var updateDto = new UpdateItemUnitViewModel
{
    UnitName = "Box of 12",
    ConversionToBase = 12m,
    IsDefaultForDisplay = false,
    IsActive = true,
    ModifiedBy = "admin"
};

var updated = await _itemUnitService.UpdateItemUnitAsync(unitId, updateDto);
// Automatically creates history entry
```

### View Conversion History
```csharp
var history = await _itemUnitService.GetUnitConversionHistoryAsync(itemId, "BOX");

foreach (var entry in history)
{
    Console.WriteLine($"{entry.ChangedDate:g}");
    Console.WriteLine($"  {entry.OldConversionToBase} → {entry.NewConversionToBase}");
    Console.WriteLine($"  Changed by: {entry.ChangedByUser}");
}
```

## 7. Deletion Operations

### Delete Item (Soft Delete)
```csharp
try
{
    var success = await _itemService.DeleteItemAsync(itemId);
    if (success)
    {
        Console.WriteLine("Item deleted successfully");
    }
}
catch (InvalidOperationException ex)
{
    // Item has child items or active movements
    Console.WriteLine($"Cannot delete: {ex.Message}");
}
```

### Delete Unit (Soft Delete)
```csharp
try
{
    var success = await _itemUnitService.DeleteItemUnitAsync(unitId);
    if (success)
    {
        Console.WriteLine("Unit deleted successfully");
    }
}
catch (InvalidOperationException ex)
{
    // Unit has child units or is used in movements
    Console.WriteLine($"Cannot delete: {ex.Message}");
}
```

## 8. Validation Before Operations

### Validate Item for Movement
```csharp
var validation = await _itemService.ValidateItemForMovementAsync(itemId);

if (validation.IsValid)
{
    // Extract validated data
    var itemCode = validation.Data["ItemCode"];
    var itemName = validation.Data["ItemName"];
    
    // Proceed with movement
}
else
{
    // Show errors to user
    foreach (var error in validation.Errors)
    {
        Console.WriteLine($"Error: {error}");
    }
}
```

### Validate Unit Conversion
```csharp
var validation = await _itemUnitService.ValidateUnitConversionAsync(itemId, "BOX");

if (validation.IsValid)
{
    var conversionValue = validation.Data["ConversionToBase"];
    // Use validated conversion value
}
else
{
    // Handle validation errors (circular reference, etc.)
}
```

## 9. Dependency Injection Setup

### In Program.cs
```csharp
// Register services
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemUnitService, ItemUnitService>();

// In your controller, inject via constructor
public class YourController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly IItemUnitService _itemUnitService;
    
    public YourController(IItemService itemService, IItemUnitService itemUnitService)
    {
        _itemService = itemService;
        _itemUnitService = itemUnitService;
    }
}
```

## 10. Common Patterns

### Create Item with Units
```csharp
public async Task<IActionResult> CreateProductWithUnits(CreateItemViewModel itemDto)
{
    // Step 1: Create item
    var item = await _itemService.CreateItemAsync(itemDto);
    
    // Step 2: Add base unit
    var baseUnit = new CreateItemUnitViewModel
    {
        UnitCode = "PCE",
        UnitName = "Piece",
        ConversionToBase = 1m,
        IsDefaultForDisplay = true,
        CreatedBy = User?.Identity?.Name ?? "System"
    };
    await _itemUnitService.CreateItemUnitAsync(item.ItemId, baseUnit);
    
    // Step 3: Validate for movement
    var validation = await _itemService.ValidateItemForMovementAsync(item.ItemId);
    
    return Ok(new { item, isValidForMovement = validation.IsValid });
}
```

### Search and Validate
```csharp
public async Task<IActionResult> SearchAndValidate(string barcode)
{
    try
    {
        // Find item by barcode
        var item = await _itemService.GetItemByBarcodeAsync(barcode, isInternal: true);
        
        // Validate for movement
        var validation = await _itemService.ValidateItemForMovementAsync(item.ItemId);
        
        return Ok(new { item, validation });
    }
    catch (InvalidOperationException ex)
    {
        return NotFound(new { error = ex.Message });
    }
}
```

---

## Troubleshooting

### Issue: "Item code already exists"
**Solution:** Use unique item codes. Check ItemCode before creating.

### Issue: "Cannot delete item that has child items"
**Solution:** Delete child items first, or use a different approach (mark as inactive).

### Issue: "Circular reference detected in unit hierarchy"
**Solution:** Don't set a unit as its own parent or create cycles in parent relationships.

### Issue: "Cannot delete unit that has associated movements"
**Solution:** Items in movements are protected. Use inactive units instead.

---

For more detailed information, see:
- [ITEM_SERVICES_DOCUMENTATION.md](ITEM_SERVICES_DOCUMENTATION.md)
- [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)
- [README_ITEM_SERVICES.md](README_ITEM_SERVICES.md)
