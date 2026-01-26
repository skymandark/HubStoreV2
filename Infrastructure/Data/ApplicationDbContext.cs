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

        // Direct Purchase Orders
        public DbSet<DirectPurchaseOrderHeader> DirectPurchaseOrderHeaders { get; set; }
        public DbSet<DirectPurchaseOrderDetail> DirectPurchaseOrderDetails { get; set; }

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
        public DbSet<InventoryLayer> InventoryLayers { get; set; }
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
// Attachments
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============= 1. Item Classifications =============
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Branch)
                .WithMany()
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Section)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SectionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.MainItem)
                .WithMany()
                .HasForeignKey(i => i.MainItemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.SubItem)
                .WithMany()
                .HasForeignKey(i => i.SubItemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Vendor)
                .WithMany()
                .HasForeignKey(i => i.VendorId);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Brand)
                .WithMany()
                .HasForeignKey(i => i.BrandId);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemType)
                .WithMany()
                .HasForeignKey(i => i.ItemTypeId);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemCategory)
                .WithMany()
                .HasForeignKey(i => i.ItemCategoryId);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.SellMainPackage)
                .WithMany(p => p.SellMainItems)
                .HasForeignKey(i => i.SellMainPackageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.BuyMainPackage)
                .WithMany(p => p.BuyMainItems)
                .HasForeignKey(i => i.BuyMainPackageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.ParentItem)
                .WithMany(i => i.ChildItems)
                .HasForeignKey(i => i.ParentItemId);

            modelBuilder.Entity<MainItem>()
                .HasOne(m => m.Section)
                .WithMany(s => s.MainItems)
                .HasForeignKey(m => m.SectionId);

            modelBuilder.Entity<SubItem>()
                .HasOne(s => s.MainItem)
                .WithMany(m => m.SubItems)
                .HasForeignKey(s => s.MainItemId);

            // ============= 2. General Orders & Movements =============
            modelBuilder.Entity<Order>()
                .HasOne(o => o.OrderType)
                .WithMany()
                .HasForeignKey(o => o.OrderTypeId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BranchFrom)
                .WithMany(b => b.OrdersFrom)
                .HasForeignKey(o => o.BranchFromId)
               ;

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BranchTo)
                .WithMany(b => b.OrdersTo)
                .HasForeignKey(o => o.BranchToId)
                ;

            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(ol => ol.OrderId);

            modelBuilder.Entity<Movement>()
                .HasOne(m => m.BranchFrom)
                .WithMany(b => b.MovementsFrom)
                .HasForeignKey(m => m.BranchFromId);


            modelBuilder.Entity<Movement>()
                .HasOne(m => m.BranchTo)
                .WithMany(b => b.MovementsTo)
                .HasForeignKey(m => m.BranchToId);
              

            modelBuilder.Entity<MovementLine>()
                .HasOne(ml => ml.Movement)
                .WithMany(m => m.MovementLines)
                .HasForeignKey(ml => ml.MovementId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MovementLine>()
                .HasOne(ml => ml.Branch)
                .WithMany(b => b.MovementLines)
                .HasForeignKey(ml => ml.BranchId);
                

            // ============= 3. Purchase Orders =============
            modelBuilder.Entity<PurchaseOrderHeader>()
                .HasOne(p => p.Branch)
                .WithMany(b => b.PurchaseOrderHeaders)
                .HasForeignKey(p => p.BranchId);

            modelBuilder.Entity<PurchaseOrderHeader>()
                .HasOne(p => p.Supplier)
                .WithMany()
                .HasForeignKey(p => p.SupplierId);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pd => pd.PurchaseOrderHeader)
                .WithMany(p => p.PurchaseOrderDetails)
                .HasForeignKey(pd => pd.PurchaseOrderId)
               ;

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pd => pd.Item)
                .WithMany()
                .HasForeignKey(pd => pd.ItemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasOne(pd => pd.Unit)
                .WithMany()
                .HasForeignKey(pd => pd.UnitId)
               ;

            // ============= 3.1 Direct Purchase Orders =============
            modelBuilder.Entity<DirectPurchaseOrderHeader>()
                .HasOne(d => d.Branch)
                .WithMany()
                .HasForeignKey(d => d.BranchId)
               ;

            modelBuilder.Entity<DirectPurchaseOrderDetail>()
                .HasOne(dd => dd.DirectPurchaseOrderHeader)
                .WithMany(d => d.DirectPurchaseOrderDetails)
                .HasForeignKey(dd => dd.DirectPurchaseOrderId)
              ;
            // ============= 4. Transfer Orders =============
            modelBuilder.Entity<TransferOrderHeader>()
                .HasOne(t => t.FromBranch)
                .WithMany(b => b.TransferOrderHeadersFrom)
                .HasForeignKey(t => t.FromBranchId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TransferOrderHeader>()
                .HasOne(t => t.ToBranch)
                .WithMany(b => b.TransferOrderHeadersTo)
                .HasForeignKey(t => t.ToBranchId)
               ;

            modelBuilder.Entity<TransferOrderDetail>()
                .HasOne(td => td.TransferOrderHeader)
                .WithMany(t => t.TransferOrderDetails)
                .HasForeignKey(td => td.TransferOrderId);

            // ============= 5. Stock In (Receipts) =============
            modelBuilder.Entity<StockInHeader>()
                .HasOne(s => s.Branch)
                .WithMany()
                .HasForeignKey(s => s.BranchId)
              ;

            modelBuilder.Entity<StockInHeader>()
                .HasOne(s => s.Supplier)
                .WithMany()
                .HasForeignKey(s => s.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockInDetail>()
                .HasOne(sd => sd.StockInHeader)
                .WithMany(s => s.StockInDetails)
                .HasForeignKey(sd => sd.StockInId);

            modelBuilder.Entity<StockInDetail>()
                .HasOne(sd => sd.Item)
                .WithMany()
                .HasForeignKey(sd => sd.ItemId)
               ;

            // ============= 6. Stock Out (Issues) =============
            modelBuilder.Entity<StockOutHeader>()
                .HasOne(s => s.Branch)
                .WithMany()
                .HasForeignKey(s => s.BranchId)
             ;

            modelBuilder.Entity<StockOutDetail>()
                .HasOne(sd => sd.StockOutHeader)
                .WithMany(s => s.StockOutDetails)
                .HasForeignKey(sd => sd.StockOutId);

            // ============= 7. Direct Receipts & Invoices =============
            modelBuilder.Entity<DirectReceiptHeader>()
                .HasOne(d => d.Branch)
                .WithMany()
                .HasForeignKey(d => d.BranchId)
               ;

            modelBuilder.Entity<DirectReceiptDetail>()
                .HasOne(dd => dd.DirectReceiptHeader)
                .WithMany(d => d.DirectReceiptDetails)
                .HasForeignKey(dd => dd.DirectReceiptId);

            modelBuilder.Entity<StockOutReturnHeader>()
                .HasOne(s => s.Branch)
                .WithMany(b => b.StockOutReturnHeaders)
                .HasForeignKey(s => s.BranchId)
               ;

            modelBuilder.Entity<StockOutReturnDetail>()
                .HasOne(sd => sd.StockOutReturnHeader)
                .WithMany(s => s.StockOutReturnDetails)
                .HasForeignKey(sd => sd.StockOutReturnId);

            // ============= 8. Approval & Audit =============
            modelBuilder.Entity<ApprovalHistory>()
                .HasOne(ah => ah.DocumentType)
                .WithMany()
                .HasForeignKey(ah => ah.DocumentTypeId)
                ;

            modelBuilder.Entity<ApprovalHistory>()
                .HasOne(ah => ah.ApprovalStatus)
                .WithMany()
                .HasForeignKey(ah => ah.ApprovalStatusId)
              ;

            // ============= 9. Settings =============
            modelBuilder.Entity<Setting_flag_details>()
                .HasKey(s => new { s.FlagMasterId, s.FlagDetailName });

            modelBuilder.Entity<Setting_flag_details>()
                .HasOne(s => s.Setting_flag_master)
                .WithMany()
                .HasForeignKey(s => s.FlagMasterId);

            // ============= 10. Receipt Linkage =============
            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.OrderLine)
                .WithMany()
                .HasForeignKey(r => r.OrderLineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Movement)
                .WithMany()
                .HasForeignKey(r => r.MovementId)
               ;
        }
    }
}
