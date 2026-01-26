using Core.Services.AttachmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HubStoreV2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private readonly UserManager<Core.Domin.AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public AttachmentController(
            IAttachmentService attachmentService,
            UserManager<Core.Domin.AppUser> userManager,
            IWebHostEnvironment environment)
        {
            _attachmentService = attachmentService;
            _userManager = userManager;
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromForm] string entityType,
            [FromForm] int entityId,
            [FromForm] string description = null)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                var attachment = await _attachmentService.UploadFileAsync(file.OpenReadStream(), file.FileName, file.ContentType, file.Length, entityType, entityId, user.Id, description);
                return Ok(new { attachment.Id, attachment.FileName, attachment.UploadedDate });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var attachment = await _attachmentService.GetAttachmentAsync(id);
                if (attachment == null)
                    return NotFound();

                var filePath = Path.Combine(_environment.WebRootPath, attachment.FilePath);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, attachment.ContentType, attachment.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("entity/{entityType}/{entityId}")]
        public async Task<IActionResult> GetAttachmentsForEntity(string entityType, int entityId)
        {
            try
            {
                var attachments = await _attachmentService.GetAttachmentsForEntityAsync(entityType, entityId);
                return Ok(attachments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                await _attachmentService.DeleteAttachmentAsync(id, user.Id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}