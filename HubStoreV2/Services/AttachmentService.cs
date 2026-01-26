using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Core.Services.AttachmentServices;

namespace HubStoreV2.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        // File validation constants
        private readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public AttachmentService(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
        }

        public async Task<Attachment> UploadFileAsync(Stream fileStream, string fileName, string contentType, long fileSize, string entityType, int entityId, string userId, string description = null)
        {
            if (fileStream == null || fileSize == 0)
                throw new ArgumentException("File is required");

            // Validate file
            if (!IsValidFile(fileName, fileSize))
                throw new ArgumentException("Invalid file type or size");

            // Generate unique file name
            var extension = Path.GetExtension(fileName);
            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");

            // Ensure directory exists
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            // Create attachment record
            var attachment = new Attachment
            {
                FileName = fileName,
                FilePath = Path.Combine("uploads", uniqueFileName), // Relative path
                ContentType = contentType,
                FileSize = fileSize,
                UploadedBy = userId,
                UploadedDate = DateTime.UtcNow,
                EntityType = entityType,
                EntityId = entityId,
                Description = description
            };

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            return attachment;
        }

        public async Task<Attachment> GetAttachmentAsync(int id)
        {
            return await _context.Attachments.FindAsync(id);
        }

        public async Task<IEnumerable<Attachment>> GetAttachmentsForEntityAsync(string entityType, int entityId)
        {
            return await _context.Attachments
                .Where(a => a.EntityType == entityType && a.EntityId == entityId)
                .OrderByDescending(a => a.UploadedDate)
                .ToListAsync();
        }

        public async Task DeleteAttachmentAsync(int id, string userId)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
                throw new KeyNotFoundException("Attachment not found");

            // Optional: Check if user can delete (e.g., if UploadedBy == userId or admin)

            // Delete file
            var fullPath = Path.Combine(_environment.WebRootPath, attachment.FilePath);
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            // Delete record
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
        }

        private bool IsValidFile(string fileName, long fileSize)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return AllowedExtensions.Contains(extension) && fileSize <= MaxFileSize;
        }
    }
}