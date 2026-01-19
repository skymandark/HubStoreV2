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
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;

        public ItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new item
        /// </summary>
        public async Task<ItemViewModel> CreateItemAsync(CreateItemViewModel itemDto)
        {
            try
            {
                // Validate ItemCode uniqueness
                var existingItem = await _context.Items
                    .Where(i => i.ItemCode == itemDto.ItemCode && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingItem != null)
                    throw new InvalidOperationException($"Item code '{itemDto.ItemCode}' already exists.");

                // Validate InternalBarcode uniqueness
                var existingBarcode = await _context.Items
                    .Where(i => i.InternalBarcode == itemDto.InternalBarcode && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (existingBarcode != null)
                    throw new InvalidOperationException($"Internal barcode '{itemDto.InternalBarcode}' already exists.");

                // Validate ParentItemId if provided
                if (itemDto.ParentItemId.HasValue)
                {
                    var parentItem = await _context.Items
                        .Where(i => i.ItemId == itemDto.ParentItemId && !i.IsDeleted)
                        .FirstOrDefaultAsync();

                    if (parentItem == null)
                        throw new InvalidOperationException($"Parent item with ID '{itemDto.ParentItemId}' does not exist.");

                    if (!parentItem.IsParent)
                        throw new InvalidOperationException("Parent item must have IsParent set to true.");
                }

                var item = new Item
                {
                    ItemCode = itemDto.ItemCode,
                    ItemName = itemDto.NameArab, // Use NameArab as the main ItemName for now
                    NameArab = itemDto.NameArab,
                    NameEng = itemDto.NameEng,
                    SectionId = itemDto.SectionId,
                    MainItemId = itemDto.MainItemId,
                    SubItemId = itemDto.SubItemId,
                    BrandId = itemDto.BrandId,
                    ItemTypeId = itemDto.ItemTypeId,
                    BuyMainPackageId = itemDto.BuyMainPackageId,
                    PackageCount = itemDto.PackageCount,
                    BuySubPackageCount = itemDto.BuySubPackageCount,
                    MainUnitCode = itemDto.MainUnitCode,
                    BaseUnitCode = itemDto.BaseUnitCode,
                    ExternalBarcode = itemDto.ExternalBarcode,
                    InternalBarcode = itemDto.InternalBarcode,
                    Notes = itemDto.Notes,
                    CreatedBy = itemDto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                };

                _context.Items.Add(item);
                await _context.SaveChangesAsync();

                return MapToViewModel(item);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error creating item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing item
        /// </summary>
        public async Task<ItemViewModel> UpdateItemAsync(int itemId, UpdateItemViewModel itemDto)
        {
            try
            {
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                    throw new InvalidOperationException($"Item with ID '{itemId}' not found.");

                // Check if new InternalBarcode is unique (if changed)
                if (itemDto.InternalBarcode != item.InternalBarcode)
                {
                    var existingBarcode = await _context.Items
                        .Where(i => i.InternalBarcode == itemDto.InternalBarcode && i.ItemId != itemId && !i.IsDeleted)
                        .FirstOrDefaultAsync();

                    if (existingBarcode != null)
                        throw new InvalidOperationException($"Internal barcode '{itemDto.InternalBarcode}' already exists.");
                }

                item.NameArab = itemDto.NameArab ?? item.NameArab;
                item.NameEng = itemDto.NameEng ?? item.NameEng;
                item.ItemName = itemDto.NameArab ?? item.ItemName;
                item.SectionId = itemDto.SectionId;
                item.MainItemId = itemDto.MainItemId;
                item.SubItemId = itemDto.SubItemId;
                item.BrandId = itemDto.BrandId;
                item.ItemTypeId = itemDto.ItemTypeId;
                item.BuyMainPackageId = itemDto.BuyMainPackageId;
                item.PackageCount = itemDto.PackageCount;
                item.BuySubPackageCount = itemDto.BuySubPackageCount;
                item.MainUnitCode = itemDto.MainUnitCode ?? item.MainUnitCode;
                item.BaseUnitCode = itemDto.BaseUnitCode ?? item.BaseUnitCode;
                item.ExternalBarcode = itemDto.ExternalBarcode ?? item.ExternalBarcode;
                item.InternalBarcode = itemDto.InternalBarcode ?? item.InternalBarcode;
                item.IsActive = itemDto.IsActive;
                item.Notes = itemDto.Notes ?? item.Notes;
                item.ModifiedBy = itemDto.ModifiedBy;
                item.ModifiedAt = DateTime.UtcNow;

                _context.Items.Update(item);
                await _context.SaveChangesAsync();

                return MapToViewModel(item);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets an item by ID
        /// </summary>
        public async Task<ItemViewModel> GetItemAsync(int itemId)
        {
            try
            {
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .Include(i => i.ItemUnits)
                    .FirstOrDefaultAsync();

                if (item == null)
                    throw new InvalidOperationException($"Item with ID '{itemId}' not found.");

                return MapToViewModel(item);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets paginated list of items with filters
        /// </summary>
        public async Task<PaginatedResult<ItemViewModel>> GetItemsAsync(ItemFilterViewModel filters, PaginationViewModel pagination)
        {
            try
            {
                var query = _context.Items
                    .Where(i => !i.IsDeleted)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filters.ItemCode))
                    query = query.Where(i => i.ItemCode.Contains(filters.ItemCode));

                if (!string.IsNullOrEmpty(filters.ItemName))
                    query = query.Where(i => i.ItemName.Contains(filters.ItemName));

                if (filters.IsActive.HasValue)
                    query = query.Where(i => i.IsActive == filters.IsActive.Value);

                if (filters.IsParent.HasValue)
                    query = query.Where(i => i.IsParent == filters.IsParent.Value);

                if (!string.IsNullOrEmpty(filters.Barcode))
                    query = query.Where(i => i.InternalBarcode.Contains(filters.Barcode) || i.ExternalBarcode.Contains(filters.Barcode));

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

                var items = await query
                    .OrderByDescending(i => i.CreatedAt)
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .Include(i => i.ItemUnits)
                    .Include(i => i.Section)
                    .Include(i => i.MainItem)
                    .Include(i => i.SubItem)
                    .Include(i => i.Brand)
                    .Include(i => i.ItemType)
                    .Include(i => i.BuyMainPackage)
                    .ToListAsync();

                return new PaginatedResult<ItemViewModel>
                {
                    Data = items.Select(MapToViewModel).ToList(),
                    TotalCount = totalCount,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalPages = totalPages
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving items: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Soft deletes an item
        /// </summary>
        public async Task<bool> DeleteItemAsync(int itemId)
        {
            try
            {
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .FirstOrDefaultAsync();

                if (item == null)
                    throw new InvalidOperationException($"Item with ID '{itemId}' not found.");

                // Check if item has child items
                var hasChildItems = await _context.Items
                    .Where(i => i.ParentItemId == itemId && !i.IsDeleted)
                    .AnyAsync();

                if (hasChildItems)
                    throw new InvalidOperationException("Cannot delete item that has child items.");

                // Check if item has active movement lines or order lines
                var hasMovementLines = await _context.MovementLines
                    .Where(ml => ml.ItemId == itemId)
                    .AnyAsync();

                var hasOrderLines = await _context.OrderLines
                    .Where(ol => ol.ItemId == itemId)
                    .AnyAsync();

                if (hasMovementLines || hasOrderLines)
                    throw new InvalidOperationException("Cannot delete item that has associated movements or orders.");

                item.IsDeleted = true;
                item.ModifiedAt = DateTime.UtcNow;

                _context.Items.Update(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting item: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets item by barcode (internal or external)
        /// </summary>
        public async Task<ItemViewModel> GetItemByBarcodeAsync(string barcode, bool isInternal)
        {
            try
            {
                if (string.IsNullOrEmpty(barcode))
                    throw new ArgumentException("Barcode cannot be empty.", nameof(barcode));

                Item item;

                if (isInternal)
                {
                    item = await _context.Items
                        .Where(i => i.InternalBarcode == barcode && !i.IsDeleted && i.IsActive)
                        .Include(i => i.ItemUnits)
                        .Include(i => i.Section)
                        .Include(i => i.MainItem)
                        .Include(i => i.SubItem)
                        .Include(i => i.Brand)
                        .Include(i => i.ItemType)
                        .Include(i => i.BuyMainPackage)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    item = await _context.Items
                        .Where(i => i.ExternalBarcode == barcode && !i.IsDeleted && i.IsActive)
                        .Include(i => i.ItemUnits)
                        .Include(i => i.Section)
                        .Include(i => i.MainItem)
                        .Include(i => i.SubItem)
                        .Include(i => i.Brand)
                        .Include(i => i.ItemType)
                        .Include(i => i.BuyMainPackage)
                        .FirstOrDefaultAsync();
                }

                if (item == null)
                    throw new InvalidOperationException($"Item with {(isInternal ? "internal" : "external")} barcode '{barcode}' not found.");

                return MapToViewModel(item);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving item by barcode: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets item hierarchy (tree structure)
        /// </summary>
        public async Task<ItemHierarchyViewModel> GetItemHierarchyAsync()
        {
            try
            {
                var items = await _context.Items
                    .Where(i => !i.IsDeleted && i.IsActive)
                    .ToListAsync();

                // Build root nodes (items without parent or with non-existent parent)
                var rootItems = items
                    .Where(i => !i.ParentItemId.HasValue || !items.Any(p => p.ItemId == i.ParentItemId))
                    .ToList();

                // Create a root node to hold all root items
                var rootHierarchy = new ItemHierarchyViewModel
                {
                    ItemId = 0,
                    ItemCode = "ROOT",
                    ItemName = "Item Hierarchy",
                    ChildItems = rootItems
                        .Select(i => BuildHierarchy(i, items))
                        .ToList()
                };

                return rootHierarchy;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving item hierarchy: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validates if an item can be used in movements
        /// </summary>
        public async Task<ValidationResult> ValidateItemForMovementAsync(int itemId)
        {
            var result = new ValidationResult { IsValid = true };

            try
            {
                var item = await _context.Items
                    .Where(i => i.ItemId == itemId && !i.IsDeleted)
                    .Include(i => i.ItemUnits)
                    .FirstOrDefaultAsync();

                if (item == null)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Item with ID '{itemId}' not found.");
                    return result;
                }

                if (!item.IsActive)
                {
                    result.IsValid = false;
                    result.Errors.Add("Item is not active.");
                }

                if (!item.ItemUnits.Any())
                {
                    result.IsValid = false;
                    result.Errors.Add("Item must have at least one unit defined.");
                }

                if (string.IsNullOrEmpty(item.BaseUnitCode))
                {
                    result.IsValid = false;
                    result.Errors.Add("Item must have a base unit code defined.");
                }

                result.Data["ItemId"] = item.ItemId;
                result.Data["ItemCode"] = item.ItemCode;
                result.Data["ItemName"] = item.ItemName;

                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add($"Error validating item: {ex.Message}");
                return result;
            }
        }

        // Helper Methods
        private ItemViewModel MapToViewModel(Item item)
        {
            return new ItemViewModel
            {
                ItemId = item.ItemId,
                ItemCode = item.ItemCode,
                ItemName = item.ItemName,
                NameArab = item.NameArab,
                NameEng = item.NameEng,
                SectionId = item.SectionId,
                SectionName = item.Section?.NameArab,
                MainItemId = item.MainItemId,
                MainItemName = item.MainItem?.NameArab,
                SubItemId = item.SubItemId,
                SubItemName = item.SubItem?.NameArab,
                BrandId = item.BrandId,
                BrandName = item.Brand?.NameArab,
                ItemTypeId = item.ItemTypeId,
                ItemTypeName = item.ItemType?.NameArab,
                BuyMainPackageId = item.BuyMainPackageId,
                BuyMainPackageName = item.BuyMainPackage?.NameArab,
                PackageCount = item.PackageCount,
                BuySubPackageCount = item.BuySubPackageCount,
                ParentItemId = item.ParentItemId,
                IsParent = item.IsParent,
                MainUnitCode = item.MainUnitCode,
                BaseUnitCode = item.BaseUnitCode,
                ExternalBarcode = item.ExternalBarcode,
                InternalBarcode = item.InternalBarcode,
                IsActive = item.IsActive,
                Notes = item.Notes,
                CreatedAt = item.CreatedAt
            };
        }

        private ItemHierarchyViewModel BuildHierarchy(Item item, List<Item> allItems)
        {
            var childItems = allItems
                .Where(i => i.ParentItemId == item.ItemId && !i.IsDeleted && i.IsActive)
                .Select(i => BuildHierarchy(i, allItems))
                .ToList();

            return new ItemHierarchyViewModel
            {
                ItemId = item.ItemId,
                ItemCode = item.ItemCode,
                ItemName = item.ItemName,
                ChildItems = childItems
            };
        }
    }
}
