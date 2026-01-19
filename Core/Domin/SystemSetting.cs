using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domin
{
    public class SystemSetting
    {
        [Key]
        public int SystemSettingId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SettingKey { get; set; }

        [MaxLength(500)]
        public string SettingValue { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }
    }
}

