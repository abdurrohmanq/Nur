using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Products.DTOs;

namespace Nur.Application.UseCases.Products.Queries.GetProducts;

public class GetProductByNameQuery : IRequest<ProductDTO>
{
    public GetProductByNameQuery(string name) { Name = name; }
    public string Name { get; set; }
}

public class GetProductByNameQueryHandler(IMapper mapper,
    IRepository<Product> repository) : IRequestHandler<GetProductByNameQuery, ProductDTO>
{
    public async Task<ProductDTO> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        => mapper.Map<ProductDTO>(await repository.SelectAsync(u => u.Name.Equals(request.Name),
            includes: new[] { "Category", "Attachment" }));
}
