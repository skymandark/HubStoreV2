//using System;
//using System.Linq;
//using Core.Domin;
//using Infrastructure.Data;

//namespace Infrastructure.DataSeeds
//{
//    public static class LookupSeed
//    {
//        public static void SeedAllLookups(ApplicationDbContext context)
//        {
//            SeedTaxes(context);
//            SeedDocumentTypes(context);
//            SeedApprovalStatuses(context);
//            SeedTradeTypes(context);
//            SeedFiscalYear(context);
//        }

//        private static void SeedTaxes(ApplicationDbContext context)
//        {
//            if (context.Taxes.Any()) return;
//            context.Taxes.Add(new Tax { Name = "VAT 15%", Percentage = 15, IsActive = true, CreatedBy = "System" });
//            context.SaveChanges();
//        }

//        private static void SeedDocumentTypes(ApplicationDbContext context)
//        {
//            if (context.DocumentTypes.Any()) return;
//            var types = new[]
//            {
//                new DocumentType { Name = "Purchase Order", Description = "طلب شراء", IsActive = true, CreatedBy = "System" },
//                new DocumentType { Name = "Stock Out", Description = "إذن صرف", IsActive = true, CreatedBy = "System" },
//                new DocumentType { Name = "Transfer Order", Description = "أمر تحويل", IsActive = true, CreatedBy = "System" }
//            };
//            context.DocumentTypes.AddRange(types);
//            context.SaveChanges();
//        }

//        private static void SeedApprovalStatuses(ApplicationDbContext context)
//        {
//            if (context.ApprovalStatuses.Any()) return;
//            var statuses = new[]
//            {
//                new ApprovalStatus { Name = "Draft", Description = "مسودة", IsActive = true, CreatedBy = "System" },
//                new ApprovalStatus { Name = "Submitted", Description = "مرسل للمراجعة", IsActive = true, CreatedBy = "System" },
//                new ApprovalStatus { Name = "Approved", Description = "معتمد", IsActive = true, CreatedBy = "System" },
//                new ApprovalStatus { Name = "Rejected", Description = "مرفوض", IsActive = true, CreatedBy = "System" }
//            };
//            context.ApprovalStatuses.AddRange(statuses);
//            context.SaveChanges();
//        }

//        private static void SeedTradeTypes(ApplicationDbContext context)
//        {
//            if (context.TradeTypes.Any()) return;
//            var types = new[]
//            {
//                new TradeType { NameArab = "محلي", NameEng = "Local", Description = "Local Trade", CreatedBy = "System" },
//                new TradeType { NameArab = "استيراد", NameEng = "Import", Description = "Import Trade", CreatedBy = "System" }
//            };
//            context.TradeTypes.AddRange(types);
//            context.SaveChanges();
//        }

//        private static void SeedFiscalYear(ApplicationDbContext context)
//        {
//            if (context.FiscalYears.Any()) return;
//            context.FiscalYears.Add(new FiscalYear
//            {
//                YearName = "2026",
//                StartDate = new DateTime(2026, 1, 1),
//                EndDate = new DateTime(2026, 12, 31),
//                IsClosed = false,
//                CreatedBy = "System"
//            });
//            context.SaveChanges();
//        }
//    }
//}
