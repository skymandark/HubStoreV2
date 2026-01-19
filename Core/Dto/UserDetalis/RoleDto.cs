using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Core.Dto.UserDetalis;

namespace Core.Dtos.UserDetail 
{
    public class RoleDto : BaseRoleDto, IRoleDto
    {
        [Required]
        public string Name { get; set; }
        public int? ApplicationId { get; set; }
        public string RoleType { get; set; }
        public string RoleValue { get; set; }
        public List<ApplicationDto> Applications { get; set; }
    }
}
