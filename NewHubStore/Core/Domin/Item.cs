using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
      public  class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ItemCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string ItemName { get; set; }

        public int? ParentItemId { get; set; }

        [Required]
        public bool IsParent { get; set; }

        [MaxLength(20)]
        public string MainUnitCode { get; set; }

        [MaxLength(20)]
        public string BaseUnitCode { get; set; }

        [MaxLength(100)]
        public string ExternalBarcode { get; set; }

        [Required]
        [MaxLength(100)]
        public string InternalBarcode { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(ParentItemId))]
        public virtual Item ParentItem { get; set; }

        public virtual ICollection<Item> ChildItems { get; set; }
        public virtual ICollection<ItemUnit> ItemUnits { get; set; }
        public virtual ICollection<OpeningBalance> OpeningBalances { get; set; }
        public virtual ICollection<MovementLine> MovementLines { get; set; }
        public virtual ICollection<OrderLine> OrderLines { get; set; }
        public int AverageCost { get; set; }
        public int Cost { get; set; }
        public int UnitPrice { get; set; }
    }
}
