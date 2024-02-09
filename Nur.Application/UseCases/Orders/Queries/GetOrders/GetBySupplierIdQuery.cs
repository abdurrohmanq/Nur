using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.DTOs;

namespace Nur.Application.UseCases.Orders.Queries.GetOrders;

public class GetBySupplierIdQuery : IRequest<OrderDTO>
{
    public GetBySupplierIdQuery(long supplierId) { SupplierId = supplierId; }
    public long SupplierId { get; set; }
}

public class GetBySupplierIdQueryHandler(IMapper mapper,
    IRepository<Order> repository) : IRequestHandler<GetBySupplierIdQuery, OrderDTO>
{
    public async Task<OrderDTO> Handle(GetBySupplierIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<OrderDTO>(await repository.SelectAsync(u => u.SupplierId.Equals(request.SupplierId),
           includes: new[] { "User", "Address", "Supplier", "Payment", "OrderItems.Product" })
            ?? throw new NotFoundException($"This supplier is not found with id: {request.SupplierId}"));
}