using Core.Domin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SettingFlagMasterRepository
    {
        public SettingFlagMasterRepository(/*AppDbContext context*/)
        {
            // _context = context;
        }

        public async Task<List<Setting_flag_master>> GetAllAsync()
        {
            //  dummy،DbContext
            return new List<Setting_flag_master>();
        }

        public async Task<Setting_flag_master> GetByIdAsync(int id)
        {
            return new Setting_flag_master();
        }

        public async Task AddAsync(Setting_flag_master master)
        {
            // _context.Add(master); await _context.SaveChangesAsync();
        }
    }
}
