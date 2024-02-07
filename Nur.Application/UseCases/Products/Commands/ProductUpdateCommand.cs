using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Attachments.Commands;
using Nur.Application.UseCases.Products.DTOs;
using Nur.Domain.Entities.Attachments;
using Nur.Domain.Entities.Products;
using Nur.Domain.Enums;
using Unit = Nur.Domain.Enums.Unit;

namespace Nur.Application.UseCases.Products.Commands;

public class ProductUpdateCommand : IRequest<ProductDTO>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public long Quantity { get; set; }
    public string Description { get; set; }
    public Unit Unit { get; set; }

    public long CategoryId { get; set; }
    public IFormFile Attachment { get; set; }
}
public class ProductUpdateCommandHandler(IMapper mapper,
    IRepository<Product> repository,
    IRequestHandler<AttachmentCreateCommand, Attachment> attachmentCreateHandler,
    IRequestHandler<AttachmentRemoveCommand, bool> attachmentRemoveHandler) : IRequestHandler<ProductUpdateCommand, ProductDTO>
{
    public async Task<ProductDTO> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(p => p.Id.Equals(request.Id))
            ?? throw new($"This product not found with Id: {request.Id}");

        entity.Name = request.Name;
        entity.Price = request.Price;
        entity.Quantity = request.Quantity;
        entity.Description = request.Description;
        entity.Unit = request.Unit;
        entity.CategoryId = request.CategoryId;
        if (request.Attachment is not null)
        {
            var attachment = await attachmentCreateHandler.Handle(new AttachmentCreateCommand { FormFile = request.Attachment }, cancellationToken);
            await attachmentRemoveHandler.Handle(new AttachmentRemoveCommand { Attachment = entity.Attachment }, cancellationToken);
            entity.Attachment = attachment;
            entity.AttachmentId = attachment.Id;
        }

        repository.Update(entity);
        await repository.SaveAsync();

        return mapper.Map<ProductDTO>(entity);
    }
}
