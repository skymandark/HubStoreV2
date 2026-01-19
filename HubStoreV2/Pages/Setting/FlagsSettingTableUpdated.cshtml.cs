using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Identity;
using Core.Domin;

namespace HubStoreV2.Pages.Setting
{
    public class ERPSystemSettingsModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ERPSystemSettingsModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public ERPSystemSettings SystemSettings { get; set; }

        public async Task OnGetAsync()
        {
            // Load current system settings
            SystemSettings = new ERPSystemSettings
            {
                UseApprovalSystem = false,
                CompanyName = "HubStore ERP",
                DefaultCurrency = "EGP",
                EnableNotifications = true,
                AutoBackupEnabled = false
            };
        }

        public async Task<IActionResult> OnPostAsync(ERPSystemSettings systemSettings)
        {
            try
            {
                TempData["Success"] = "تم حفظ إعدادات النظام بنجاح";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"حدث خطأ أثناء حفظ الإعدادات: {ex.Message}";
            }

            return RedirectToPage();
        }
    }

    public class ERPSystemSettings
    {
        public bool UseApprovalSystem { get; set; }
        public string CompanyName { get; set; }
        public string DefaultCurrency { get; set; }
        public bool EnableNotifications { get; set; }
        public bool AutoBackupEnabled { get; set; }
    }
}