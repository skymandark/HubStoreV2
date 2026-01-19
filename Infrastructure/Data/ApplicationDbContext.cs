using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser,IdentityRole,string>
    {
        public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<ItemUnitHistory> ItemUnitHistories { get; set; }
        public DbSet<OpeningBalance> OpeningBalances { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }

        // Movements
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<MovementLine> MovementLines { get; set; }
        public DbSet<MovementType> MovementTypes { get; set; }

        // Orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }

        // Transfer Orders
        public DbSet<TransferOrderHeader> TransferOrderHeaders { get; set; }
        public DbSet<TransferOrderDetail> TransferOrderDetails { get; set; }
        public DbSet<ShipmentType> ShipmentTypes { get; set; }
        public DbSet<TransferOrderStatus> TransferOrderStatuses { get; set; }

        // Direct Receipts
        public DbSet<DirectReceiptHeader> DirectReceiptHeaders { get; set; }
        public DbSet<DirectReceiptDetail> DirectReceiptDetails { get; set; }
        public DbSet<SupplierInvoiceHeader> SupplierInvoiceHeaders { get; set; }
        public DbSet<SupplierInvoiceDetail> SupplierInvoiceDetails { get; set; }

        // Purchase Orders
        public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        // Stock In
        public DbSet<StockInHeader> StockInHeaders { get; set; }
        public DbSet<StockInDetail> StockInDetails { get; set; }

        // Stock Out
        public DbSet<StockOutHeader> StockOutHeaders { get; set; }
        public DbSet<StockOutDetail> StockOutDetails { get; set; }

        // Return Orders
        public DbSet<ReturnOrderHeader> ReturnOrderHeaders { get; set; }
        public DbSet<ReturnOrderDetail> ReturnOrderDetails { get; set; }

        // Stock Out Returns
        public DbSet<StockOutReturnHeader> StockOutReturnHeaders { get; set; }
        public DbSet<StockOutReturnDetail> StockOutReturnDetails { get; set; }

        // Reference Tables
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
        public DbSet<CostingMethod> CostingMethods { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Branch> Branches { get; set; }

        // Item Classifications
        public DbSet<Section> Sections { get; set; }
        public DbSet<MainItem> MainItems { get; set; }
        public DbSet<SubItem> SubItems { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<TradeType> TradeTypes { get; set; }

        // Additional Item Related
        public DbSet<ImportType> ImportTypes { get; set; }
        public DbSet<ItemFlag> ItemFlags { get; set; }
        public DbSet<ItemPackage> ItemPackages { get; set; }
        public DbSet<MedicineForm> MedicineForms { get; set; }
        public DbSet<ScientificGroup> ScientificGroups { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        // Settings
        public DbSet<Setting_flag_details> Setting_flag_details { get; set; }
        public DbSet<Setting_flag_master> Setting_flag_masters { get; set; }
        //public DbSet<Setting_flags_value> Setting_flags_values { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<FlagType> FlagTypes { get; set; }

        // Approval Entities
    

        // Approval & Audit
        public DbSet<ApprovalHistory> ApprovalHistories { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<AttemptLog> AttemptLogs { get; set; }
        public DbSet<ExportLog> ExportLogs { get; set; }

        public DbSet<ApprovalChain> ApprovalChains { get; set; }
        public DbSet<PendingApproval> PendingApprovals { get; set; }

        // User Features
        public DbSet<SavedView> SavedViews { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Item Relationships
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Section)
                .WithMany()
                .HasForeignKey(i => i.SectionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.MainItem)
                .WithMany()
                .HasForeignKey(i => i.MainItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.SubItem)
                .WithMany()
                .HasForeignKey(i => i.SubItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Vendor)
                .WithMany()
                .HasForeignKey(i => i.VendorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Brand)
                .WithMany()
                .HasForeignKey(i => i.BrandId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemType)
                .WithMany()
                .HasForeignKey(i => i.ItemTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemCategory)
                .WithMany()
                .HasForeignKey(i => i.ItemCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.SellMainPackage)
                .WithMany(p => p.SellMainItems)
                .HasForeignKey(i => i.SellMainPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.SellSubPackage)
                .WithMany(p => p.SellSubItems)
                .HasForeignKey(i => i.SellSubPackageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.BuyMainPackage)
                .WithMany(p => p.BuyMainItems)
                .HasForeignKey(i => i.BuyMainPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.BuySubPackage)
                .WithMany(p => p.BuySubItems)
                .HasForeignKey(i => i.BuySubPackageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.BuyTradeType)
                .WithMany(tt => tt.BuyItems)
                .HasForeignKey(i => i.BuyTradeTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.SellTradeType)
                .WithMany(tt => tt.SellItems)
                .HasForeignKey(i => i.SellTradeTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ParentItem)
                .WithMany(i => i.ChildItems)
                .HasForeignKey(i => i.ParentItemId)
                .OnDelete(DeleteBehavior.NoAction);

            // Receipt Relationships
            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.OrderLine)
                .WithMany()
                .HasForeignKey(r => r.OrderLineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Movement)
                .WithMany()
                .HasForeignKey(r => r.MovementId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order Relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.OrderType)
                .WithMany()
                .HasForeignKey(o => o.OrderTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Supplier)
                .WithMany()
                .HasForeignKey(o => o.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BranchFrom)
                .WithMany(b => b.OrdersFrom)
                .HasForeignKey(o => o.BranchFromId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BranchTo)
                .WithMany(b => b.OrdersTo)
                .HasForeignKey(o => o.BranchToId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.RequestedByUser)
                .WithMany()
                .HasForeignKey(o => o.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ApprovalStatus)
                .WithMany(ast => ast.Orders)
                .HasForeignKey(o => o.ApprovalStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderLine Relationships
            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Item)
                .WithMany(i => i.OrderLines)
                .HasForeignKey(ol => ol.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Movement Relationships
            modelBuilder.Entity<Movement>()
                .HasOne(m => m.MovementType)
                .WithMany()
                .HasForeignKey(m => m.MovementTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.BranchFrom)
                .WithMany(b => b.MovementsFrom)
                .HasForeignKey(m => m.BranchFromId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.BranchTo)
                .WithMany(b => b.MovementsTo)
                .HasForeignKey(m => m.BranchToId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.Supplier)
                .WithMany(s => s.Movements)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.ApprovalStatus)
                .WithMany(ast => ast.Movements)
                .HasForeignKey(m => m.ApprovalStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // MovementLine Relationships
            modelBuilder.Entity<MovementLine>()
                .HasOne(ml => ml.Movement)
                .WithMany(m => m.MovementLines)
                .HasForeignKey(ml => ml.MovementId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovementLine>()
                .HasOne(ml => ml.Item)
                .WithMany(i => i.MovementLines)
                .HasForeignKey(ml => ml.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovementLine>()
                .HasOne(ml => ml.Branch)
                .WithMany(b => b.MovementLines)
                .HasForeignKey(ml => ml.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovementLine>()
                .HasOne(ml => ml.MovementType)
                .WithMany(mt => mt.MovementLines)
                .HasForeignKey(ml => ml.MovementTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // TransferOrderHeader Relationships
            modelBuilder.Entity<TransferOrderHeader>()
                .HasOne(toh => toh.FromBranch)
                .WithMany()
                .HasForeignKey(toh => toh.FromBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferOrderHeader>()
                .HasOne(toh => toh.ToBranch)
                .WithMany()
                .HasForeignKey(toh => toh.ToBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferOrderHeader>()
                .HasOne(toh => toh.TransferOrderStatus)
                .WithMany()
                .HasForeignKey(toh => toh.TransferOrderStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferOrderHeader>()
                .HasOne(toh => toh.ShipmentType)
                .WithMany()
                .HasForeignKey(toh => toh.ShipmentTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            // TransferOrderDetail Relationships
            modelBuilder.Entity<TransferOrderDetail>()
                .HasOne(tod => tod.TransferOrderHeader)
                .WithMany(toh => toh.TransferOrderDetails)
                .HasForeignKey(tod => tod.TransferOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransferOrderDetail>()
                .HasOne(tod => tod.Item)
                .WithMany(i => i.TransferOrderDetails)
                .HasForeignKey(tod => tod.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // DirectReceiptHeader Relationships
            modelBuilder.Entity<DirectReceiptHeader>()
                .HasOne(drh => drh.Supplier)
                .WithMany()
                .HasForeignKey(drh => drh.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DirectReceiptHeader>()
                .HasOne(drh => drh.Branch)
                .WithMany()
                .HasForeignKey(drh => drh.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DirectReceiptHeader>()
                .HasOne(drh => drh.Status)
                .WithMany()
                .HasForeignKey(drh => drh.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // DirectReceiptDetail Relationships
            modelBuilder.Entity<DirectReceiptDetail>()
                .HasOne(drd => drd.DirectReceiptHeader)
                .WithMany(drh => drh.DirectReceiptDetails)
                .HasForeignKey(drd => drd.DirectReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DirectReceiptDetail>()
                .HasOne(drd => drd.Item)
                .WithMany(i => i.DirectReceiptDetails)
                .HasForeignKey(drd => drd.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseOrderHeader Relationships
            modelBuilder.Entity<PurchaseOrderHeader>()
                .HasOne(poh => poh.Supplier)
                .WithMany()
                .HasForeignKey(poh => poh.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrderHeader>()
                .HasOne(poh => poh.Branch)
                .WithMany()
                .HasForeignKey(poh => poh.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrderHeader>()
                .HasOne(poh => poh.BranchStock)
                .WithMany()
                .HasForeignKey(poh => poh.BranchStockId)
                .OnDelete(DeleteBehavior.NoAction);

            // PurchaseOrderDetail Relationships
            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.PurchaseOrderHeader)
                .WithMany(poh => poh.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.Item)
                .WithMany(i => i.PurchaseOrderDetails)
                .HasForeignKey(pod => pod.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pod => pod.Unit)
                .WithMany()
                .HasForeignKey(pod => pod.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // ItemUnit Relationships
            modelBuilder.Entity<ItemUnit>()
                .HasOne(iu => iu.Item)
                .WithMany(i => i.ItemUnits)
                .HasForeignKey(iu => iu.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemUnit>()
                .HasOne(iu => iu.ParentUnit)
                .WithMany(iu => iu.ChildUnits)
                .HasForeignKey(iu => iu.ParentUnitId)
                .OnDelete(DeleteBehavior.SetNull);

            // ItemUnitHistory Relationships
            modelBuilder.Entity<ItemUnitHistory>()
                .HasOne(iuh => iuh.Item)
                .WithMany()
                .HasForeignKey(iuh => iuh.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemUnitHistory>()
                .HasOne(iuh => iuh.ChangedByUser)
                .WithMany()
                .HasForeignKey(iuh => iuh.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // StockInHeader Relationships
            modelBuilder.Entity<StockInHeader>()
                .HasOne(sih => sih.Branch)
                .WithMany()
                .HasForeignKey(sih => sih.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockInHeader>()
                .HasOne(sih => sih.Supplier)
                .WithMany()
                .HasForeignKey(sih => sih.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockInHeader>()
                .HasOne(sih => sih.PurchaseOrderHeader)
                .WithMany(poh => poh.StockInHeaders)
                .HasForeignKey(sih => sih.PurchaseOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockInHeader>()
                .HasOne(sih => sih.TransferOrderHeader)
                .WithMany(toh => toh.StockInHeaders)
                .HasForeignKey(sih => sih.TransferOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockInHeader>()
                .HasOne(sih => sih.ReturnOrderHeader)
                .WithMany()
                .HasForeignKey(sih => sih.ReturnOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            // StockInDetail Relationships
            modelBuilder.Entity<StockInDetail>()
                .HasOne(sid => sid.StockInHeader)
                .WithMany(sih => sih.StockInDetails)
                .HasForeignKey(sid => sid.StockInId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockInDetail>()
                .HasOne(sid => sid.Item)
                .WithMany(i => i.StockInDetails)
                .HasForeignKey(sid => sid.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockInDetail>()
                .HasOne(sid => sid.PurchaseOrderDetail)
                .WithMany(pod => pod.StockInDetails)
                .HasForeignKey(sid => sid.PurchaseOrderDetailId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockInDetail>()
                .HasOne(sid => sid.TransferOrderDetail)
                .WithMany()
                .HasForeignKey(sid => sid.TransferOrderDetailId)
                .OnDelete(DeleteBehavior.SetNull);

            // StockOutHeader Relationships
            modelBuilder.Entity<StockOutHeader>()
                .HasOne(soh => soh.Branch)
                .WithMany()
                .HasForeignKey(soh => soh.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockOutHeader>()
                .HasOne(soh => soh.TransferOrderHeader)
                .WithMany(toh => toh.StockOutHeaders)
                .HasForeignKey(soh => soh.TransferOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockOutHeader>()
                .HasOne(soh => soh.ReturnOrderHeader)
                .WithMany()
                .HasForeignKey(soh => soh.ReturnOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            // StockOutDetail Relationships
            modelBuilder.Entity<StockOutDetail>()
                .HasOne(sod => sod.StockOutHeader)
                .WithMany(soh => soh.StockOutDetails)
                .HasForeignKey(sod => sod.StockOutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockOutDetail>()
                .HasOne(sod => sod.Item)
                .WithMany(i => i.StockOutDetails)
                .HasForeignKey(sod => sod.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockOutDetail>()
                .HasOne(sod => sod.TransferOrderDetail)
                .WithMany()
                .HasForeignKey(sod => sod.TransferOrderDetailId)
                .OnDelete(DeleteBehavior.SetNull);

            // SupplierInvoiceHeader Relationships
            modelBuilder.Entity<SupplierInvoiceHeader>()
                .HasOne(sih => sih.Supplier)
                .WithMany()
                .HasForeignKey(sih => sih.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SupplierInvoiceHeader>()
                .HasOne(sih => sih.DirectReceiptHeader)
                .WithMany(drh => drh.SupplierInvoiceHeaders)
                .HasForeignKey(sih => sih.DirectReceiptId)
                .OnDelete(DeleteBehavior.SetNull);

            // SupplierInvoiceDetail Relationships
            modelBuilder.Entity<SupplierInvoiceDetail>()
                .HasOne(sid => sid.SupplierInvoiceHeader)
                .WithMany(sih => sih.SupplierInvoiceDetails)
                .HasForeignKey(sid => sid.SupplierInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SupplierInvoiceDetail>()
                .HasOne(sid => sid.Item)
                .WithMany(i => i.SupplierInvoiceDetails)
                .HasForeignKey(sid => sid.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // ApprovalHistory Relationships
            modelBuilder.Entity<ApprovalHistory>()
                .HasOne(ah => ah.DocumentType)
                .WithMany()
                .HasForeignKey(ah => ah.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalHistory>()
                .HasOne(ah => ah.ApprovalStatus)
                .WithMany(ast => ast.ApprovalHistories)
                .HasForeignKey(ah => ah.ApprovalStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalHistory>()
                .HasOne(ah => ah.ActionByUser)
                .WithMany()
                .HasForeignKey(ah => ah.ActionByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ApprovalChain Relationships
            modelBuilder.Entity<ApprovalChain>()
                .HasOne(ac => ac.DocumentType)
                .WithMany()
                .HasForeignKey(ac => ac.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApprovalChain>()
                .HasOne(ac => ac.Role)
                .WithMany()
                .HasForeignKey(ac => ac.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification Relationships
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // SavedView Relationships
            modelBuilder.Entity<SavedView>()
                .HasOne(sv => sv.User)
                .WithMany()
                .HasForeignKey(sv => sv.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ItemFlag Relationships
            modelBuilder.Entity<ItemFlag>()
                .HasOne(ifl => ifl.Item)
                .WithMany(i => i.ItemFlags)
                .HasForeignKey(ifl => ifl.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // ItemPackage Relationships
            modelBuilder.Entity<ItemPackage>()
                .HasOne(ip => ip.Item)
                .WithMany(i => i.ItemPackages)
                .HasForeignKey(ip => ip.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemPackage>()
                .HasOne(ip => ip.Package)
                .WithMany()
                .HasForeignKey(ip => ip.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // AuditTrail Relationships
            modelBuilder.Entity<AuditTrail>()
                .HasOne(at => at.User)
                .WithMany()
                .HasForeignKey(at => at.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditTrail>()
                .HasOne(at => at.DocumentType)
                .WithMany()
                .HasForeignKey(at => at.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // OpeningBalance Relationships
            modelBuilder.Entity<OpeningBalance>()
                .HasOne(ob => ob.Item)
                .WithMany(i => i.OpeningBalances)
                .HasForeignKey(ob => ob.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OpeningBalance>()
                .HasOne(ob => ob.Branch)
                .WithMany()
                .HasForeignKey(ob => ob.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OpeningBalance>()
                .HasOne(ob => ob.FiscalYearNavigation)
                .WithMany()
                .HasForeignKey(ob => ob.FiscalYear)
                .OnDelete(DeleteBehavior.Restrict);



            // ReturnOrderHeader Relationships
            modelBuilder.Entity<ReturnOrderHeader>()
                .HasOne(roh => roh.Branch)
                .WithMany()
                .HasForeignKey(roh => roh.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReturnOrderHeader>()
                .HasOne(roh => roh.Supplier)
                .WithMany()
                .HasForeignKey(roh => roh.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

            // ReturnOrderDetail Relationships
            modelBuilder.Entity<ReturnOrderDetail>()
                .HasOne(rod => rod.ReturnOrderHeader)
                .WithMany(roh => roh.ReturnOrderDetails)
                .HasForeignKey(rod => rod.ReturnOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReturnOrderDetail>()
                .HasOne(rod => rod.Item)
                .WithMany(i => i.ReturnOrderDetails)
                .HasForeignKey(rod => rod.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReturnOrderDetail>()
                .HasOne(rod => rod.OriginalStockInDetail)
                .WithMany()
                .HasForeignKey(rod => rod.OriginalStockInDetailId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ReturnOrderDetail>()
                .HasOne(rod => rod.OriginalStockOutDetail)
                .WithMany()
                .HasForeignKey(rod => rod.OriginalStockOutDetailId)
                .OnDelete(DeleteBehavior.SetNull);

            // StockOutReturnHeader Relationships
            modelBuilder.Entity<StockOutReturnHeader>()
                .HasOne(sorh => sorh.Branch)
                .WithMany()
                .HasForeignKey(sorh => sorh.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockOutReturnHeader>()
                .HasOne(sorh => sorh.BranchStock)
                .WithMany()
                .HasForeignKey(sorh => sorh.BranchStockId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StockOutReturnHeader>()
                .HasOne(sorh => sorh.ReturnOrderHeader)
                .WithMany(roh => roh.StockOutReturnHeaders)
                .HasForeignKey(sorh => sorh.ReturnOrderId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockOutReturnHeader>()
                .HasOne(sorh => sorh.Supplier)
                .WithMany()
                .HasForeignKey(sorh => sorh.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);

            // StockOutReturnDetail Relationships
            modelBuilder.Entity<StockOutReturnDetail>()
                .HasOne(sord => sord.StockOutReturnHeader)
                .WithMany(sorh => sorh.StockOutReturnDetails)
                .HasForeignKey(sord => sord.StockOutReturnId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StockOutReturnDetail>()
                .HasOne(sord => sord.Item)
                .WithMany(i => i.StockOutReturnDetails)
                .HasForeignKey(sord => sord.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockOutReturnDetail>()
                .HasOne(sord => sord.ItemPackage)
                .WithMany()
                .HasForeignKey(sord => sord.ItemPackageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockOutReturnDetail>()
                .HasOne(sord => sord.ReturnOrderDetail)
                .WithMany(rod => rod.StockOutReturnDetails)
                .HasForeignKey(sord => sord.ReturnOrderDetailId)
                .OnDelete(DeleteBehavior.SetNull);

            // Setting Relationships - Configure primary keys first
            modelBuilder.Entity<Setting_flag_master>()
                .HasKey(sfm => sfm.FlagMasterId);

            modelBuilder.Entity<Setting_flag_details>()
                .HasKey(sfd => new { sfd.FlagMasterId, sfd.FlagDetailName }); // Composite key

            modelBuilder.Entity<Setting_flag_details>()
                .HasOne(sfd => sfd.Setting_flag_master)
                .WithMany()
                .HasForeignKey(sfd => sfd.FlagMasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Setting_flag_details>()
                .HasOne(sfd => sfd.FlagType)
                .WithMany()
                .HasForeignKey(sfd => sfd.FlagTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            // AttemptLog Relationships
            modelBuilder.Entity<AttemptLog>()
                .HasOne(al => al.User)
                .WithMany()
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExportLog Relationships
            modelBuilder.Entity<ExportLog>()
                .HasOne(el => el.User)
                .WithMany()
                .HasForeignKey(el => el.UserId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
