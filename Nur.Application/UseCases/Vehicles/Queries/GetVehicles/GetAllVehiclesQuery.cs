using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nur.Domain.Entities.Suppliers;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Vehicles.DTOs;

namespace Nur.Application.UseCases.Vehicles.Queries.GetVehicles;

public class GetAllVehiclesQuery : IRequest<IEnumerable<VehicleDTO>>
{ }

public class GetAllVehicleQueryHandler(IMapper mapper,
    IRepository<Vehicle> repository) : IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleDTO>>
{
    public async Task<IEnumerable<VehicleDTO>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await repository.SelectAll(includes: new[] { "Attachment" }).ToListAsync();
        var mappedProducts = mapper.Map<IEnumerable<VehicleDTO>>(vehicles);
        return mappedProducts;
    }
}
