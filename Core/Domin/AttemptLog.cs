using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class AttemptLog
    {
        [Key]
        public long AttemptLogId { get; set; }

        [Required]
        public string UserId { get; set; }

        public int? DocumentId { get; set; }

        [MaxLength(50)]
        public string DocumentType { get; set; }

        [Required]
        [MaxLength(200)]
        public string ActionAttempted { get; set; }

        [MaxLength(1000)]
        public string ValidationMessage { get; set; }

        [MaxLength(1000)]
        public string Reason { get; set; }

        [MaxLength(50)]
        public string ClientIp { get; set; }

        [MaxLength(200)]
        public string ClientDevice { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }
    }
}
