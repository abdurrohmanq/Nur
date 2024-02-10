using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.Queries.GetOrderItems;

public class GetByOrderIdQuery : IRequest<OrderItemDTO>
{
    public GetByOrderIdQuery(long orderItemId) { OrderId = orderItemId; }
    public long OrderId { get; set; }
}

public class GetByOrderIdQueryHandler(IMapper mapper,
    IRepository<OrderItem> repository) : IRequestHandler<GetByOrderIdQuery, OrderItemDTO>
{
    public async Task<OrderItemDTO> Handle(GetByOrderIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<OrderItemDTO>(await repository.SelectAsync(u => u.OrderId.Equals(request.OrderId), includes: new[] { "Order", "Product" })
            ?? throw new NotFoundException($"This order is not found with id: {request.OrderId}"));
}