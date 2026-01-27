using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Admin
{
    public class AdminUserListVm
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class AdminUserEditVm
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        public bool IsAdmin { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
    }

    public class AdminUserRolesVm
    {
        [Required]
        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<AdminRoleItemVm> Roles { get; set; } = new();
    }

    public class AdminRoleItemVm
    {
        public string Name { get; set; }
        public bool IsAssigned { get; set; }
    }
}
