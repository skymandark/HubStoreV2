using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class OrderLine
    {
        [Key]
        public int OrderLineId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int LineNo { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        [MaxLength(20)]
        public string UnitCode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal QtyOrdered { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal QtyReceived { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal ConversionUsedToBase { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,6)")]
        public decimal QtyBaseOrdered { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal QtyBaseReceived { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostValue { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxRate { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string LineStatus { get; set; }

        [MaxLength(100)]
        public string ExternalBarcode { get; set; }

        [MaxLength(100)]
        public string InternalBarcode { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        [ForeignKey(nameof(ItemId))]
        public virtual Item Item { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovedDate { get; set; }
    }
}
