using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Addresses;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Addresses.DTOs;

namespace Nur.Application.UseCases.Addresses.Queries;

public class GetAddressQuery : IRequest<AddressDTO>
{
    public GetAddressQuery(long addressId) { Id = addressId; }
    public long Id { get; set; }
}

public class GetAddressQueryHandler(IMapper mapper,
    IRepository<Address> repository) : IRequestHandler<GetAddressQuery, AddressDTO>
{
    public async Task<AddressDTO> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        => mapper.Map<AddressDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This address is not found with id: {request.Id}"));
}
