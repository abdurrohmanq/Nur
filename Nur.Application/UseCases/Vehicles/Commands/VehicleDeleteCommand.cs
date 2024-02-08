using MediatR;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Suppliers;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Vehicles.Commands;

public class VehicleDeleteCommand : IRequest<bool>
{
    public VehicleDeleteCommand(long vehicleId) { Id = vehicleId; }
    public long Id { get; set; }
}

public class VehicleDeleteCommandHandler(IRepository<Vehicle> repository) : IRequestHandler<VehicleDeleteCommand, bool>
{
    public async Task<bool> Handle(VehicleDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id == request.Id)
            ?? throw new NotFoundException($"This vehicle is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}