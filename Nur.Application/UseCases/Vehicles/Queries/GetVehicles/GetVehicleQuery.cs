using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Suppliers;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Vehicles.DTOs;

namespace Nur.Application.UseCases.Vehicles.Queries.GetVehicles;

public class GetVehicleQuery : IRequest<VehicleDTO>
{
    public GetVehicleQuery(long vehicleId) { Id = vehicleId; }
    public long Id { get; set; }
}

public class GetVehicleQueryHandler(IMapper mapper,
    IRepository<Vehicle> repository) : IRequestHandler<GetVehicleQuery, VehicleDTO>
{
    public async Task<VehicleDTO> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
        => mapper.Map<VehicleDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id), includes: new[] { "Attachment" })
            ?? throw new NotFoundException($"This vehicle is not found with id: {request.Id}"));
}