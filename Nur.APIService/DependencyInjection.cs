using Nur.APIService.Services;
using Nur.APIService.Constants;
using Nur.APIService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Nur.APIService;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpClient<IUserService, UserService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Users/");
        });

        return services;
    }
}
