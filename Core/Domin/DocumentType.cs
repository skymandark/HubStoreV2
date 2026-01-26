using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ICollection<ApprovalChain> ApprovalChains { get; set; }
        public string CreatedBy { get; set; }
    }
}
