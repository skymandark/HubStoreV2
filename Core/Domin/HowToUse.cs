using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Domin
{
    public class HowToUse
    {
        [Key]
        public int HowToUseId { get; set; }

        [Required]
        [MaxLength(200)]
        public string DescriptionArab { get; set; }

        [Required]
        [MaxLength(200)]
        public string DescriptionEng { get; set; }

        [MaxLength(1000)]
        public string Instructions { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation Properties
        public virtual ICollection<Item> Items { get; set; }
    }
}