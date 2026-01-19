using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Dtos.UserDetail;

namespace Core.Dto.UserDetalis
{
    public  class UserRolesDto : BaseUserRolesDto
    {
        public UserRolesDto()
        {
            Roles = new List<RoleDto>();
        }

        public string UserName { get; set; }

        public List<SelectItemDto> RolesList { get; set; }

        public List<RoleDto> Roles { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
