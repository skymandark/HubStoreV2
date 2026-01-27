//using Core.Domain.Interfaces.Setting;
using Core.Domin;
using Core.Domin.Interfaces.Setting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.SettingServices
{
    public class SettingService : ISettingService
    {
        private readonly IConfiguration _configuration;
        private readonly DbContext _context;

        public SettingService(IConfiguration configuration, DbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // Note: These methods are placeholders for future implementation using database flags
        public async Task<List<Setting_flag_master>> GetAllMastersAsync()
        {
            // TODO: Implement when repositories are available
            return new List<Setting_flag_master>();
        }

        public async Task<Setting_flag_master> GetMasterByIdAsync(int id)
        {
            // TODO: Implement
            return null;
        }

        public async Task AddMasterAsync(Setting_flag_master master)
        {
            // TODO: Implement
        }

        public async Task AddDetailAsync(Setting_flag_details detail)
        {
            // TODO: Implement
        }

        public async Task<bool> GetApprovalWorkflowModeAsync()
        {
            var setting = await _context.Set<SystemSetting>()
                .FirstOrDefaultAsync(s => s.SettingKey == "ApprovalWorkflowMode");

            if (setting != null && bool.TryParse(setting.SettingValue, out var mode))
            {
                return mode;
            }

            return true; // Default to true if not set
        }
    }
}
