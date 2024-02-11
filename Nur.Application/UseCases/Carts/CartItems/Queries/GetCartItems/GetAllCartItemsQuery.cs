using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Carts.CartItems.DTOs;

namespace Nur.Application.UseCases.Carts.CartItems.Queries.GetCartItems;

public class GetAllCartItemsQuery : IRequest<IEnumerable<CartItemDTO>>
{ }

public class GetAllCartItemsQueryHandler(IMapper mapper,
    IRepository<CartItem> repository) : IRequestHandler<GetAllCartItemsQuery, IEnumerable<CartItemDTO>>
{
    public async Task<IEnumerable<CartItemDTO>> Handle(GetAllCartItemsQuery request, CancellationToken cancellationToken)
    {
        var users = await repository.SelectAll(includes: new[] {"Cart", "Product"}).ToListAsync();
        var mappedUsers = mapper.Map<IEnumerable<CartItemDTO>>(users);
        return mappedUsers;
    }
}