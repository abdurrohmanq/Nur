using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Suppliers;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Suppliers.DTOs;

namespace Nur.Application.UseCases.Suppliers.Queries.GetSuppliers;

public class GetByVehicleIdQuery : IRequest<SupplierDTO>
{
    public GetByVehicleIdQuery(long vehicleId) { VehicleId = vehicleId; }
    public long VehicleId { get; set; }
}

public class GetByVehicleIdQueryHandler(IMapper mapper,
    IRepository<Supplier> repository) : IRequestHandler<GetByVehicleIdQuery, SupplierDTO>
{
    public async Task<SupplierDTO> Handle(GetByVehicleIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<SupplierDTO>(await repository.SelectAsync(u => u.VehicleId.Equals(request.VehicleId),
            includes: new[] { "Vehicle", "Attachment" })
            ?? throw new NotFoundException($"This supplier is not found with id: {request.VehicleId}"));
}
