using System;
using System.Linq;
using Core.Domin;
using Infrastructure.Data;

namespace Infrastructure.DataSeeds
{
    public static class SystemSettingSeed
    {
        public static void SeedSettings(ApplicationDbContext context)
        {
            if (context.SystemSettings.Any()) return;

            var settings = new[]
            {
                new SystemSetting
                {
                    SettingKey = "ApprovalWorkflowMode",
                    SettingValue = "true",
                    Description = "تفعيل نظام الموافقات على العمليات",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = "System",
                    ModifiedDate = DateTime.UtcNow
                },
                new SystemSetting
                {
                    SettingKey = "CompanyName",
                    SettingValue = "HubStore ERP",
                    Description = "اسم الشركة",
                    CreatedDate = DateTime.UtcNow,
                    ModifiedBy = "System",
                    ModifiedDate = DateTime.UtcNow
                }
            };

            context.SystemSettings.AddRange(settings);
            context.SaveChanges();
        }
    }
}
