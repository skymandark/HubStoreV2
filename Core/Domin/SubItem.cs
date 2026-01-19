using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class SubItem
    {
        [Key]
        public int SubItemId { get; set; }

        [Required]
        public int MainItemId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameArab { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameEng { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

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
        [ForeignKey(nameof(MainItemId))]
        public virtual MainItem MainItem { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}