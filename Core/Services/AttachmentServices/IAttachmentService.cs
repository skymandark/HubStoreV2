using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Domin;

namespace Core.Services.AttachmentServices
{
    public interface IAttachmentService
    {
        Task<Attachment> UploadFileAsync(Stream fileStream, string fileName, string contentType, long fileSize, string entityType, int entityId, string userId, string description = null);
        Task<Attachment> GetAttachmentAsync(int id);
        Task<IEnumerable<Attachment>> GetAttachmentsForEntityAsync(string entityType, int entityId);
        Task DeleteAttachmentAsync(int id, string userId);
    }
}