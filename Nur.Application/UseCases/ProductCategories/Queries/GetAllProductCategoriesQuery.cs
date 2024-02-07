using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.ProductCategories.DTOs;

namespace Nur.Application.UseCases.ProductCategories.Queries;

public class GetAllProductCategoriesQuery : IRequest<IEnumerable<ProductCategoryDTO>>
{ }

public class GetAllProductCategoriesQueryHandler(IMapper mapper,
    IRepository<ProductCategory> repository) : IRequestHandler<GetAllProductCategoriesQuery, IEnumerable<ProductCategoryDTO>>
{
    public async Task<IEnumerable<ProductCategoryDTO>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await repository.SelectAll(includes: new[] { "Products.Attachment" }).ToListAsync();
        var mappedCategories = mapper.Map<IEnumerable<ProductCategoryDTO>>(categories);
        return mappedCategories;
    }
}