using System;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.SupplierViewModels
{
    public class SupplierRequestDto
    {
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "كود المورد مطلوب")]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required(ErrorMessage = "اسم المورد مطلوب")]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string ContactInfo { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        [MaxLength(100)]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class SupplierListDto
    {
        public int SupplierId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
