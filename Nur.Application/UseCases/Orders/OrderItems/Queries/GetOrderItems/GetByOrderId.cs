using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.Queries.GetOrderItems;

public class GetByOrderIdQuery : IRequest<IEnumerable<OrderItemDTO>>
{
    public GetByOrderIdQuery(long orderItemId) { OrderId = orderItemId; }
    public long OrderId { get; set; }
}

public class GetByOrderIdQueryHandler(IMapper mapper,
    IRepository<OrderItem> repository) : IRequestHandler<GetByOrderIdQuery, IEnumerable<OrderItemDTO>>
{
    public async Task<IEnumerable<OrderItemDTO>> Handle(GetByOrderIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<OrderItemDTO>>(await repository.SelectAll(u => u.OrderId.Equals(request.OrderId),
           includes: new[] { "Order", "Product" })
           .ToListAsync());
}