using Core.Domin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services.SettingServices
{
    public interface ISettingService
    {
        Task<List<Setting_flag_master>> GetAllMastersAsync();
        Task<Setting_flag_master> GetMasterByIdAsync(int id);
        Task AddMasterAsync(Setting_flag_master master);
        Task AddDetailAsync(Setting_flag_details detail);
        Task<bool> GetApprovalWorkflowModeAsync(); // True for Approval Mode, False for Direct
    }
}



