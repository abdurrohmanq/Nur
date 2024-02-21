using MediatR;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Cafes;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Cafes.Commands;

public class CafeDeleteCommand : IRequest<bool>
{
    public CafeDeleteCommand(long CafeId) { Id = CafeId; }
    public long Id { get; set; }
}

public class CafeDeleteCommandHandler(IRepository<Cafe> repository) : IRequestHandler<CafeDeleteCommand, bool>
{
    public async Task<bool> Handle(CafeDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This Cafe is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}