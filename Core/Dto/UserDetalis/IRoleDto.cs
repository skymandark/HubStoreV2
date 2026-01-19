using System;
using System.Collections.Generic;
using System.Text;
using Core.Dto.UserDetalis;

namespace Core.Dtos.UserDetail
{
    public interface IRoleDto : IBaseRoleDto
    {
        string Name { get; set; }
        public int? ApplicationId { get; set; }
        public string RoleType { get; set; }
        public string RoleValue { get; set; }
        public List<ApplicationDto> Applications { get; set; }
    }
}
