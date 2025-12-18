# Item Management Services Documentation

This document provides comprehensive documentation for the Item and ItemUnit services in the HubStore application.

## Overview

The Item Management system consists of two main services:
1. **ItemService** - Manages items/products with full CRUD operations
2. **ItemUnitService** - Manages units of measurement for items

## Table of Contents

- [ItemService](#itemservice)
- [ItemUnitService](#itemunitservice)
- [API Endpoints](#api-endpoints)
- [Error Handling](#error-handling)
- [Examples](#examples)

---

## ItemService

The `ItemService` handles all operations related to items/products in the inventory system.

### Methods

#### 1. CreateItemAsync(itemDto)
**Purpose:** Creates a new item in the system.

**Input:** `CreateItemViewModel`
```csharp
public class CreateItemViewModel
{
    public string ItemCode { get; set; }           // Unique code for the item
    public string ItemName { get; set; }           // Name of the item
    public int? ParentItemId { get; set; }         // ID of parent item (for hierarchies)
    public bool IsParent { get; set; }             // Is this a parent item
    public string MainUnitCode { get; set; }       // Main unit code
    public string BaseUnitCode { get; set; }       // Base unit code
    public string ExternalBarcode { get; set; }    // External barcode
    public string InternalBarcode { get; set; }    // Internal barcode
    public string Notes { get; set; }              // Notes about the item
    public string CreatedBy { get; set; }          // User creating the item
}
```

**Output:** `ItemViewModel`

**Validations:**
- ItemCode must be unique
- InternalBarcode must be unique
- ParentItemId must reference a valid parent item if provided
- Parent item must have IsParent = true

**Example:**
```csharp
var itemDto = new CreateItemViewModel
{
    ItemCode = "ITEM001",
    ItemName = "Test Item",
    IsParent = false,
    MainUnitCode = "PCE",
    BaseUnitCode = "PCE",
    InternalBarcode = "INT123",
    CreatedBy = "user@example.com"
};

var result = await itemService.CreateItemAsync(itemDto);
```

#### 2. UpdateItemAsync(itemId, itemDto)
**Purpose:** Updates an existing item.

**Input:** 
- `itemId` - ID of the item to update
- `UpdateItemViewModel` - Updated item data

```csharp
public class UpdateItemViewModel
{
    public string ItemName { get; set; }
    public string MainUnitCode { get; set; }
    public string BaseUnitCode { get; set; }
    public string ExternalBarcode { get; set; }
    public string InternalBarcode { get; set; }
    public bool IsActive { get; set; }
    public string Notes { get; set; }
    public string ModifiedBy { get; set; }
}
```

**Output:** `ItemViewModel`

**Example:**
```csharp
var updateDto = new UpdateItemViewModel
{
    ItemName = "Updated Item Name",
    IsActive = true,
    ModifiedBy = "user@example.com"
};

var result = await itemService.UpdateItemAsync(1, updateDto);
```

#### 3. GetItemAsync(itemId)
**Purpose:** Retrieves a specific item by ID.

**Input:** `itemId` - The ID of the item

**Output:** `ItemViewModel`

**Example:**
```csharp
var item = await itemService.GetItemAsync(1);
```

#### 4. GetItemsAsync(filters, pagination)
**Purpose:** Retrieves a paginated list of items with optional filters.

**Input:**
```csharp
public class ItemFilterViewModel
{
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsParent { get; set; }
    public string Barcode { get; set; }
}

public class PaginationViewModel
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
```

**Output:** `PaginatedResult<ItemViewModel>`

**Example:**
```csharp
var filters = new ItemFilterViewModel
{
    ItemCode = "ITEM",
    IsActive = true
};

var pagination = new PaginationViewModel
{
    PageNumber = 1,
    PageSize = 20
};

var result = await itemService.GetItemsAsync(filters, pagination);
```

#### 5. DeleteItemAsync(itemId)
**Purpose:** Soft deletes an item (marks as deleted, doesn't remove from DB).

**Input:** `itemId` - The ID of the item to delete

**Output:** `bool` - Success status

**Validations:**
- Item must not have child items
- Item must not have associated movements or orders

**Example:**
```csharp
var success = await itemService.DeleteItemAsync(1);
```

#### 6. GetItemByBarcodeAsync(barcode, isInternal)
**Purpose:** Retrieves an item by its barcode.

**Input:**
- `barcode` - The barcode to search for
- `isInternal` - Whether to search in internal (true) or external (false) barcodes

**Output:** `ItemViewModel`

**Example:**
```csharp
// Search by internal barcode
var item = await itemService.GetItemByBarcodeAsync("INT123", true);

// Search by external barcode
var item = await itemService.GetItemByBarcodeAsync("EXT456", false);
```

#### 7. GetItemHierarchyAsync()
**Purpose:** Retrieves the complete item hierarchy as a tree structure.

**Input:** None

**Output:** `ItemHierarchyViewModel` - Root node with child items recursively

**Example:**
```csharp
var hierarchy = await itemService.GetItemHierarchyAsync();
// Returns tree structure of all items
```

**Response Structure:**
```json
{
  "itemId": 0,
  "itemCode": "ROOT",
  "itemName": "Item Hierarchy",
  "childItems": [
    {
      "itemId": 1,
      "itemCode": "PARENT001",
      "itemName": "Parent Item",
      "childItems": [
        {
          "itemId": 2,
          "itemCode": "CHILD001",
          "itemName": "Child Item",
          "childItems": []
        }
      ]
    }
  ]
}
```

#### 8. ValidateItemForMovementAsync(itemId)
**Purpose:** Validates if an item can be used in inventory movements.

**Input:** `itemId` - The ID of the item to validate

**Output:** `ValidationResult`

```csharp
public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
    public Dictionary<string, object> Data { get; set; }
}
```

**Validation Checks:**
- Item exists
- Item is active
- Item has at least one unit defined
- Item has a base unit code

**Example:**
```csharp
var validation = await itemService.ValidateItemForMovementAsync(1);
if (validation.IsValid)
{
    // Item is valid for movements
}
else
{
    // Log errors
    foreach (var error in validation.Errors)
    {
        Console.WriteLine(error);
    }
}
```

---

## ItemUnitService

The `ItemUnitService` manages units of measurement associated with items.

### Methods

#### 1. CreateItemUnitAsync(itemId, unitDto)
**Purpose:** Creates a new unit for an item.

**Input:**
- `itemId` - The ID of the item
- `CreateItemUnitViewModel`

```csharp
public class CreateItemUnitViewModel
{
    public string UnitCode { get; set; }              // Code for the unit
    public string UnitName { get; set; }              // Display name
    public decimal ConversionToBase { get; set; }     // Conversion factor
    public int? ParentUnitId { get; set; }            // Parent unit (for hierarchies)
    public bool IsDefaultForDisplay { get; set; }     // Is default unit
    public string CreatedBy { get; set; }             // User creating
}
```

**Output:** `ItemUnitViewModel`

**Validations:**
- Item must exist
- UnitCode must be unique for this item
- ConversionToBase must be > 0
- ParentUnitId must reference a valid unit of the same item

**Example:**
```csharp
var unitDto = new CreateItemUnitViewModel
{
    UnitCode = "PCE",
    UnitName = "Piece",
    ConversionToBase = 1m,
    IsDefaultForDisplay = true,
    CreatedBy = "user@example.com"
};

var result = await itemUnitService.CreateItemUnitAsync(1, unitDto);
```

#### 2. UpdateItemUnitAsync(itemUnitId, unitDto)
**Purpose:** Updates an existing unit.

**Input:**
- `itemUnitId` - The ID of the unit to update
- `UpdateItemUnitViewModel`

```csharp
public class UpdateItemUnitViewModel
{
    public string UnitName { get; set; }
    public decimal ConversionToBase { get; set; }
    public bool IsDefaultForDisplay { get; set; }
    public bool IsActive { get; set; }
    public string ModifiedBy { get; set; }
}
```

**Output:** `ItemUnitViewModel`

**Side Effects:**
- Creates an entry in `ItemUnitHistory` to track the change

**Example:**
```csharp
var updateDto = new UpdateItemUnitViewModel
{
    UnitName = "Updated Name",
    ConversionToBase = 2m,
    IsActive = true,
    ModifiedBy = "user@example.com"
};

var result = await itemUnitService.UpdateItemUnitAsync(1, updateDto);
```

#### 3. GetItemUnitsAsync(itemId)
**Purpose:** Retrieves all active units for an item.

**Input:** `itemId` - The ID of the item

**Output:** `List<ItemUnitViewModel>`

**Ordering:** By IsDefaultForDisplay (desc), then by CreatedAt (asc)

**Example:**
```csharp
var units = await itemUnitService.GetItemUnitsAsync(1);
```

#### 4. DeleteItemUnitAsync(itemUnitId)
**Purpose:** Soft deletes a unit (marks as inactive).

**Input:** `itemUnitId` - The ID of the unit to delete

**Output:** `bool` - Success status

**Validations:**
- Unit must not be used in movements
- Unit must not be used in orders
- Unit must not have child units

**Example:**
```csharp
var success = await itemUnitService.DeleteItemUnitAsync(1);
```

#### 5. GetUnitConversionHistoryAsync(itemId, unitCode)
**Purpose:** Retrieves the conversion history for a specific unit.

**Input:**
- `itemId` - The ID of the item
- `unitCode` - The code of the unit

**Output:** `List<UnitConversionHistoryViewModel>`

**Example:**
```csharp
var history = await itemUnitService.GetUnitConversionHistoryAsync(1, "PCE");
```

**Response:**
```json
[
  {
    "itemUnitHistoryId": 1,
    "itemId": 1,
    "unitCode": "PCE",
    "oldConversionToBase": 1.0,
    "newConversionToBase": 2.0,
    "changedDate": "2025-12-14T10:30:00",
    "changedByUser": "user@example.com",
    "notes": "Updated unit conversion from 1.0 to 2.0"
  }
]
```

#### 6. ValidateUnitConversionAsync(itemId, unitCode)
**Purpose:** Validates unit conversion values and hierarchy.

**Input:**
- `itemId` - The ID of the item
- `unitCode` - The code of the unit

**Output:** `ValidationResult`

**Validation Checks:**
- Item exists
- Unit exists
- ConversionToBase > 0
- No circular references in unit hierarchy

**Example:**
```csharp
var validation = await itemUnitService.ValidateUnitConversionAsync(1, "PCE");
if (validation.IsValid)
{
    // Unit conversion is valid
}
```

---

## API Endpoints

### Item Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/item/create` | Create a new item |
| PUT | `/api/item/update/{itemId}` | Update an item |
| GET | `/api/item/get/{itemId}` | Get a specific item |
| GET | `/api/item/list` | Get paginated list of items |
| DELETE | `/api/item/delete/{itemId}` | Delete an item |
| GET | `/api/item/barcode/{barcode}` | Get item by barcode |
| GET | `/api/item/hierarchy` | Get item hierarchy |
| GET | `/api/item/validate-movement/{itemId}` | Validate item for movement |

### ItemUnit Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/item/{itemId}/unit/create` | Create item unit |
| PUT | `/api/item/unit/update/{itemUnitId}` | Update item unit |
| GET | `/api/item/{itemId}/units` | Get all units for item |
| DELETE | `/api/item/unit/delete/{itemUnitId}` | Delete item unit |
| GET | `/api/item/{itemId}/unit/{unitCode}/history` | Get conversion history |
| GET | `/api/item/{itemId}/unit/{unitCode}/validate` | Validate unit conversion |

---

## Error Handling

All services follow a consistent error handling pattern:

### Common Exceptions

1. **InvalidOperationException** - Business logic validation failures
   - Status Code: 400 Bad Request
   - Example: Item code already exists

2. **ArgumentException** - Invalid input parameters
   - Status Code: 400 Bad Request
   - Example: Empty barcode

3. **ApplicationException** - Unexpected application errors
   - Status Code: 500 Internal Server Error
   - Includes detailed error message

### Response Format

**Success Response:**
```json
{
  "itemId": 1,
  "itemCode": "ITEM001",
  "itemName": "Test Item",
  ...
}
```

**Error Response:**
```json
{
  "message": "Item code 'ITEM001' already exists."
}
```

**Validation Result Response:**
```json
{
  "isValid": false,
  "errors": [
    "Item must have at least one unit defined.",
    "Item must have a base unit code defined."
  ],
  "data": {
    "ItemId": 1,
    "ItemCode": "ITEM001"
  }
}
```

---

## Examples

### Complete Item Creation Flow

```csharp
// 1. Create item
var itemDto = new CreateItemViewModel
{
    ItemCode = "PROD001",
    ItemName = "Product 1",
    IsParent = false,
    MainUnitCode = "PCE",
    BaseUnitCode = "PCE",
    InternalBarcode = "INT-PROD001",
    ExternalBarcode = "EXT-PROD001",
    Notes = "Sample product",
    CreatedBy = "admin@example.com"
};

var item = await itemService.CreateItemAsync(itemDto);
Console.WriteLine($"Created item: {item.ItemId}");

// 2. Add units
var unit1 = new CreateItemUnitViewModel
{
    UnitCode = "PCE",
    UnitName = "Piece",
    ConversionToBase = 1m,
    IsDefaultForDisplay = true,
    CreatedBy = "admin@example.com"
};

var createdUnit = await itemUnitService.CreateItemUnitAsync(item.ItemId, unit1);

// 3. Validate for movement
var validation = await itemService.ValidateItemForMovementAsync(item.ItemId);
if (validation.IsValid)
{
    Console.WriteLine("Item is ready for use in movements");
}
```

### Searching Items

```csharp
// Search by barcode
var item = await itemService.GetItemByBarcodeAsync("INT-PROD001", isInternal: true);

// Search with filters and pagination
var filter = new ItemFilterViewModel
{
    ItemCode = "PROD",
    IsActive = true
};

var pagination = new PaginationViewModel
{
    PageNumber = 1,
    PageSize = 20
};

var results = await itemService.GetItemsAsync(filter, pagination);
Console.WriteLine($"Found {results.TotalCount} items");
```

### Managing Hierarchies

```csharp
// Get hierarchy tree
var hierarchy = await itemService.GetItemHierarchyAsync();

// Traverse tree
void PrintHierarchy(ItemHierarchyViewModel node, int level = 0)
{
    var indent = new string(' ', level * 2);
    Console.WriteLine($"{indent}- {node.ItemCode}: {node.ItemName}");
    
    foreach (var child in node.ChildItems)
    {
        PrintHierarchy(child, level + 1);
    }
}

PrintHierarchy(hierarchy);
```

---

## Best Practices

1. **Always Validate Before Operations**
   - Use `ValidateItemForMovementAsync()` before using items in movements
   - Use `ValidateUnitConversionAsync()` before updating units

2. **Handle Errors Gracefully**
   - Catch specific exception types
   - Log errors for debugging
   - Return meaningful error messages to clients

3. **Use Pagination**
   - Always paginate when retrieving lists
   - Limit page size to prevent performance issues

4. **Set Audit Information**
   - Always provide `CreatedBy` and `ModifiedBy` from current user
   - Use `User.Identity.Name` in controllers

5. **Validate Unique Constraints**
   - Check ItemCode and InternalBarcode uniqueness before creating items
   - Check UnitCode uniqueness per item before creating units

---

## Dependency Injection Setup

Add to Program.cs:

```csharp
// Register Item Services
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemUnitService, ItemUnitService>();
```

---

## Database Considerations

- Items support soft deletion (IsDeleted flag)
- ItemUnit changes are tracked in ItemUnitHistory
- Parent-child relationships are maintained via foreign keys
- All timestamps use UTC (DateTime.UtcNow)

---

## Version History

- **v1.0** - Initial implementation with all core methods
- Date: December 14, 2025

---

For questions or issues, please contact the development team.
