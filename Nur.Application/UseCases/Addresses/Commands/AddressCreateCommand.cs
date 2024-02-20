using AutoMapper;
using MediatR;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Addresses.DTOs;
using Nur.Domain.Entities.Addresses;

namespace Nur.Application.UseCases.Addresses.Commands;

public class AddressCreateCommand : IRequest<AddressDTO>
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string DoorCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class AddressCreateCommandHandler(IMapper mapper,
             IRepository<Address> repository) : IRequestHandler<AddressCreateCommand, AddressDTO>
{
    public async Task<AddressDTO> Handle(AddressCreateCommand request, CancellationToken cancellationToken)
    {
        var address = mapper.Map<Address>(request);
        await repository.InsertAsync(address);
        await repository.SaveAsync();

        return mapper.Map<AddressDTO>(address);
    }
}
