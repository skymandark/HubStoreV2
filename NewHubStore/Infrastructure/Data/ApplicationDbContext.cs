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
        public DbSet<Receipt> receipts { get; set; }
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

        // Reference Tables
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
        public DbSet<CostingMethod> CostingMethods { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Branch> Branches { get; set; }

        // Approval & Audit
        public DbSet<ApprovalHistory> ApprovalHistories { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<AttemptLog> AttemptLogs { get; set; }
        public DbSet<ExportLog> ExportLogs { get; set; }

        public DbSet<ApprovalChain> approvalChains { get; set; }
        public DbSet<PendingApproval> pendingApprovals { get; set; }

        // User Features
        public DbSet<SavedView> SavedViews { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
        
            base.OnModelCreating(modelBuilder);

            // ============================================
            // ITEM CONFIGURATIONS
            // ============================================

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasOne(i => i.ParentItem)
                    .WithMany(i => i.ChildItems)
                    .HasForeignKey(i => i.ParentItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(i => !i.IsDeleted);
            });

            modelBuilder.Entity<ItemUnit>(entity =>
            {
                entity.HasOne(iu => iu.ParentUnit)
                    .WithMany(iu => iu.ChildUnits)
                    .HasForeignKey(iu => iu.ParentUnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(iu => iu.Item)
                    .WithMany(i => i.ItemUnits)
                    .HasForeignKey(iu => iu.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

               
            });

            modelBuilder.Entity<ItemUnitHistory>(entity =>
            {
                entity.HasOne(iuh => iuh.Item)
                    .WithMany()
                    .HasForeignKey(iuh => iuh.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================
            // OPENING BALANCE CONFIGURATIONS
            // ============================================

            modelBuilder.Entity<OpeningBalance>(entity =>
            {
                entity.HasOne(ob => ob.Item)
                    .WithMany(i => i.OpeningBalances)
                    .HasForeignKey(ob => ob.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ob => ob.Branch)
                    .WithMany(b => b.OpeningBalances)
                    .HasForeignKey(ob => ob.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ob => ob.FiscalYearNavigation)
                    .WithMany(fy => fy.OpeningBalances)
                    .HasForeignKey(ob => ob.FiscalYear)
                    .HasPrincipalKey(fy => fy.Year)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================
            // MOVEMENT CONFIGURATIONS
            // ============================================

            modelBuilder.Entity<Movement>(entity =>
            {
                entity.HasOne(m => m.MovementType)
                    .WithMany(mt => mt.Movements)
                    .HasForeignKey(m => m.MovementTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.BranchFrom)
                    .WithMany(b => b.MovementsFrom)
                    .HasForeignKey(m => m.BranchFromId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.BranchTo)
                    .WithMany(b => b.MovementsTo)
                    .HasForeignKey(m => m.BranchToId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Supplier)
                    .WithMany(s => s.Movements)
                    .HasForeignKey(m => m.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.ApprovalStatus)
                    .WithMany(a => a.Movements)
                    .HasForeignKey(m => m.ApprovalStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(m => !m.IsDeleted);
            });

            modelBuilder.Entity<MovementLine>(entity =>
            {
                entity.HasOne(ml => ml.Movement)
                    .WithMany(m => m.MovementLines)
                    .HasForeignKey(ml => ml.MovementId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ml => ml.Item)
                    .WithMany(i => i.MovementLines)
                    .HasForeignKey(ml => ml.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ml => ml.Branch)
                    .WithMany(b => b.MovementLines)
                    .HasForeignKey(ml => ml.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ml => ml.MovementType)
                    .WithMany(mt => mt.MovementLines)
                    .HasForeignKey(ml => ml.MovementTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

           
            });

            // ============================================
            // ORDER CONFIGURATIONS
            // ============================================

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o => o.OrderType)
                    .WithMany(ot => ot.Orders)
                    .HasForeignKey(o => o.OrderTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Supplier)
                    .WithMany(s => s.Orders)
                    .HasForeignKey(o => o.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.BranchFrom)
                    .WithMany(b => b.OrdersFrom)
                    .HasForeignKey(o => o.BranchFromId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.BranchTo)
                    .WithMany(b => b.OrdersTo)
                    .HasForeignKey(o => o.BranchToId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.ApprovalStatus)
                    .WithMany(a => a.Orders)
                    .HasForeignKey(o => o.ApprovalStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(o => !o.IsDeleted);
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasOne(ol => ol.Order)
                    .WithMany(o => o.OrderLines)
                    .HasForeignKey(ol => ol.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ol => ol.Item)
                    .WithMany(i => i.OrderLines)
                    .HasForeignKey(ol => ol.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);


            });

            // ============================================
            // TRANSFER ORDER CONFIGURATIONS
            // ============================================

            modelBuilder.Entity<TransferOrderHeader>(entity =>
            {
                entity.HasOne(toh => toh.FromBranch)
                    .WithMany(b => b.TransferOrderHeadersFrom)
                    .HasForeignKey(toh => toh.FromBranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(toh => toh.ToBranch)
                    .WithMany(b => b.TransferOrderHeadersTo)
                    .HasForeignKey(toh => toh.ToBranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(toh => toh.ShipmentType)
                    .WithMany(st => st.TransferOrderHeaders)
                    .HasForeignKey(toh => toh.ShipmentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(toh => toh.TransferOrderStatus)
                    .WithMany(tos => tos.TransferOrderHeaders)
                    .HasForeignKey(toh => toh.TransferOrderStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(toh => !toh.IsDeleted);
            });

            modelBuilder.Entity<TransferOrderDetail>(entity =>
            {
                entity.HasOne(tod => tod.TransferOrderHeader)
                    .WithMany(toh => toh.TransferOrderDetails)
                    .HasForeignKey(tod => tod.TransferOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tod => tod.Item)
                    .WithMany(i => i.TransferOrderDetails)
                    .HasForeignKey(tod => tod.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(tod => !tod.IsDeleted);
            });

            // ============================================
            // DIRECT RECEIPT CONFIGURATIONS
            // ============================================

            modelBuilder.Entity<DirectReceiptHeader>(entity =>
            {
                entity.HasOne(drh => drh.Supplier)
                    .WithMany(s => s.DirectReceiptHeaders)
                    .HasForeignKey(drh => drh.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(drh => drh.Branch)
                    .WithMany(b => b.DirectReceiptHeaders)
                    .HasForeignKey(drh => drh.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(drh => drh.Status)
                    .WithMany(a => a.DirectReceiptHeaders)
                    .HasForeignKey(drh => drh.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(drh => !drh.IsDeleted);
            });

            modelBuilder.Entity<DirectReceiptDetail>(entity =>
            {
                entity.HasOne(drd => drd.DirectReceiptHeader)
                    .WithMany(drh => drh.DirectReceiptDetails)
                    .HasForeignKey(drd => drd.DirectReceiptId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(drd => drd.Item)
                    .WithMany(i => i.DirectReceiptDetails)
                    .HasForeignKey(drd => drd.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(drd => !drd.IsDeleted);
            });

            modelBuilder.Entity<SupplierInvoiceHeader>(entity =>
            {
                entity.HasOne(sih => sih.Supplier)
                    .WithMany(s => s.SupplierInvoiceHeaders)
                    .HasForeignKey(sih => sih.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(sih => sih.DirectReceiptHeader)
                    .WithMany(drh => drh.SupplierInvoiceHeaders)
                    .HasForeignKey(sih => sih.DirectReceiptId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(sih => !sih.IsDeleted);
            });

            modelBuilder.Entity<SupplierInvoiceDetail>(entity =>
            {
                entity.HasOne(sid => sid.SupplierInvoiceHeader)
                    .WithMany(sih => sih.SupplierInvoiceDetails)
                    .HasForeignKey(sid => sid.SupplierInvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sid => sid.Item)
                    .WithMany(i => i.SupplierInvoiceDetails)
                    .HasForeignKey(sid => sid.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(sid => !sid.IsDeleted);
            });

            modelBuilder.Entity<ApprovalChain>(entity =>
            {
                entity.HasOne(ac => ac.DocumentType)
                    .WithMany(dt => dt.ApprovalChains)
                    .HasForeignKey(ac => ac.DocumentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ac => ac.Role)
                    .WithMany()
                    .HasForeignKey(ac => ac.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================
            // APPROVAL & AUDIT CONFIGURATIONS
            // ============================================

            modelBuilder.Entity<ApprovalHistory>(entity =>
            {
                entity.HasOne(ah => ah.DocumentType)
                    .WithMany(dt => dt.ApprovalHistories)
                    .HasForeignKey(ah => ah.DocumentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ah => ah.ApprovalStatus)
                    .WithMany(a => a.ApprovalHistories)
                    .HasForeignKey(ah => ah.ApprovalStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.HasOne(at => at.DocumentType)
                    .WithMany(dt => dt.AuditTrails)
                    .HasForeignKey(at => at.DocumentTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


        }

    }


}
