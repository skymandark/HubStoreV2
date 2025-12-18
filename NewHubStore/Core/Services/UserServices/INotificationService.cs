using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ViewModels.UserViewModels;

namespace Core.Services.UserServices
{
    public interface INotificationService
    {
        Task<NotificationViewModel> CreateNotificationAsync(CreateNotificationViewModel notificationDto);
        Task<List<NotificationViewModel>> GetUserNotificationsAsync(string userId, NotificationFilterViewModel filters);
        Task<bool> MarkAsReadAsync(long notificationId, string userId);
        Task<int> GetUnreadCountAsync(string userId);
        Task<int> CalculatePriorityAsync(object documentDto);
        Task SendApprovalReminderAsync(int documentId);
    }
}
