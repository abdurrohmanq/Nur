using MediatR;
using AutoMapper;
using Nur.Domain.Enums;
using Nur.Domain.Entities.Users;
using Nur.Application.Commons.Constants;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Users.DTOs;

namespace Nur.Application.UseCases.Users.Commands;

public class UserUpdateCommand : IRequest<UserDTO>
{
    public long Id { get; set; }
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

public class UserUpdateCommandHandler(IMapper mapper,
    IRepository<User> repository) : IRequestHandler<UserUpdateCommand, UserDTO>
{
    public async Task<UserDTO> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(u => u.Id.Equals(request.Id))
            ?? throw new($"This user is not found with id: {request.Id}");

        entity = mapper.Map(request, entity);
        entity.DateOfBirth = request.DateOfBirth.AddHours(TimeConstant.UTC);

        repository.Update(entity);
        await repository.SaveAsync();

        return mapper.Map<UserDTO>(entity); 
    }
}
