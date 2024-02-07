using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Products.DTOs;

namespace Nur.Application.UseCases.Products.Queries.GetProducts;

public class GetAllProductsQuery : IRequest<IEnumerable<ProductDTO>>
{
}

public class GetAllProductQueryHandler(IMapper mapper,
    IRepository<Product> repository) : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
{
    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.SelectAll(includes: new[] { "Category", "Attachment" }).ToListAsync();
        var mappedProducts = mapper.Map<IEnumerable<ProductDTO>>(products);
        return mappedProducts;
    }
}
