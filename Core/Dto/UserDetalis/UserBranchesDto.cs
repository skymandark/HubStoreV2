using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserDetalis
{
    public  class UserBranchesDto
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public bool IsFound { get; set; }
    }
}
