using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class ItemUnitHistory
    {
        [Key]
        public int ItemUnitHistoryId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UnitCode { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal OldConversionToBase { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal NewConversionToBase { get; set; }

        [Required]
        public string ChangedByUserId { get; set; }

        [Required]
        public DateTime ChangedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string Notes { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(ChangedByUserId))]
        public virtual AppUser ChangedByUser { get; set; }
    }
}
