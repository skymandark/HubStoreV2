using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameArab { get; set; }

        [Required]
        [MaxLength(100)]
        public string NameEng { get; set; }

        [MaxLength(20)]
        public string Code { get; set; }

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
        [InverseProperty("SellMainPackage")]
        public virtual ICollection<Item> SellMainItems { get; set; }
        [InverseProperty("SellSubPackage")]
        public virtual ICollection<Item> SellSubItems { get; set; }
        [InverseProperty("BuyMainPackage")]
        public virtual ICollection<Item> BuyMainItems { get; set; }
        [InverseProperty("BuySubPackage")]
        public virtual ICollection<Item> BuySubItems { get; set; }
        public virtual ICollection<ItemPackage> ItemPackages { get; set; }
    }
}