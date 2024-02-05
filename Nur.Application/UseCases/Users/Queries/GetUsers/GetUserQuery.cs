using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Users;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Users.DTOs;

namespace Nur.Application.UseCases.Users.Queries.GetUsers;

public record GetUserQuery : IRequest<UserDTO>
{
    public GetUserQuery(long userId) { Id = userId; }
    public long Id { get; set; }
}

public class GetUserQueryHandler(IMapper mapper,
    IRepository<User> repository) : IRequestHandler<GetUserQuery, UserDTO>
{
    public async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
        => mapper.Map<UserDTO>(await repository.SelectAsync(u => u.Id.Equals(request.Id))
            ?? throw new($"This user is not found with id: {request.Id}"));
}
