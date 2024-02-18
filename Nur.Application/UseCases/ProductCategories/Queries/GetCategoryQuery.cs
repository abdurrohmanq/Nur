using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Application.Exceptions;

namespace Nur.Application.UseCases.ProductCategories.Queries;

public class GetCategoryQuery : IRequest<ProductCategoryDTO>
{
    public GetCategoryQuery(long productId) { Id = productId; }
    public long Id { get; set; }
}

public class GetCategoryQueryHandler(IMapper mapper,
    IRepository<ProductCategory> repository) : IRequestHandler<GetCategoryQuery, ProductCategoryDTO>
{
    public async Task<ProductCategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        => mapper.Map<ProductCategoryDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id),
            includes: new[] { "Products.Attachment" })
            ?? throw new NotFoundException($"This category is not found with id: {request.Id}"));
}
