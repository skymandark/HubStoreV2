using System;
using Core.Domin;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Infrastructure.DataSeeds
{
    public static class ItemSeed
    {
        public static void SeedItems(ApplicationDbContext context)
        {
            // Seed related entities first
            SeedSections(context);
            SeedMainItems(context);
            SeedSubItems(context);
            SeedPackages(context);
            SeedItemTypes(context);

            var mainBranchId = context.Branches.Where(b => b.Code == "BR001").Select(b => b.BranchId).FirstOrDefault();
            var alexBranchId = context.Branches.Where(b => b.Code == "BR002").Select(b => b.BranchId).FirstOrDefault();
            var gizaBranchId = context.Branches.Where(b => b.Code == "BR003").Select(b => b.BranchId).FirstOrDefault();

            var sectionId = context.Sections.Where(s => s.NameEng == "Electronics").Select(s => s.SectionId).FirstOrDefault();
            var mainItemId = context.MainItems.Where(m => m.NameEng == "Computer Equipment").Select(m => m.MainItemId).FirstOrDefault();

            var pkgPcsId = context.Packages.Where(p => p.Code == "PCS").Select(p => p.PackageId).FirstOrDefault();
            var itemTypeProductId = context.ItemTypes.Where(t => t.Code == "PRD").Select(t => t.ItemTypeId).FirstOrDefault();

            var subLaptopId = context.SubItems.Where(s => s.NameEng == "Laptops").Select(s => s.SubItemId).FirstOrDefault();
            var subMouseId = context.SubItems.Where(s => s.NameEng == "Mouse").Select(s => s.SubItemId).FirstOrDefault();
            var subKeyboardId = context.SubItems.Where(s => s.NameEng == "Keyboard").Select(s => s.SubItemId).FirstOrDefault();
            var subMonitorsId = context.SubItems.Where(s => s.NameEng == "Monitors").Select(s => s.SubItemId).FirstOrDefault();
            var subPrintersId = context.SubItems.Where(s => s.NameEng == "Printers").Select(s => s.SubItemId).FirstOrDefault();

            if (mainBranchId == 0 || alexBranchId == 0 || gizaBranchId == 0 || sectionId == 0 || mainItemId == 0 || pkgPcsId == 0 || itemTypeProductId == 0
                || subLaptopId == 0 || subMouseId == 0 || subKeyboardId == 0 || subMonitorsId == 0 || subPrintersId == 0)
            {
                throw new InvalidOperationException("Seeding prerequisites are missing. Ensure BranchSeed runs before ItemSeed and related reference data exists.");
            }

            var items = new[]
            {
                new Item
                {
                    NameArab = "لابتوب ديل",
                    NameEng = "Dell Laptop",
                    Description = "Dell Inspiron 15.6 inch Laptop",
                    ItemCode = "DL001",
                    InternalBarcode = "DL001",
                    ExternalBarcode = "DL001",
                    ItemName = "Dell Laptop",
                    MainUnitCode = "PCS",
                    BaseUnitCode = "PCS",
                    Notes = "High performance laptop",
                    AverageCost = 15000.00m,
                    Cost = 14500.00m,
                    BranchId = mainBranchId,
                    SectionId = sectionId,
                    MainItemId = mainItemId,
                    SubItemId = subLaptopId,
                    ItemTypeId = itemTypeProductId,
                    SellMainPackageId = pkgPcsId,
                    BuyMainPackageId = pkgPcsId,
                    StockAccount = "STOCK",
                    TradeAccount = "TRADE",
                    VatAccount = "VAT",
                    TotalCost = 14500.00m,
                    CustomerPrice = 18000.00m,
                    BasicPrice = 16000.00m,
                    CostWithoutVat = 14500.00m,
                    ProfitAmount = 3500.00m,
                    UnitPrice = 18000.00m,
                    Locked = false,
                    IsActive = true,
                    IsPos = true,
                    IsOnline = true,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new Item
                {
                    NameArab = "ماوس لوجيتيك",
                    NameEng = "Logitech Mouse",
                    Description = "Logitech Wireless Mouse",
                    ItemCode = "LM001",
                    InternalBarcode = "LM001",
                    ExternalBarcode = "LM001",
                    ItemName = "Logitech Mouse",
                    MainUnitCode = "PCS",
                    BaseUnitCode = "PCS",
                    Notes = "Wireless optical mouse",
                    AverageCost = 250.00m,
                    Cost = 230.00m,
                    BranchId = mainBranchId,
                    SectionId = sectionId,
                    MainItemId = mainItemId,
                    SubItemId = subMouseId,
                    ItemTypeId = itemTypeProductId,
                    SellMainPackageId = pkgPcsId,
                    BuyMainPackageId = pkgPcsId,
                    StockAccount = "STOCK",
                    TradeAccount = "TRADE",
                    VatAccount = "VAT",
                    TotalCost = 230.00m,
                    CustomerPrice = 350.00m,
                    BasicPrice = 280.00m,
                    CostWithoutVat = 230.00m,
                    ProfitAmount = 120.00m,
                    UnitPrice = 350.00m,
                    Locked = false,
                    IsActive = true,
                    IsPos = true,
                    IsOnline = true,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new Item
                {
                    NameArab = "كيبورد ميكانيكي",
                    NameEng = "Mechanical Keyboard",
                    Description = "RGB Mechanical Gaming Keyboard",
                    ItemCode = "MK001",
                    InternalBarcode = "MK001",
                    ExternalBarcode = "MK001",
                    ItemName = "Mechanical Keyboard",
                    MainUnitCode = "PCS",
                    BaseUnitCode = "PCS",
                    Notes = "Gaming mechanical keyboard with RGB",
                    AverageCost = 800.00m,
                    Cost = 750.00m,
                    BranchId = alexBranchId,
                    SectionId = sectionId,
                    MainItemId = mainItemId,
                    SubItemId = subKeyboardId,
                    ItemTypeId = itemTypeProductId,
                    SellMainPackageId = pkgPcsId,
                    BuyMainPackageId = pkgPcsId,
                    StockAccount = "STOCK",
                    TradeAccount = "TRADE",
                    VatAccount = "VAT",
                    TotalCost = 750.00m,
                    CustomerPrice = 1200.00m,
                    BasicPrice = 950.00m,
                    CostWithoutVat = 750.00m,
                    ProfitAmount = 450.00m,
                    UnitPrice = 1200.00m,
                    Locked = false,
                    IsActive = true,
                    IsPos = true,
                    IsOnline = true,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new Item
                {
                    NameArab = "شاشة سامسونج",
                    NameEng = "Samsung Monitor",
                    Description = "Samsung 24 inch LED Monitor",
                    ItemCode = "SM001",
                    InternalBarcode = "SM001",
                    ExternalBarcode = "SM001",
                    ItemName = "Samsung Monitor",
                    MainUnitCode = "PCS",
                    BaseUnitCode = "PCS",
                    Notes = "Full HD LED monitor",
                    AverageCost = 2200.00m,
                    Cost = 2000.00m,
                    BranchId = alexBranchId,
                    SectionId = sectionId,
                    MainItemId = mainItemId,
                    SubItemId = subMonitorsId,
                    ItemTypeId = itemTypeProductId,
                    SellMainPackageId = pkgPcsId,
                    BuyMainPackageId = pkgPcsId,
                    StockAccount = "STOCK",
                    TradeAccount = "TRADE",
                    VatAccount = "VAT",
                    TotalCost = 2000.00m,
                    CustomerPrice = 2800.00m,
                    BasicPrice = 2400.00m,
                    CostWithoutVat = 2000.00m,
                    ProfitAmount = 800.00m,
                    UnitPrice = 2800.00m,
                    Locked = false,
                    IsActive = true,
                    IsPos = true,
                    IsOnline = true,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new Item
                {
                    NameArab = "طابعة كانون",
                    NameEng = "Canon Printer",
                    Description = "Canon Color Laser Printer",
                    ItemCode = "CP001",
                    InternalBarcode = "CP001",
                    ExternalBarcode = "CP001",
                    ItemName = "Canon Printer",
                    MainUnitCode = "PCS",
                    BaseUnitCode = "PCS",
                    Notes = "Multi-function color printer",
                    AverageCost = 3500.00m,
                    Cost = 3200.00m,
                    BranchId = gizaBranchId,
                    SectionId = sectionId,
                    MainItemId = mainItemId,
                    SubItemId = subPrintersId,
                    ItemTypeId = itemTypeProductId,
                    SellMainPackageId = pkgPcsId,
                    BuyMainPackageId = pkgPcsId,
                    StockAccount = "STOCK",
                    TradeAccount = "TRADE",
                    VatAccount = "VAT",
                    TotalCost = 3200.00m,
                    CustomerPrice = 4500.00m,
                    BasicPrice = 3800.00m,
                    CostWithoutVat = 3200.00m,
                    ProfitAmount = 1300.00m,
                    UnitPrice = 4500.00m,
                    Locked = false,
                    IsActive = true,
                    IsPos = true,
                    IsOnline = false,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                }
            };

            foreach (var item in items)
            {
                var exists = context.Items.Any(i => i.ItemCode == item.ItemCode);
                if (!exists)
                {
                    context.Items.Add(item);
                }
            }

            context.SaveChanges();
        }

        private static void SeedSections(ApplicationDbContext context)
        {
            var exists = context.Sections.Any(s => s.NameEng == "Electronics");
            if (exists)
            {
                return;
            }

            context.Sections.Add(new Section
            {
                NameArab = "إلكترونيات",
                NameEng = "Electronics",
                Description = "Electronic devices and accessories",
                CreatedBy = "System",
                ModifiedBy = "System",
                ModifiedAt = DateTime.UtcNow
            });

            context.SaveChanges();
        }

        private static void SeedMainItems(ApplicationDbContext context)
        {
            var exists = context.MainItems.Any(m => m.NameEng == "Computer Equipment");
            if (exists)
            {
                return;
            }

            var sectionId = context.Sections.Where(s => s.NameEng == "Electronics").Select(s => s.SectionId).FirstOrDefault();
            if (sectionId == 0)
            {
                throw new InvalidOperationException("Cannot seed MainItems because Electronics section does not exist.");
            }

            context.MainItems.Add(new MainItem
            {
                SectionId = sectionId,
                NameArab = "أجهزة كمبيوتر",
                NameEng = "Computer Equipment",
                Description = "Computer hardware and peripherals",
                CreatedBy = "System",
                ModifiedBy = "System",
                ModifiedAt = DateTime.UtcNow
            });

            context.SaveChanges();
        }

        private static void SeedSubItems(ApplicationDbContext context)
        {
            var mainItemId = context.MainItems.Where(m => m.NameEng == "Computer Equipment").Select(m => m.MainItemId).FirstOrDefault();
            if (mainItemId == 0)
            {
                throw new InvalidOperationException("Cannot seed SubItems because Computer Equipment main item does not exist.");
            }

            var subItems = new[]
            {
                new SubItem
                {
                    MainItemId = mainItemId,
                    NameArab = "لابتوب",
                    NameEng = "Laptops",
                    Description = "Laptop computers",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new SubItem
                {
                    MainItemId = mainItemId,
                    NameArab = "ماوس",
                    NameEng = "Mouse",
                    Description = "Computer mouse devices",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new SubItem
                {
                    MainItemId = mainItemId,
                    NameArab = "كيبورد",
                    NameEng = "Keyboard",
                    Description = "Computer keyboards",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new SubItem
                {
                    MainItemId = mainItemId,
                    NameArab = "شاشات",
                    NameEng = "Monitors",
                    Description = "Computer monitors",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new SubItem
                {
                    MainItemId = mainItemId,
                    NameArab = "طابعات",
                    NameEng = "Printers",
                    Description = "Computer printers",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                }
            };

            foreach (var subItem in subItems)
            {
                var exists = context.SubItems.Any(s => s.NameEng == subItem.NameEng);
                if (!exists)
                {
                    context.SubItems.Add(subItem);
                }
            }

            context.SaveChanges();
        }

        private static void SeedPackages(ApplicationDbContext context)
        {
            var packages = new[]
            {
                new Package
                {
                    NameArab = "قطعة",
                    NameEng = "Piece",
                    Code = "PCS",
                    Description = "Single unit package",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new Package
                {
                    NameArab = "صندوق",
                    NameEng = "Box",
                    Code = "BOX",
                    Description = "Box package",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                }
            };

            foreach (var package in packages)
            {
                var exists = context.Packages.Any(p => p.Code == package.Code);
                if (!exists)
                {
                    context.Packages.Add(package);
                }
            }

            context.SaveChanges();
        }

        private static void SeedItemTypes(ApplicationDbContext context)
        {
            var itemTypes = new[]
            {
                new ItemType
                {
                    NameArab = "منتج",
                    NameEng = "Product",
                    Code = "PRD",
                    Description = "Standard product type",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                },
                new ItemType
                {
                    NameArab = "خدمة",
                    NameEng = "Service",
                    Code = "SVC",
                    Description = "Service type",
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow
                }
            };

            foreach (var itemType in itemTypes)
            {
                var exists = context.ItemTypes.Any(t => t.Code == itemType.Code);
                if (!exists)
                {
                    context.ItemTypes.Add(itemType);
                }
            }

            context.SaveChanges();
        }
    }
}
