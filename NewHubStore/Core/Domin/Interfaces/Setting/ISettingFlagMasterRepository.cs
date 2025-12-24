using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin.Interfaces.Setting
{
    public interface ISettingFlagMasterRepository
    {
        Task<List<Setting_flag_master>> GetAllAsync();
        Task<Setting_flag_master> GetByIdAsync(int id);
        Task AddAsync(Setting_flag_master master);
    }
}
