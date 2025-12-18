using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class SavedView
    {
        [Key]
        public int SavedViewId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ViewName { get; set; }

        public string FiltersJson { get; set; }

        public string ColumnsJson { get; set; }

        public string SortOrderJson { get; set; }

        [Required]
        public bool IsDefault { get; set; } = false;

        public bool IsShared { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }

    }
}
