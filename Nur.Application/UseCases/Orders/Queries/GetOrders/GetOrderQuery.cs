using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.DTOs;

namespace Nur.Application.UseCases.Orders.Queries.GetOrders;

public class GetOrderQuery : IRequest<OrderDTO>
{
    public GetOrderQuery(long orderId) { Id = orderId; }
    public long Id { get; set; }
}

public class GetOrderQueryHandler(IMapper mapper,
    IRepository<Order> repository) : IRequestHandler<GetOrderQuery, OrderDTO>
{
    public async Task<OrderDTO> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        => mapper.Map<OrderDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id), includes: new[] { "User", "Address", "Supplier", "Payment", "OrderItems.Product" })
            ?? throw new NotFoundException($"This order is not found with id: {request.Id}"));
}
