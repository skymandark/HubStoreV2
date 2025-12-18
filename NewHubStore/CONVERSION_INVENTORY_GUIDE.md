# Unit Conversion & Inventory Calculation Services

## Overview
Two new services have been implemented to handle unit conversions and inventory balance calculations:

1. **UnitConversionService** - Converts quantities between different units
2. **InventoryCalculationService** - Calculates inventory balances and validates stock availability

## UnitConversionService

### Methods

#### 1. ConvertToBaseAsync(itemId, unitCode, qtyInput)
Converts a given quantity from a specific unit to the base unit.

**Request:**
```
POST /api/conversion/to-base
Content-Type: application/json

{
  "itemId": 1,
  "unitCode": "BOX",
  "qtyInput": 5
}
```

**Response (Success):**
```json
{
  "itemId": 1,
  "itemCode": "ITEM001",
  "baseQuantity": 50,
  "baseUnitCode": "PCS",
  "conversionFactor": 10,
  "sourceUnitCode": "BOX",
  "sourceQuantity": 5,
  "message": "تم التحويل بنجاح"
}
```

#### 2. BreakdownFromBaseAsync(itemId, baseQty)
Breaks down a base unit quantity to all available units for that item.

**Request:**
```
GET /api/conversion/breakdown?itemId=1&baseQty=100
```

**Response (Success):**
```json
{
  "itemId": 1,
  "itemCode": "ITEM001",
  "baseQuantity": 100,
  "baseUnitCode": "PCS",
  "breakdown": [
    {
      "unitCode": "BOX",
      "quantity": 10,
      "conversionFactor": 10
    },
    {
      "unitCode": "PACK",
      "quantity": 50,
      "conversionFactor": 2
    }
  ]
}
```

#### 3. GetConversionFactorAsync(itemId, unitCode, asOfDate)
Retrieves the conversion factor between a specific unit and the base unit.

**Request:**
```
GET /api/conversion/factor?itemId=1&unitCode=BOX&asOfDate=2024-01-01
```

**Response (Success):**
```json
{
  "itemId": 1,
  "itemCode": "ITEM001",
  "unitCode": "BOX",
  "baseUnitCode": "PCS",
  "conversionFactor": 10,
  "effectiveDate": "2024-01-01",
  "isHistorical": false
}
```

#### 4. ValidateConversionAsync(itemId, unitCode, qty)
Validates that a conversion can be performed safely without precision loss or overflow.

**Request:**
```
GET /api/conversion/validate?itemId=1&unitCode=BOX&qty=50
```

**Response (Success):**
```json
{
  "isValid": true,
  "itemId": 1,
  "unitCode": "BOX",
  "quantity": 50,
  "baseQuantity": 500,
  "precisionLoss": 0,
  "message": "التحويل صحيح"
}
```

---

## InventoryCalculationService

### Methods

#### 1. CalculateBalanceAsync(itemId, branchId, asOfDate)
Calculates the current inventory balance for an item in a specific branch.

**Request:**
```
GET /api/inventory/balance?itemId=1&branchId=1&asOfDate=2024-12-15
```

**Response (Success):**
```json
{
  "itemId": 1,
  "itemCode": "ITEM001",
  "branchId": 1,
  "branchName": "الفرع الرئيسي",
  "quantityBase": 500,
  "baseUnitCode": "PCS",
  "costValue": 5000.50,
  "unitPrice": 10.00,
  "asOfDate": "2024-12-15"
}
```

#### 2. GetOpeningBalanceAsync(itemId, branchId, fiscalYear)
Retrieves the opening balance for an item in a specific fiscal year.

**Request:**
```
GET /api/inventory/opening-balance?itemId=1&branchId=1&fiscalYear=2024
```

**Response (Success):**
```json
{
  "openingBalanceId": 5,
  "itemId": 1,
  "itemCode": "ITEM001",
  "branchId": 1,
  "branchName": "الفرع الرئيسي",
  "fiscalYear": 2024,
  "openingQuantityBase": 100,
  "baseUnitCode": "PCS",
  "costValue": 1000.00,
  "createdDate": "2024-01-01T00:00:00Z",
  "createdBy": "admin"
}
```

#### 3. CalculateBalanceByBranchAsync(branchId, asOfDate)
Calculates inventory balance for all items in a specific branch.

**Request:**
```
GET /api/inventory/branch-balance?branchId=1&asOfDate=2024-12-15
```

**Response (Success):**
```json
{
  "branchId": 1,
  "branchName": "الفرع الرئيسي",
  "asOfDate": "2024-12-15",
  "items": [
    {
      "itemId": 1,
      "itemCode": "ITEM001",
      "quantityBase": 500,
      "baseUnitCode": "PCS",
      "costValue": 5000.50
    },
    {
      "itemId": 2,
      "itemCode": "ITEM002",
      "quantityBase": 200,
      "baseUnitCode": "KG",
      "costValue": 4000.00
    }
  ],
  "totalCostValue": 9000.50
}
```

