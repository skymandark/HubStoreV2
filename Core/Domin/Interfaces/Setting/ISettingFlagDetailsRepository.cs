using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin.Interfaces.Setting
{
    public class SettingService(ISettingFlagMasterRepository masterRepository, ISettingFlagDetailsRepository detailsRepository) 
    {
        private readonly ISettingFlagMasterRepository _masterRepository = masterRepository;
        private readonly ISettingFlagDetailsRepository _detailsRepository = detailsRepository;

        public async Task<List<Setting_flag_master>> GetAllMastersAsync()
        {
            return await _masterRepository.GetAllAsync();
        }

        public async Task<Setting_flag_master> GetMasterByIdAsync(int id)
        {
            return await _masterRepository.GetByIdAsync(id);
        }

        public async Task AddMasterAsync(Setting_flag_master master)
        {
            if (master == null) throw new ArgumentNullException(nameof(master));
            await _masterRepository.AddAsync(master);
        }

        public async Task AddDetailAsync(Setting_flag_details detail)
        {
            if (detail == null) throw new ArgumentNullException(nameof(detail));
            await _detailsRepository.AddAsync(detail);
        }
    }
    public interface ISettingFlagDetailsRepository
    {
        Task AddAsync(Setting_flag_details detail);
        Task<List<Setting_flag_details>> GetAllAsync();
        Task<Setting_flag_details> GetByNameAsync(string name);
    }
}
