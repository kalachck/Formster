using Formster.Application.Dtos;
using Formster.Application.Sqrs;
using Formster.Application.Sqrs.Commands;
using Formster.Application.Sqrs.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Formster.Application;

public static class DependencyRegistrar
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<ILocalMessageBus, LocalMessageBus>();
        
        services.AddScoped<ICommandHandler<AddFormSubmissionCommand>, AddFormSubmissionCommandHandler>();
        services.AddScoped<IQueryHandler<GetAllForSubmissionsQuery, IEnumerable<FormSubmissionResponseDto>>, GetAllForSubmissionsQueryHandler>();
        services.AddScoped<IQueryHandler<SearchFormSubmissionsQuery, IEnumerable<FormSubmissionResponseDto>>, SearchFormSubmissionsQueryHandler>();
    }
}
