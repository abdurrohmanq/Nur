using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nur.Domain.Entities.Addresses;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Addresses.DTOs;

namespace Nur.Application.UseCases.Addresses.Queries;

public class GetAllAddressesQuery : IRequest<IEnumerable<AddressDTO>>
{ }

public class GetAllAddressesQueryHandler(IMapper mapper,
    IRepository<Address> repository) : IRequestHandler<GetAllAddressesQuery, IEnumerable<AddressDTO>>
{
    public async Task<IEnumerable<AddressDTO>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        var Addresses = await repository.SelectAll().ToListAsync();
        var mappedAddresses = mapper.Map<IEnumerable<AddressDTO>>(Addresses);
        return mappedAddresses;
    }
}