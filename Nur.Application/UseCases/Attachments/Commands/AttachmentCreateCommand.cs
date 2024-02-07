using MediatR;
using Microsoft.AspNetCore.Http;
using Nur.Application.Commons.Helpers;
using Nur.Application.Commons.Interfaces;
using Nur.Domain.Entities.Attachments;

namespace Nur.Application.UseCases.Attachments.Commands;

public class AttachmentCreateCommand : IRequest<Attachment>
{
    public IFormFile FormFile { get; set; }
}

public class AttachmentCreateCommandHandler(IRepository<Attachment> repository,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<AttachmentCreateCommand, Attachment>
{
    public async Task<Attachment> Handle(AttachmentCreateCommand request, CancellationToken cancellationToken)
    {
        var webRootPath = Path.Combine(PathHelper.WebRootPath, "Images");

        if (!Directory.Exists(webRootPath))
            Directory.CreateDirectory(webRootPath);

        var fileExtension = Path.GetExtension(request.FormFile.FileName);
        var fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
        var filePath = Path.Combine(webRootPath, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.FormFile.CopyToAsync(fileStream);
        }

        var imageUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Images/{fileName}";

        var attachment = new Attachment()
        {
            FileName = fileName,
            FilePath = imageUrl,
        };

        await repository.InsertAsync(attachment);
        await repository.SaveAsync();

        return attachment;
    }
}