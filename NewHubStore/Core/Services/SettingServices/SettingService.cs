//using Core.Domain.Interfaces.Setting;
using Core.Domin;
using Core.Domin.Interfaces.Setting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services.SettingServices
{
    public class SettingService : ISettingService
    {
        private readonly ISettingFlagMasterRepository _masterRepository;
        private readonly ISettingFlagDetailsRepository _detailsRepository;

        public SettingService(ISettingFlagMasterRepository masterRepository,
                              ISettingFlagDetailsRepository detailsRepository)
        {
            _masterRepository = masterRepository;
            _detailsRepository = detailsRepository;
        }

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
}
