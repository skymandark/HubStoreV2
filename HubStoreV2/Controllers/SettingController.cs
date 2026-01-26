using Core.Domin;
using Core.Services.SettingServices;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HubStoreV2.Controllers
{
    public class SettingController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly ApplicationDbContext _context;

        public SettingController(ISettingService settingService, ApplicationDbContext context)
        {
            _settingService = settingService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "إعدادات النظام";

            // Get current settings
            var approvalWorkflowMode = await _settingService.GetApprovalWorkflowModeAsync();

            // Get all system settings
            var systemSettings = await _context.Set<SystemSetting>().ToListAsync();

            var model = new
            {
                ApprovalWorkflowMode = approvalWorkflowMode,
                SystemSettings = systemSettings
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApprovalWorkflow(bool useApprovalSystem)
        {
            // Update or create the setting
            var setting = await _context.Set<SystemSetting>()
                .FirstOrDefaultAsync(s => s.SettingKey == "ApprovalWorkflowMode");

            if (setting == null)
            {
                setting = new SystemSetting
                {
                    SettingKey = "ApprovalWorkflowMode",
                    SettingValue = useApprovalSystem.ToString(),
                    Description = "تفعيل نظام الموافقات على العمليات",
                    ModifiedBy = User.Identity?.Name ?? "System",
                    ModifiedDate = DateTime.UtcNow
                };
                _context.Set<SystemSetting>().Add(setting);
            }
            else
            {
                setting.SettingValue = useApprovalSystem.ToString();
                setting.ModifiedDate = DateTime.UtcNow;
                setting.ModifiedBy = User.Identity?.Name ?? "System";
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "تم حفظ الإعدادات بنجاح";
            return RedirectToAction("Index");
        }
    }
}