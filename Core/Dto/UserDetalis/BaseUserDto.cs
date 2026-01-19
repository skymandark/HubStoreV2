using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserDetalis
{
    public  class BaseUserDto
    {
        public string Id { get; set; }

        public bool IsDefaultId() => string.IsNullOrEmpty(Id);
    }
}
