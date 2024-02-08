using AutoMapper;
using MediatR;
using Nur.Application.Commons.Interfaces;
using Nur.Application.Exceptions;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Domain.Entities.Products;

namespace Nur.Application.UseCases.ProductCategories.Commands;

public class CategoryCreateCommand : IRequest<ProductCategoryDTO>
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class CategoryCreateCommandHandler(IMapper mapper,
    IRepository<ProductCategory> repository) : IRequestHandler<CategoryCreateCommand, ProductCategoryDTO>
{
    public async Task<ProductCategoryDTO> Handle(CategoryCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(c => c.Name.ToLower().Equals(request.Name.ToLower()));
        if (entity is not null)
            throw new AlreadyExistException($"This category already exist! With name: {request.Name}");

        entity = mapper.Map<ProductCategory>(request);
        await repository.InsertAsync(entity);
        await repository.SaveAsync();

        return mapper.Map<ProductCategoryDTO>(entity);
    }
}
