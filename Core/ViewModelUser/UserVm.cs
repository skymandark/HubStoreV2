using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModel
{
    public class UserVm
    {
        public string UserId { get; set; }
        public int UserSerial { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? EmployeeId { get; set; }
        public bool Active { get; set; }
        public bool IsAdmin { get; set; }
    }
}
