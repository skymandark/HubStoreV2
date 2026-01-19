//using Core.Domin;
//using Microsoft.EntityFrameworkCore;
//using Repository;
//using System.Threading.Tasks;

//namespace Infrastructure.ServicesImpelemention
//{
//    public class SettingFlagDetailsRepository
//    {
//        private readonly ApplicationIdentityDbContext _context;

//        public SettingFlagDetailsRepository(ApplicationIdentityDbContext context)
//        {
//            _context = context;
//        }

//        public async Task AddAsync(Setting_flag_details detail)
//        {
//            _context.Set<Setting_flag_details>().Add(detail);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
