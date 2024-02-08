using MediatR;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Addresses;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Addresses.Commands;

public class AddressDeleteCommand : IRequest<bool>
{
    public AddressDeleteCommand(long addressId) { Id = addressId; }
    public long Id { get; set; }
}

public class AddressDeleteCommandHandler(IRepository<Address> repository) : IRequestHandler<AddressDeleteCommand, bool>
{
    public async Task<bool> Handle(AddressDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id == request.Id)
            ?? throw new NotFoundException($"This address is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}
