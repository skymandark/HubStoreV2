using System;
using System.Collections.Generic;
using System.Text;


namespace Core.Dtos.UserDetail
{
    public class BaseRoleDto : IBaseRoleDto
    {
        public string Id { get; set; }

        public bool IsDefaultId() => EqualityComparer<string>.Default.Equals(Id, default);

        object IBaseRoleDto.Id => Id;
    }
}
