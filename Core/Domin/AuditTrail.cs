using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class AuditTrail
    {
        [Key]
        public long LogId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ActionType { get; set; }

        [Required]
        public int DocumentTypeId { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [Required]
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;

        public string OldValues { get; set; }

        public string NewValues { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [MaxLength(50)]
        public string ClientIp { get; set; }

        [MaxLength(200)]
        public string ClientDevice { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        public virtual DocumentType DocumentType { get; set; }
    }
}
