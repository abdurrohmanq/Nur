using MediatR;
using Nur.Domain.Entities.Attachments;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Attachments.Commands;

public class AttachmentRemoveCommand : IRequest<bool>
{
    public Attachment Attachment { get; set; }
}

public class AttachmentRemoveCommandHandler(IRepository<Attachment> repository) : IRequestHandler<AttachmentRemoveCommand, bool>
{
    public async Task<bool> Handle(AttachmentRemoveCommand request, CancellationToken cancellationToken)
    {
        if (request.Attachment is null) 
            return false;
        var attachment = repository.SelectAsync(a => a.Id.Equals(request.Attachment.Id));
        if (attachment is null)
            return false;

        repository.Delete(request.Attachment);
        await repository.SaveAsync();
        return true;
    }
}
