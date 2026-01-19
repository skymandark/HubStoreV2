using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class TradeType
    {
        [Key]
        public int TradeTypeId { get; set; }

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
        [InverseProperty("BuyTradeType")]
        public virtual ICollection<Item> BuyItems { get; set; }
        [InverseProperty("SellTradeType")]
        public virtual ICollection<Item> SellItems { get; set; }
    }
}