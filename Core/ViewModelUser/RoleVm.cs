using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public  class RoleVm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string RoleType { get; set; }
        public string RoleValue { get; set; }
        public bool IsFound { get; set; }
    }
}
