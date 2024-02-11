using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Carts.CartItems.DTOs;

namespace Nur.Application.UseCases.Carts.CartItems.Queries.GetCartItems;

public class GetByCartIdQuery : IRequest<IEnumerable<CartItemDTO>>
{
    public GetByCartIdQuery(long cartId) { CartId = cartId; }
    public long CartId { get; set; }
}

public class GetByCartIdQueryHandler(IMapper mapper,
    IRepository<CartItem> repository) : IRequestHandler<GetByCartIdQuery, IEnumerable<CartItemDTO>>
{
    public async Task<IEnumerable<CartItemDTO>> Handle(GetByCartIdQuery request, CancellationToken cancellationToken)
       => mapper.Map<IEnumerable<CartItemDTO>>(await repository.SelectAll(u => u.CartId.Equals(request.CartId),
           includes: new[] { "Cart", "Product" })
           .ToListAsync());
}