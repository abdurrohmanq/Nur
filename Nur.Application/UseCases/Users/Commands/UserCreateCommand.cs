using AutoMapper;
using MediatR;
using Nur.Application.Commons.Constants;
using Nur.Application.Commons.Helpers;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Users.DTOs;
using Nur.Domain.Entities.Users;
using Nur.Domain.Enums;

namespace Nur.Application.UseCases.Users.Commands;

public class UserCreateCommand : IRequest<UserDTO>
{
    public long? TelegramId { get; set; }
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class UserCreateCommandHandler(IMapper mapper,
    IRepository<User> repository) : IRequestHandler<UserCreateCommand, UserDTO>
{
    public async Task<UserDTO> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(u => u.TelegramId.Equals(request.TelegramId));
        if (entity is not null)
            throw new($"This user already exist with telegram id: {request.TelegramId}");

        entity = mapper.Map<User>(request);
        entity.CreatedAt = TimeHelper.GetDateTime();
        entity.DateOfBirth = request.DateOfBirth.AddHours(TimeConstant.UTC);

        await repository.InsertAsync(entity);
        await repository.SaveAsync();

        return mapper.Map<UserDTO>(entity);
    }
}
