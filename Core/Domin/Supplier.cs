using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string ContactInfo { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public virtual ICollection<Movement> Movements { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<DirectReceiptHeader> DirectReceiptHeaders { get; set; }
        public virtual ICollection<SupplierInvoiceHeader> SupplierInvoiceHeaders { get; set; }
        public virtual ICollection<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public virtual ICollection<StockInHeader> StockInHeaders { get; set; }
        public virtual ICollection<ReturnOrderHeader> ReturnOrderHeaders { get; set; }
        public virtual ICollection<StockOutReturnHeader> StockOutReturnHeaders { get; set; }
    }
}
