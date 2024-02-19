using AutoMapper;
using MediatR;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Carts.CartItems.DTOs;

namespace Nur.Application.UseCases.Carts.CartItems.Queries.GetCartItems;

public class GetByProductIdQuery : IRequest<CartItemDTO>
{
    public GetByProductIdQuery(long productId) { ProductId = productId; }
    public long ProductId { get; set; }
}

public class GetByProductIdQueryHandler(IMapper mapper,
    IRepository<CartItem> repository) : IRequestHandler<GetByProductIdQuery, CartItemDTO>
{
    public async Task<CartItemDTO> Handle(GetByProductIdQuery request, CancellationToken cancellationToken)
       => mapper.Map<CartItemDTO>(await repository.SelectAsync(u => u.ProductId.Equals(request.ProductId),
           includes: new[] { "Cart", "Product" }));
}