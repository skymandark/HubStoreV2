using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class Notification
    {
        [Key]
        public long NotificationId { get; set; }

        [Required]
        public string UserId { get; set; }

        public int? DocumentId { get; set; }

        [MaxLength(50)]
        public string DocumentType { get; set; }

        [Required]
        public int PriorityScore { get; set; } = 0;

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }

        [MaxLength(500)]
        public string Link { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
