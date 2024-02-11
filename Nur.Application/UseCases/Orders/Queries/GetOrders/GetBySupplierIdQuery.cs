using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.DTOs;

namespace Nur.Application.UseCases.Orders.Queries.GetOrders;

public class GetBySupplierIdQuery : IRequest<IEnumerable<OrderDTO>>
{
    public GetBySupplierIdQuery(long supplierId) { SupplierId = supplierId; }
    public long SupplierId { get; set; }
}

public class GetBySupplierIdQueryHandler(IMapper mapper,
    IRepository<Order> repository) : IRequestHandler<GetBySupplierIdQuery, IEnumerable<OrderDTO>>
{
    public async Task<IEnumerable<OrderDTO>> Handle(GetBySupplierIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<OrderDTO>>(await repository.SelectAll(u => u.SupplierId.Equals(request.SupplierId),
           includes: new[] { "User", "Address", "Supplier", "Payment", "OrderItems.Product" })
           .ToListAsync());
}