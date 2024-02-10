using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.Queries.GetOrderItems;

public class GetOrderItemQuery : IRequest<OrderItemDTO>
{
    public GetOrderItemQuery(long orderId) { Id = orderId; }
    public long Id { get; set; }
}

public class GetOrderItemQueryHandler(IMapper mapper,
    IRepository<OrderItem> repository) : IRequestHandler<GetOrderItemQuery, OrderItemDTO>
{
    public async Task<OrderItemDTO> Handle(GetOrderItemQuery request, CancellationToken cancellationToken)
        => mapper.Map<OrderItemDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id), includes: new[] { "Order", "Product"})
            ?? throw new NotFoundException($"This order is not found with id: {request.Id}"));
}
