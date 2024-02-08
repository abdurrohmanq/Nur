using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Suppliers.DTOs;

namespace Nur.Application.UseCases.Suppliers.Queries.GetSuppliers;

public class GetSupplierQuery : IRequest<SupplierDTO>
{
    public GetSupplierQuery(long supplierId) { Id = supplierId; }
    public long Id { get; set; }
}

public class GetSupplierQueryHandler(IMapper mapper,
    IRepository<Product> repository) : IRequestHandler<GetSupplierQuery, SupplierDTO>
{
    public async Task<SupplierDTO> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        => mapper.Map<SupplierDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id), 
            includes: new[] { "Vehicle", "Attachment" })
            ?? throw new NotFoundException($"This supplier is not found with id: {request.Id}"));
}
