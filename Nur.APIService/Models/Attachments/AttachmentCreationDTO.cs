using Microsoft.AspNetCore.Http;

namespace Nur.APIService.Models.Attachments;

public class AttachmentCreationDTO
{
    public IFormFile FormFile { get; set; }
}
