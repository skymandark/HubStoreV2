using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Core.Domin.Approval;

namespace Core.Domin
{
    public class ItemFlag
    {
        [Key]
        public int ItemFlagId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int FlagId { get; set; }

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

        //[ForeignKey(nameof(FlagId))]
        //public virtual Flag Flag { get; set; }
    }
}