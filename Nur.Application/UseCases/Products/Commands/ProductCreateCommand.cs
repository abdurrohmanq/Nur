using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nur.Application.Commons.Helpers;
using Nur.Application.Commons.Interfaces;
using Nur.Application.Exceptions;
using Nur.Application.UseCases.Attachments.Commands;
using Nur.Application.UseCases.Products.DTOs;
using Nur.Domain.Entities.Attachments;
using Nur.Domain.Entities.Products;
using Unit = Nur.Domain.Enums.Unit;

namespace Nur.Application.UseCases.Products.Commands;

public class ProductCreateCommand : IRequest<ProductDTO>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public double? Quantity { get; set; }
    public string Description { get; set; }
    public Unit Unit { get; set; }
    public long CategoryId { get; set; }
    public IFormFile Attachment { get; set; }
}

public class ProductCreateCommandHandler(IMapper mapper,
    IRepository<Product> repository,
    IRepository<ProductCategory> categoryRepository,
    IRequestHandler<AttachmentCreateCommand, Attachment> attachmentHandler) : IRequestHandler<ProductCreateCommand, ProductDTO>
{
    public async Task<ProductDTO> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(p => p.Name.Equals(request.Name));
        if (entity is not null)
            throw new AlreadyExistException($"This product already exist with name: {request.Name}");

        var category = await categoryRepository.SelectAsync(c => c.Id.Equals(request.CategoryId))
            ?? throw new NotFoundException($"Category is not found with id: {request.CategoryId}");

        entity = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Quantity = request.Quantity,
            Description = request.Description,
            Unit = request.Unit,
            CategoryId = category.Id,
            Category = category
        };

        if (request.Attachment is not null)
        {
            var attachment = await attachmentHandler.Handle(new AttachmentCreateCommand { FormFile = request.Attachment }, cancellationToken);
            entity.Attachment = attachment;
            entity.AttachmentId = attachment.Id;
        }

        await repository.InsertAsync(entity);
        await repository.SaveAsync();

        return mapper.Map<ProductDTO>(entity);
    }
}