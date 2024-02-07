using MediatR;
using Nur.Application.Commons.Interfaces;
using Nur.Domain.Entities.Users;

namespace Nur.Application.UseCases.Users.Commands;

public class UserDeleteCommand : IRequest<bool>
{
    public UserDeleteCommand(long userId) { Id = userId; }
    public long Id { get; set; }
}

public class UserDeleteCommandHandler(IRepository<User> repository) : IRequestHandler<UserDeleteCommand, bool>
{
    public async Task<bool> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id == request.Id)
            ?? throw new($"This user is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}
