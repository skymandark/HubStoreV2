using System;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data;
using Infrastructure.DataSeeds;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Note: If you need UserManager, pass it as second parameter and call UserSeed
            
            // 1. Core Lookups
            //LookupSeed.SeedAllLookups(context);
            SystemSettingSeed.SeedSettings(context);
            
            // 2. Organization
            BranchSeed.SeedBranches(context);
            //SupplierSeed.SeedSuppliers(context);
            
            // 3. Inventory
            ItemSeed.SeedItems(context);
            
            // 4. Identity (Optional - if needed)
            // SeedUsers(userManager);
        }
    }
}
