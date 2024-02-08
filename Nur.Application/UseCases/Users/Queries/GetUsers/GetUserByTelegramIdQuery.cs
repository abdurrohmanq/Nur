using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Users;
using Nur.Application.Exceptions;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Users.DTOs;

namespace Nur.Application.UseCases.Users.Queries.GetUsers;

public record GetUserByTelegramIdQuery : IRequest<UserDTO>
{
    public GetUserByTelegramIdQuery(long Id) { TelegramId = Id; }
    public long TelegramId { get; set; }
}

public class GetUserByTelegramIdHandler(IMapper mapper,
    IRepository<User> repository) : IRequestHandler<GetUserByTelegramIdQuery, UserDTO>
{
    public async Task<UserDTO> Handle(GetUserByTelegramIdQuery request, CancellationToken cancellationToken)
        => mapper.Map<UserDTO>(await repository.SelectAsync(u => u.TelegramId.Equals(request.TelegramId))
            ?? throw new NotFoundException($"This user is not found with id: {request.TelegramId}"));
}