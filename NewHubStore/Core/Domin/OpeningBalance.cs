using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class OpeningBalance
    {
        [Key]
        public int OpeningBalanceId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int FiscalYear { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal OpeningQuantityBase { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostValue { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        [ForeignKey(nameof(FiscalYear))]
        public virtual FiscalYear FiscalYearNavigation { get; set; }
        public int Quantity { get; set; }
    }
}
