using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class DirectReceiptHeader
    {
        [Key]
        public int DirectReceiptId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DirectReceiptCode { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [MaxLength(100)]
        public string ReferenceInvoiceNumber { get; set; }

        [MaxLength(1000)]
        public string RemarksArab { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVat { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetTotal { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }
 
        [ForeignKey(nameof(StatusId))]
        public virtual ApprovalStatus Status { get; set; }



        public virtual ICollection<DirectReceiptDetail> DirectReceiptDetails { get; set; }
        public virtual ICollection<SupplierInvoiceHeader> SupplierInvoiceHeaders { get; set; }
        public virtual ICollection<Order> Orders { get; set; } // Linked purchase orders
    }
}