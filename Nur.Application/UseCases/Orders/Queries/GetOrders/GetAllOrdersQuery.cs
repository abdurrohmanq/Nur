using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.DTOs;

namespace Nur.Application.UseCases.Orders.Queries.GetOrders;

public class GetAllOrdersQuery : IRequest<IEnumerable<OrderDTO>>
{ }

public class GetAllOrderQueryHandler(IMapper mapper,
    IRepository<Order> repository) : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDTO>>
{
    public async Task<IEnumerable<OrderDTO>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await repository.SelectAll(includes: new[] { "User", "Address", "Supplier", "Payment", "OrderItems.Product" }).ToListAsync();
        var mappedOrders = mapper.Map<IEnumerable<OrderDTO>>(orders);
        return mappedOrders;
    }
}