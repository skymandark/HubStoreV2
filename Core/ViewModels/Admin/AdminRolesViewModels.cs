using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Admin
{
    public class AdminRoleListVm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int UsersCount { get; set; }
    }

    public class AdminRoleCreateVm
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
    }
}
