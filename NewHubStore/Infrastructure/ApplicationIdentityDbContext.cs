using System;
using System.Collections.Generic;
using System.Text;
using CoreOne.HubStore.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Approval;
using Repository.IdentityData;
using static System.Net.Mime.MediaTypeNames;

namespace Repository
{
    public class ApplicationIdentityDbContext:IdentityDbContext<UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>
    {
        public static string ConnectionString = "";

        public static class TableConsts
        {
            public const string IdentityRoles = "Roles";
            public const string IdentityRoleClaims = "RoleClaims";
            public const string IdentityUserRoles = "UserRoles";
            public const string IdentityUsers = "Users";
            public const string IdentityUserLogins = "UserLogins";
            public const string IdentityUserClaims = "UserClaims";
            public const string IdentityUserTokens = "UserTokens";
        }

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        public DbSet<Applications> Applications { get; set; }
        public DbSet<ApproveDefnetions> ApproveDefnetions { get; set; }
        public DbSet<ApproveDetails> ApproveDetails { get; set; }
        public DbSet<ApproveStepStatusDefs> ApproveStepStatusDefs { get; set; }
        public DbSet<ApproveSteps> ApproveSteps { get; set; }
        public DbSet<ApprovesUsers> ApprovesUsers { get; set; }
        public DbSet<MenuFlags> MenuFlags { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<RoleMenus> RoleMenus { get; set; }
        public DbSet<UserFlags> UserFlags { get; set; }
        public DbSet<UserValidations> UserValidations { get; set; }
        public DbSet<ValidDefs> ValidDefs { get; set; }
        public DbSet<FlagDefs> FlagDefs { get; set; }
        public DbSet<Branch> Branches { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoleMenus>().HasKey(x => new { x.MenuId, x.RoleId });
            builder.Entity<UserFlags>().HasKey(x => new { x.FlagId, x.UserId });
            builder.Entity<UserValidations>().HasKey(x => new { x.ApplicationId, x.UserId, x.ValidValue, x.ValidId });



            //ValidDef
            base.OnModelCreating(builder);
            builder.Entity<ValidDefs>().HasData(
                new ValidDefs { Id = 1, ValidName = "فرع" },
                new ValidDefs { Id = 2, ValidName = "سمسار" },
                new ValidDefs { Id = 3, ValidName = "عميل" }
            );



            ConfigureIdentityContext(builder);
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.Entity<UserIdentityRole>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<UserIdentityRoleClaim>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<UserIdentityUserRole>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<UserIdentity>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<UserIdentityUserLogin>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<UserIdentityUserClaim>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<UserIdentityUserToken>().ToTable(TableConsts.IdentityUserTokens);
        }
    }
}
