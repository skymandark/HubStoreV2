using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.UserDetail  
{
    public interface IBaseRoleDto
    {
        object Id { get; }
        bool IsDefaultId();
    }
}
