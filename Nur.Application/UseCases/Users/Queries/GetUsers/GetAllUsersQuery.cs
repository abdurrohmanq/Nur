using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Users.DTOs;
using Nur.Domain.Entities.Users;

namespace Nur.Application.UseCases.Users.Queries.GetUsers;

public record GetAllUsersQuery : IRequest<IEnumerable<UserDTO>>
{}

public class GetAllUsersQueryHandler(IMapper mapper,
    IRepository<User> repository) : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>
{
    public async Task<IEnumerable<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await repository.SelectAll().ToListAsync();
        var mappedUsers = mapper.Map<IEnumerable<UserDTO>>(users);
        return mappedUsers;
    }
}
