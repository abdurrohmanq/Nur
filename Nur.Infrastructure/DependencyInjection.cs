using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nur.Application.Commons.Interfaces;
using Nur.Infrastructure.Contexts;
using Nur.Infrastructure.Repositories;

namespace Nur.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Add DbContext
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        //Repository
        services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
        return services;
    }
}
