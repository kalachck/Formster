using Formster.Application.Dtos;
using Formster.Application.Cqrs;
using Formster.Application.Cqrs.Commands;
using Formster.Application.Cqrs.Queries;
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
