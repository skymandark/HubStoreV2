using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserDetalis
{
    public  class UserApproveDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<ApproveDefinitionDto> Approves { get; set; }
    }

    public class ApproveDefinitionDto
    {
        public int Id { get; set; }
        public int StepId { get; set; }
        public string UserId { get; set; }
        public string ApproveName { get; set; }
        public string TableName { get; set; }
        public string SystemName { get; set; }

    }
}
