using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.Queries.GetOrderItems;

public class GetAllOrderItemsQuery : IRequest<IEnumerable<OrderItemDTO>>
{ }

public class GetAllOrderItemQueryHandler(IMapper mapper,
    IRepository<OrderItem> repository) : IRequestHandler<GetAllOrderItemsQuery, IEnumerable<OrderItemDTO>>
{
    public async Task<IEnumerable<OrderItemDTO>> Handle(GetAllOrderItemsQuery request, CancellationToken cancellationToken)
    {
        var orderItems = await repository.SelectAll(includes: new[] { "Order", "Product" }).ToListAsync();
        var mappedOrdersItems = mapper.Map<IEnumerable<OrderItemDTO>>(orderItems);
        return mappedOrdersItems;
    }
}