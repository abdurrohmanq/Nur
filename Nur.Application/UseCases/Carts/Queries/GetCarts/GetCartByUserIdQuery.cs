using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Carts;
using Nur.Application.Exceptions;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Carts.DTOs;

namespace Nur.Application.UseCases.Carts.Queries.GetCarts;

public class GetCartByUserIdQuery : IRequest<CartDTO>
{
    public GetCartByUserIdQuery(long userId) { UserId = userId; }
    public long UserId { get; set; }
}

public class GetCartQueryHandler(IMapper mapper,
    IRepository<Cart> repository) : IRequestHandler<GetCartByUserIdQuery, CartDTO>
{
    public async Task<CartDTO> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<CartDTO>(await repository.SelectAsync(u => u.UserId.Equals(request.UserId),
            includes: new[] {"User", "CartItems.Product" })
            ?? throw new NotFoundException($"This user was not found with id: {request.UserId}"));
}