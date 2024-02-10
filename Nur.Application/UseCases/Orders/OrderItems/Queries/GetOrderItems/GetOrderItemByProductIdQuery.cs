using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.Queries.GetOrderItems;

public class GetOrderItemByProductIdQuery : IRequest<OrderItemDTO>
{
    public GetOrderItemByProductIdQuery(long productId) { ProductId = productId; }
    public long ProductId { get; set; }
}

public class GetOrderItemByProductIdQueryHandler(IMapper mapper,
    IRepository<OrderItem> repository) : IRequestHandler<GetOrderItemByProductIdQuery, OrderItemDTO>
{
    public async Task<OrderItemDTO> Handle(GetOrderItemByProductIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<OrderItemDTO>(await repository.SelectAsync(u => u.ProductId.Equals(request.ProductId), includes: new[] { "Order", "Product" })
            ?? throw new NotFoundException($"This product is not found with id: {request.ProductId}"));
}