using System;
using System.Collections.Generic;

namespace Core.ViewModels.UserViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; }
    }

    public class SavedViewViewModel
    {
        public int SavedViewId { get; set; }
        public string ViewName { get; set; }
        public string FiltersJson { get; set; }
        public string ColumnsJson { get; set; }
        public string SortOrderJson { get; set; }
        public bool IsDefault { get; set; }
        public bool IsShared { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    public class NotificationViewModel
    {
        public long NotificationId { get; set; }
        public string UserId { get; set; }
        public int? DocumentId { get; set; }
        public string DocumentType { get; set; }
        public int PriorityScore { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }

    // Create/Update DTOs
    public class CreateUserViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }

    public class UpdateUserViewModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateSavedViewViewModel
    {
        public string ViewName { get; set; }
        public string FiltersJson { get; set; }
        public string ColumnsJson { get; set; }
        public string SortOrderJson { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateSavedViewViewModel
    {
        public string ViewName { get; set; }
        public string FiltersJson { get; set; }
        public string ColumnsJson { get; set; }
        public string SortOrderJson { get; set; }
        public bool IsDefault { get; set; }
        public bool IsShared { get; set; }
    }

    public class CreateNotificationViewModel
    {
        public string UserId { get; set; }
        public int? DocumentId { get; set; }
        public string DocumentType { get; set; }
        public int PriorityScore { get; set; }
        public string Message { get; set; }
        public string Link { get; set; }
    }

    // Filters
    public class NotificationFilterViewModel
    {
        public bool? IsRead { get; set; }
        public int? MinPriority { get; set; }
        public int? MaxPriority { get; set; }
        public string DocumentType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
