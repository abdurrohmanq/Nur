using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Products.DTOs;

namespace Nur.Application.UseCases.Products.Queries.GetProducts;

public class GetByCategoryIdQuery : IRequest<IEnumerable<ProductDTO>>
{
    public GetByCategoryIdQuery(long categoryId) { CategoryId = categoryId; }
    public long CategoryId { get; set; }
}

public class GetByCategoryIdQueryHandler(IMapper mapper,
    IRepository<Product> repository) : IRequestHandler<GetByCategoryIdQuery, IEnumerable<ProductDTO>>
{
    public async Task<IEnumerable<ProductDTO>> Handle(GetByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.SelectAll(c => c.CategoryId.Equals(request.CategoryId), includes: new[] { "Category", "Attachment" }).ToListAsync();
        var mappedProducts = mapper.Map<IEnumerable<ProductDTO>>(products);
        return mappedProducts;
    }
}