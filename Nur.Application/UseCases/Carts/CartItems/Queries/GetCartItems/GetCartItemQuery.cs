using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Carts.CartItems.DTOs;

namespace Nur.Application.UseCases.Carts.CartItems.Queries.GetCartItems;

public class GetCartItemQuery : IRequest<CartItemDTO>
{
    public GetCartItemQuery(long id) { Id = id; }
    public long Id { get; set; }
}

public class GetCartItemQueryHandler(IMapper mapper,
    IRepository<CartItem> repository) : IRequestHandler<GetCartItemQuery, CartItemDTO>
{
    public async Task<CartItemDTO> Handle(GetCartItemQuery request, CancellationToken cancellationToken)
       => mapper.Map<CartItemDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id))
           ?? throw new NotFoundException($"This cart item is not found with id: {request.Id}"));
}