using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class ApprovalStatus
    {
        [Key]
        public int ApprovalStatusId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Movement> Movements { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
    }
}
