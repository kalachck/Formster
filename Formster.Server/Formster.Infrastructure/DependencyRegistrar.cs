using Formster.Domain.Interfaces;
using Formster.Infrastructure.Repositories;
using Formster.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Formster.Infrastructure;

public static class DependencyRegistrar
{
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseInMemoryDatabase("formster-db");
        });

        services.AddScoped<IFormSubmissionRepository, FormSubmissionRepository>();
        
        services.AddScoped<IDataSeeder, DataSeeder>();
    }

    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();

        await dataSeeder.SeedAsync();
    }
}
