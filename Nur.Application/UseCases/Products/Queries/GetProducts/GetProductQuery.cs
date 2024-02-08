using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Products.DTOs;

namespace Nur.Application.UseCases.Products.Queries.GetProducts;

public class GetProductQuery : IRequest<ProductDTO>
{
    public GetProductQuery(long productId) { Id = productId; }
    public long Id { get; set; }
}

public class GetProductQueryHandler(IMapper mapper,
    IRepository<Product> repository) : IRequestHandler<GetProductQuery, ProductDTO>
{
    public async Task<ProductDTO> Handle(GetProductQuery request, CancellationToken cancellationToken)
        => mapper.Map<ProductDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id), includes: new[] {"Category", "Attachment" })
            ?? throw new NotFoundException($"This product is not found with id: {request.Id}"));
}
