using Formster.Domain.Interfaces;
using Formster.Infrastructure.Repositories;
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
    }
}
