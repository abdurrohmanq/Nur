using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Addresses;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Addresses.DTOs;

namespace Nur.Application.UseCases.Addresses.Commands;

public class AddressUpdateCommand : IRequest<AddressDTO>
{
    public long Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string DoorCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class AddressUpdateCommandHandler(IMapper mapper,
    IRepository<Address> repository) : IRequestHandler<AddressUpdateCommand, AddressDTO>
{
    public async Task<AddressDTO> Handle(AddressUpdateCommand request, CancellationToken cancellationToken)
    {
        var existAddress = await repository.SelectAsync(a => a.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This address is not found with id: {request.Id}");

        existAddress = mapper.Map(request, existAddress);
        repository.Update(existAddress);
        await repository.SaveAsync();

        return mapper.Map<AddressDTO>(existAddress);    
    }
}
