using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nur.Application.Behaviors;
using Nur.Application.Commons.Mappers;
using Nur.Application.UseCases.Users.Commands;
using Nur.Application.UseCases.Users.DTOs;
using Nur.Application.UseCases.Users.Queries.GetUsers;
using System.Reflection;

namespace Nur.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddLogging(configure =>
        {
            configure.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        //Users
        services.AddScoped<IRequestHandler<UserCreateCommand, int>, UserCreateCommandHandler>();
        services.AddScoped<IRequestHandler<UserUpdateCommand, int>, UserUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<UserDeleteCommand, bool>, UserDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>, GetAllUsersQueryHandler>();
        services.AddScoped<IRequestHandler<GetUserQuery, UserDTO>, GetUserQueryHandler>();
        services.AddScoped<IRequestHandler<GetUserByTelegramIdQuery, UserDTO>, GetUserByTelegramIdHandler>();
        return services;
    }
}
