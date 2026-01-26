using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domin
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; }

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; }

        public long FileSize { get; set; }

        [Required]
        [MaxLength(450)] // Assuming UserId is string from Identity
        public string UploadedBy { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string EntityType { get; set; }

        public int EntityId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // Navigation properties if needed, but for now, keep simple
    }
}