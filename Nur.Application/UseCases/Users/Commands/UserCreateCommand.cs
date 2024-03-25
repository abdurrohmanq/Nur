using MediatR;
using AutoMapper;
using Nur.Domain.Enums;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Carts;
using Nur.Domain.Entities.Users;
using Nur.Application.Commons.Helpers;
using Nur.Application.Commons.Constants;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Users.DTOs;

namespace Nur.Application.UseCases.Users.Commands;

public class UserCreateCommand : IRequest<UserDTO>
{
    public long? TelegramId { get; set; }
    public string Username { get; set; }
    public long ChatId { get; set; }
    public string LanguageCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class UserCreateCommandHandler(IMapper mapper,
    IRepository<User> repository,
    IRepository<Cart> cartRepository) : IRequestHandler<UserCreateCommand, UserDTO>
{
    public async Task<UserDTO> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(u => u.TelegramId.Equals(request.TelegramId));
        if (entity is not null)
            throw new AlreadyExistException($"This user already exist with telegram id: {request.TelegramId}");

        entity = mapper.Map<User>(request);
        entity.CreatedAt = TimeHelper.GetDateTime().ToLocalTime();
        entity.DateOfBirth = request.DateOfBirth.AddHours(TimeConstant.UTC);

        var cart = new Cart { UserId = entity.Id, User = entity };

        await repository.InsertAsync(entity);
        await cartRepository.InsertAsync(cart);
        await repository.SaveAsync();

        return mapper.Map<UserDTO>(entity);
    }
}
