//using Core.Domin;
//using Microsoft.EntityFrameworkCore;
//using Repository;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Infrastructure
//{
//    public class SettingFlagMasterRepository
//    {
//        private readonly ApplicationIdentityDbContext _context;

//        public SettingFlagMasterRepository(ApplicationIdentityDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<Setting_flag_master>> GetAllAsync()
//        {
//            return await _context.Set<Setting_flag_master>().ToListAsync();
//        }

//        public async Task<Setting_flag_master?> GetByNameAsync(string name)
//        {
//            return await _context.Set<Setting_flag_master>()
//                                 .FirstOrDefaultAsync(x => x.FlagMasterName == name);
//        }


//        public async Task AddAsync(Setting_flag_master master)
//        {
//            _context.Set<Setting_flag_master>().Add(master);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
