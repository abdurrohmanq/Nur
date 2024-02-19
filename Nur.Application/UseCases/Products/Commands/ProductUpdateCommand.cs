using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nur.Application.Exceptions;
using Unit = Nur.Domain.Enums.Unit;
using Nur.Domain.Entities.Products;
using Nur.Domain.Entities.Attachments;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Products.DTOs;
using Nur.Application.UseCases.Attachments.Commands;

namespace Nur.Application.UseCases.Products.Commands;

public class ProductUpdateCommand : IRequest<ProductDTO>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public double? Quantity { get; set; }
    public string Description { get; set; }
    public Unit Unit { get; set; }

    public long CategoryId { get; set; }
    public IFormFile Attachment { get; set; }
}
public class ProductUpdateCommandHandler(IMapper mapper,
    IRepository<Product> repository,
    IRepository<ProductCategory> categoryRepository,
    IRequestHandler<AttachmentCreateCommand, Attachment> attachmentCreateHandler,
    IRequestHandler<AttachmentRemoveCommand, bool> attachmentRemoveHandler) : IRequestHandler<ProductUpdateCommand, ProductDTO>
{
    public async Task<ProductDTO> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(p => p.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This product not found with Id: {request.Id}");

        if (entity.CategoryId != request.Id)
        {
            var category = await categoryRepository.SelectAsync(c => c.Id.Equals(request.CategoryId))
                ?? throw new NotFoundException($"This category is not found with id: {request.CategoryId}");
            entity.CategoryId = request.CategoryId;
            entity.Category = category;
        }

        entity.Name = request.Name;
        entity.Price = request.Price;
        entity.Quantity = request.Quantity;
        entity.Description = request.Description;
        entity.Unit = request.Unit;
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