#### 4. GetMovementImpactAsync(movementId)
Determines the impact of a movement (increase/decrease) and calculates affected quantities.

**Request:**
```
GET /api/inventory/movement-impact?movementId=10
```

**Response (Success):**
```json
{
  "movementId": 10,
  "movementCode": "MOV-001",
  "movementType": "Receipt",
  "impact": "Increase",
  "totalQuantityBase": 100,
  "affectedItems": [
    {
      "itemId": 1,
      "itemCode": "ITEM001",
      "quantityBase": 100,
      "branchId": 1,
      "branchName": "الفرع الرئيسي"
    }
  ]
}
```

#### 5. ValidateStockAvailabilityAsync(itemId, branchId, qtyBase)
Validates if sufficient quantity is available for an outbound movement.

**Request:**
```
GET /api/inventory/validate-availability?itemId=1&branchId=1&qtyBase=50
```

**Response (Success - Stock Available):**
```json
{
  "isAvailable": true,
  "itemId": 1,
  "itemCode": "ITEM001",
  "branchId": 1,
  "requiredQuantity": 50,
  "availableQuantity": 500,
  "remainingQuantity": 450,
  "message": "المخزون كافي"
}
```

**Response (Failure - Stock Not Available):**
```json
{
  "isAvailable": false,
  "itemId": 1,
  "itemCode": "ITEM001",
  "branchId": 1,
  "requiredQuantity": 100,
  "availableQuantity": 50,
  "shortfall": 50,
  "message": "المخزون غير كافي"
}
```

---

## Integration Points

### Controllers
- **ConversionController** - `/api/conversion/*`
- **InventoryController** - `/api/inventory/*`

### Service Registration
Both services are automatically registered in `Program.cs`:
```csharp
builder.Services.AddScoped<IUnitConversionService, UnitConversionService>();
builder.Services.AddScoped<IInventoryCalculationService, InventoryCalculationService>();
```

### Authorization
All endpoints require `[Authorize]` attribute. Include Bearer token in header:
```
Authorization: Bearer {token}
```

### Error Handling
All endpoints return standardized error responses:

**400 Bad Request:**
```json
{
  "error": "Invalid request",
  "message": "الطلب غير صحيح",
  "details": "ItemId must be greater than 0"
}
```

**404 Not Found:**
```json
{
  "error": "Item not found",
  "message": "المنتج غير موجود",
  "itemId": 999
}
```

**500 Internal Server Error:**
```json
{
  "error": "Internal server error",
  "message": "حدث خطأ في الخادم",
  "details": "An unexpected error occurred"
}
```

---

## Common Use Cases

### 1. Convert Purchase Order Quantity to Inventory
```csharp
// User receives 5 boxes of item ITEM001
var result = await conversionService.ConvertToBaseAsync(1, "BOX", 5);
// Result: 50 PCS (pieces)
```

### 2. Check Stock Before Sale
```csharp
// Customer wants 30 pieces
var isAvailable = await inventoryService.ValidateStockAvailabilityAsync(1, 1, 30);
if (isAvailable.IsAvailable) {
    // Proceed with sale
}
```

### 3. Month-End Inventory Report
```csharp
// Get all items in branch 1 as of month end
var balance = await inventoryService.CalculateBalanceByBranchAsync(1, new DateTime(2024, 12, 31));
```

### 4. Opening Balance Setup
```csharp
// Get opening balance for fiscal year 2024
var opening = await inventoryService.GetOpeningBalanceAsync(1, 1, 2024);
```

---

## Best Practices

1. **Always validate conversions** - Use `ValidateConversionAsync` before performing conversions
2. **Check stock before movements** - Use `ValidateStockAvailabilityAsync` before issuing items
3. **Use asOfDate for reporting** - All balance queries accept asOfDate for historical reporting
4. **Handle errors gracefully** - Wrap service calls in try-catch blocks
5. **Log important operations** - Services use ILogger for audit trail

---

## Dependencies

- **Entity Framework Core** - Database access
- **ApplicationDbContext** - Database context
- Domain Models: Item, ItemUnit, Movement, MovementLine, Branch, OpeningBalance, FiscalYear
- **ILogger<T>** - Logging

---

## Testing Checklist

- [ ] Test unit conversion between different units
- [ ] Test breakdown from base quantity
- [ ] Test historical conversion factors
- [ ] Test inventory balance calculation
- [ ] Test branch-wide inventory report
- [ ] Test movement impact determination
- [ ] Test stock availability validation with sufficient stock
- [ ] Test stock availability validation with insufficient stock
- [ ] Test error cases (invalid item, invalid branch, etc.)
- [ ] Test authorization on all endpoints

