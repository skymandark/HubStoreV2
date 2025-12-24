using Core.Domin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class SettingFlagDetailsRepository
    {
        public SettingFlagDetailsRepository(/*AppDbContext context*/)
        {
            // _context = context;
        }

        public async Task AddAsync(Setting_flag_details detail)
        {
            // _context.Add(detail); await _context.SaveChangesAsync();
        }
    }
}
