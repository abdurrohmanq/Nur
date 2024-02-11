using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nur.Domain.Entities.Suppliers;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Suppliers.DTOs;

namespace Nur.Application.UseCases.Suppliers.Queries.GetSuppliers;

public class GetByVehicleIdQuery : IRequest<IEnumerable<SupplierDTO>>
{
    public GetByVehicleIdQuery(long vehicleId) { VehicleId = vehicleId; }
    public long VehicleId { get; set; }
}

public class GetByVehicleIdQueryHandler(IMapper mapper,
    IRepository<Supplier> repository) : IRequestHandler<GetByVehicleIdQuery, IEnumerable<SupplierDTO>>
{
    public async Task<IEnumerable<SupplierDTO>> Handle(GetByVehicleIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<IEnumerable<SupplierDTO>>(await repository.SelectAll(u => u.VehicleId.Equals(request.VehicleId),
            includes: new[] { "Vehicle", "Attachment" })
            .ToListAsync());
}
