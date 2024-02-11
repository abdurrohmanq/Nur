using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.Queries.GetOrderItems;

public class GetOrderItemByProductIdQuery : IRequest<IEnumerable<OrderItemDTO>>
{
    public GetOrderItemByProductIdQuery(long productId) { ProductId = productId; }
    public long ProductId { get; set; }
}

public class GetOrderItemByProductIdQueryHandler(IMapper mapper,
    IRepository<OrderItem> repository) : IRequestHandler<GetOrderItemByProductIdQuery, IEnumerable<OrderItemDTO>>
{
    public async Task<IEnumerable<OrderItemDTO>> Handle(GetOrderItemByProductIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<OrderItemDTO>>(await repository.SelectAll(u => u.ProductId.Equals(request.ProductId),
           includes: new[] { "Order", "Product" })
           .ToListAsync());
}