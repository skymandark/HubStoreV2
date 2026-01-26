using System;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels
{
    public class AttachmentViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string Description { get; set; }

        // Additional properties for UI
        public string FileSizeDisplay => FileSize > 1024 * 1024
            ? $"{FileSize / (1024 * 1024.0):F2} MB"
            : $"{FileSize / 1024.0:F2} KB";
    }

    public class UploadAttachmentViewModel
    {
        [Required]
        [MaxLength(50)]
        public string EntityType { get; set; }

        [Required]
        public int EntityId { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}