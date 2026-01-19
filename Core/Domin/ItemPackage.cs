using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class ItemPackage
    {
        [Key]
        public int ItemPackageId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int PackageId { get; set; }

        public int? PackageCount { get; set; }

        public bool IsDefault { get; set; } = false;

        [MaxLength(50)]
        public string PackageTransactionType { get; set; }

        public int? LevelId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Navigation Properties
        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(PackageId))]
        public virtual Package Package { get; set; }
    }
}