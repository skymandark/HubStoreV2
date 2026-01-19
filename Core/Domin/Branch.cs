using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class Branch
    {
        [Key]
        public int BranchId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Location { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public virtual ICollection<Movement> MovementsFrom { get; set; }
        public virtual ICollection<Movement> MovementsTo { get; set; }
        public virtual ICollection<MovementLine> MovementLines { get; set; }
        public virtual ICollection<Order> OrdersFrom { get; set; }
        public virtual ICollection<Order> OrdersTo { get; set; }
        public virtual ICollection<OpeningBalance> OpeningBalances { get; set; }
        public virtual ICollection<TransferOrderHeader> TransferOrderHeadersFrom { get; set; }
        public virtual ICollection<TransferOrderHeader> TransferOrderHeadersTo { get; set; }
        public virtual ICollection<DirectReceiptHeader> DirectReceiptHeaders { get; set; }
        public virtual ICollection<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public virtual ICollection<StockInHeader> StockInHeaders { get; set; }
        public virtual ICollection<ReturnOrderHeader> ReturnOrderHeaders { get; set; }
        public virtual ICollection<StockOutReturnHeader> StockOutReturnHeaders { get; set; }
        public int BaseId { get; set; }
        public string Name_Arab { get; set; }
    }
}
