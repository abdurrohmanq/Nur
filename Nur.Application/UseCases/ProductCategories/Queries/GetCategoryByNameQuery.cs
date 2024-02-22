using AutoMapper;
using MediatR;
using Nur.Application.Commons.Interfaces;
using Nur.Application.Exceptions;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Domain.Entities.Products;

namespace Nur.Application.UseCases.ProductCategories.Queries;

public class GetCategoryByNameQuery : IRequest<ProductCategoryDTO>
{
    public GetCategoryByNameQuery(string name) { Name = name; }
    public string Name { get; set; }
}

public class GetCategoryByNameQueryHandler(IMapper mapper,
    IRepository<ProductCategory> repository) : IRequestHandler<GetCategoryByNameQuery, ProductCategoryDTO>
{
    public async Task<ProductCategoryDTO> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
        => mapper.Map<ProductCategoryDTO>(await repository.SelectAsync(u => u.Name.Equals(request.Name),
            includes: new[] { "Products.Attachment" })
            ?? throw new NotFoundException($"This category is not found with name: {request.Name}"));
}
