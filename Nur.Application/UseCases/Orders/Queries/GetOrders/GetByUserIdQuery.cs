using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.DTOs;

namespace Nur.Application.UseCases.Orders.Queries.GetOrders;

public class GetByUserIdQuery : IRequest<OrderDTO>
{
    public GetByUserIdQuery(long userId) { UserId = userId; }
    public long UserId { get; set; }
}

public class GetByUserIdQueryHandler(IMapper mapper,
    IRepository<Order> repository) : IRequestHandler<GetByUserIdQuery, OrderDTO>
{
    public async Task<OrderDTO> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<OrderDTO>(await repository.SelectAsync(u => u.UserId.Equals(request.UserId),
           includes: new[] { "User", "Address", "Supplier", "Payment", "OrderItems.Product" })
            ?? throw new NotFoundException($"This user is not found with id: {request.UserId}"));
}
