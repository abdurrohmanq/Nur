using AutoMapper;
using Nur.Application.UseCases.Users.Commands;
using Nur.Application.UseCases.Users.DTOs;
using Nur.Domain.Entities.Users;

namespace Nur.Application.Commons.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Users
        CreateMap<UserDTO, User>().ReverseMap();
        CreateMap<UserUpdateCommand, User>().ReverseMap();
        CreateMap<UserCreateCommand, User>().ReverseMap();
    }
}
