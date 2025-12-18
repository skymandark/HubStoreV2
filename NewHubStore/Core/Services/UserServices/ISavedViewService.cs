using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.UserViewModels;

namespace Core.Services.UserServices
{
    public interface ISavedViewService
    {
        Task<SavedViewViewModel> SaveViewAsync(string userId, CreateSavedViewViewModel viewDto);
        Task<List<SavedViewViewModel>> GetSavedViewsAsync(string userId);
        Task<SavedViewViewModel> UpdateViewAsync(int viewId, UpdateSavedViewViewModel viewDto);
        Task<bool> DeleteViewAsync(int viewId);
        Task<bool> SetDefaultViewAsync(string userId, int viewId);
        Task<bool> ShareViewAsync(int viewId, List<string> userIds);
    }
}
