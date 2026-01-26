using System;
using Core.Domin;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Infrastructure.DataSeeds
{
    public static class BranchSeed
    {
        public static void SeedBranches(ApplicationDbContext context)
        {
            var branches = new[]
            {
                new Branch
                {
                    Code = "BR001",
                    Name = "Main Branch",
                    Name_Arab = "الفرع الرئيسي",
                    Location = "Cairo, Egypt",
                    Address = "123 Main Street, Cairo",
                    Phone = "+20 2 12345678",
                    IsActive = true,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow,
                    BaseId = 1
                },
                new Branch
                {
                    Code = "BR002",
                    Name = "Alexandria Branch",
                    Name_Arab = "فرع الإسكندرية",
                    Location = "Alexandria, Egypt",
                    Address = "45 Alexandria Road, Alexandria",
                    Phone = "+20 3 87654321",
                    IsActive = true,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow,
                    BaseId = 2
                },
                new Branch
                {
                    Code = "BR003",
                    Name = "Giza Branch",
                    Name_Arab = "فرع الجيزة",
                    Location = "Giza, Egypt",
                    Address = "78 Giza Street, Giza",
                    Phone = "+20 2 98765432",
                    IsActive = true,
                    CreatedBy = "System",
                    ModifiedBy = "System",
                    ModifiedAt = DateTime.UtcNow,
                    BaseId = 3
                }
            };

            foreach (var branch in branches)
            {
                var exists = context.Branches.Any(b => b.Code == branch.Code);
                if (!exists)
                {
                    context.Branches.Add(branch);
                }
            }

            context.SaveChanges();
        }
    }
}
