using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.DTOs;

namespace Nur.Application.UseCases.Orders.Queries.GetOrders;

public class GetByUserIdQuery : IRequest<IEnumerable<OrderDTO>>
{
    public GetByUserIdQuery(long userId) { UserId = userId; }
    public long UserId { get; set; }
}

public class GetByUserIdQueryHandler(IMapper mapper,
    IRepository<Order> repository) : IRequestHandler<GetByUserIdQuery, IEnumerable<OrderDTO>>
{
    public async Task<IEnumerable<OrderDTO>> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<OrderDTO>>(await repository.SelectAll(u => u.UserId.Equals(request.UserId),
           includes: new[] { "User", "Address", "Supplier", "Payment", "OrderItems.Product" })
           .ToListAsync());
}
