using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nur.Domain.Entities.Suppliers;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Suppliers.DTOs;

namespace Nur.Application.UseCases.Suppliers.Queries.GetSuppliers;

public class GetAllSuppliersQuery : IRequest<IEnumerable<SupplierDTO>>
{ }

public class GetAllSupplierQueryHandler(IMapper mapper,
    IRepository<Supplier> repository) : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDTO>>
{
    public async Task<IEnumerable<SupplierDTO>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.SelectAll(includes: new[] { "Vehicle", "Attachment" }).ToListAsync();
        var mappedSuppliers = mapper.Map<IEnumerable<SupplierDTO>>(products);
        return mappedSuppliers;
    }
}
