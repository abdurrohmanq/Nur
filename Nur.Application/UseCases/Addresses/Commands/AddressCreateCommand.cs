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
        address.Latitude = AddDotAfterSecondDigit(address.Latitude);
        address.Longitude = AddDotAfterSecondDigit(address.Longitude);
        await repository.InsertAsync(address);
        await repository.SaveAsync();

        return mapper.Map<AddressDTO>(address);
    }

    private static double AddDotAfterSecondDigit(double num)
    {
        string numStr = num.ToString();
        int dotIndex = 2;
        if (!numStr.Contains("."))
        {
            string formattedNumStr = numStr.Substring(0, dotIndex) + "." + numStr.Substring(dotIndex);
            return double.Parse(formattedNumStr);
        }
        return num;
    }
}
