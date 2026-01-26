//using System;
//using System.Linq;
//using Core.Domin;
//using Infrastructure.Data;

//namespace Infrastructure.DataSeeds
//{
//    public static class SupplierSeed
//    {
//        public static void SeedSuppliers(ApplicationDbContext context)
//        {
//            if (context.Suppliers.Any()) return;

//            var suppliers = new[]
//            {
//                new Supplier
//                {
//                    Code = "SUP001",
//                    Name = "Global Technology Solutions",
//                    ContactInfo = "John Doe",
//                    Address = "Tech Park, Dubai",
//                    Phone = "+971 4 1112223",
//                    Email = "info@globaltech.com",
//                    IsActive = true,
//                    CreatedBy = "System",
//                    CreatedAt = DateTime.UtcNow
//                },
//                new Supplier
//                {
//                    Code = "SUP002",
//                    Name = "Al-Baraka Trading Co.",
//                    ContactInfo = "Ahmed Ali",
//                    Address = "Olaya District, Riyadh",
//                    Phone = "+966 11 4445556",
//                    Email = "sales@albaraka.sa",
//                    IsActive = true,
//                    CreatedBy = "System",
//                    CreatedAt = DateTime.UtcNow
//                }
//            };

//            context.Suppliers.AddRange(suppliers);
//            context.SaveChanges();
//        }
//    }
//}
