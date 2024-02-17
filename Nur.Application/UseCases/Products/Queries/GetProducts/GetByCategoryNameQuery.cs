using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Products.DTOs;

namespace Nur.Application.UseCases.Products.Queries.GetProducts;

public class GetByCategoryNameQuery : IRequest<IEnumerable<ProductDTO>>
{
    public GetByCategoryNameQuery(string categoryName) { CategoryName = Regex.Replace(categoryName, @"[^a-zA-Z]", ""); }
    public string CategoryName { get; set; }
}

public class GetByCategoryNameQueryHandler(IMapper mapper,
    IRepository<Product> repository) : IRequestHandler<GetByCategoryNameQuery, IEnumerable<ProductDTO>>
{
    public async Task<IEnumerable<ProductDTO>> Handle(GetByCategoryNameQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.SelectAll(c => c.Category.Name.Equals(request.CategoryName),
            includes: new[] { "Category", "Attachment" }).ToListAsync();
        var mappedProducts = mapper.Map<IEnumerable<ProductDTO>>(products);
        return mappedProducts;
    }
}
