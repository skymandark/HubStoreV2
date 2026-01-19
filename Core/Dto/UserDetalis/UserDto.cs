using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserDetalis
{
    public  class UserDto: BaseUserDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_@\-\.\+]+$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }


        public int UserSerial { get; set; }

        public int? EmployeeId { get; set; }

        [StringLength(50)]
        public string PinCode { get; set; }
        public bool IsAdmin { get; set; }

        public List<ApplicationDto> Applications { get; set; }
        public string Branches { get; set; }
        public string Roles { get; set; }

        public List<UserRoleDto> RolesList { get; set; }
        public List<UserBranchesDto> BranchesList { get; set; }
    }

    public class UserRoleDto
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
