using AutoMapper;
using MediatR;
using Nur.Application.Commons.Interfaces;
using Nur.Application.Exceptions;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Domain.Entities.Products;

namespace Nur.Application.UseCases.ProductCategories.Commands;

public class CategoryUpdateCommand : IRequest<ProductCategoryDTO>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class CategoryUpdateCommandHandler(IMapper mapper,
    IRepository<ProductCategory> repository) : IRequestHandler<CategoryUpdateCommand, ProductCategoryDTO>
{
    public async Task<ProductCategoryDTO> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(u => u.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This category is not found with id: {request.Id}");

        entity = mapper.Map(request, entity);

        repository.Update(entity);
        await repository.SaveAsync();

        return mapper.Map<ProductCategoryDTO>(entity);
    }
}

