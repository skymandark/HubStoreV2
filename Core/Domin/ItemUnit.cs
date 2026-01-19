using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class ItemUnit
    {
        [Key]
        public int ItemUnitId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UnitCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string UnitName { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal ConversionToBase { get; set; }

        public int? ParentUnitId { get; set; }

        public bool IsDefaultForDisplay { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(ParentUnitId))]
        public virtual ItemUnit ParentUnit { get; set; }

        public virtual ICollection<ItemUnit> ChildUnits { get; set; }
        public virtual ICollection<ItemUnitHistory> ItemUnitHistories { get; set; }
        public int? ConversionFactor { get; set; }
    }
}
