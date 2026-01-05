using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domin
{
    public class SupplierInvoiceHeader
    {
        [Key]
        public int SupplierInvoiceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public int? DirectReceiptId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VatAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

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

        [ForeignKey(nameof(DirectReceiptId))]
        public virtual DirectReceiptHeader DirectReceiptHeader { get; set; }

        public virtual ICollection<SupplierInvoiceDetail> SupplierInvoiceDetails { get; set; }
    }
}