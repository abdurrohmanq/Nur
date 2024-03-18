using Microsoft.EntityFrameworkCore;
using Nur.Infrastructure.Contexts;

namespace Nur.WebAPI.Extensions;

public static class MigrateHelper
{
    public static void MigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}
