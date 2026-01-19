using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class ExportLog
    {
        [Key]
        public long ExportLogId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ExportType { get; set; }

        public string FiltersJson { get; set; }

        [Required]
        public int RowCount { get; set; }

        [MaxLength(500)]
        public string FileReference { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }
    }
}
