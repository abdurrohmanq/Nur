using MediatR;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Suppliers;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Suppliers.Commands;

public class SupplierDeleteCommand : IRequest<bool>
{
    public SupplierDeleteCommand(long supplierId) { Id = supplierId; }
    public long Id { get; set; }
}

public class SupplierDeleteCommandHandler(IRepository<Supplier> repository) : IRequestHandler<SupplierDeleteCommand, bool>
{
    public async Task<bool> Handle(SupplierDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id == request.Id)
            ?? throw new NotFoundException($"This supplier is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}
