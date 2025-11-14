using Formster.Application.Dtos;
using Formster.Application.Utils;
using Formster.Domain.Interfaces;

namespace Formster.Application.Cqrs.Queries;

public record SearchFormSubmissionsQuery(
    string? FormName,
    string? Query) : IQuery<IEnumerable<FormSubmissionResponseDto>>;


public class SearchFormSubmissionsQueryHandler : QueryHandler<SearchFormSubmissionsQuery, IEnumerable<FormSubmissionResponseDto>>
{
    private readonly IFormSubmissionRepository _repository;

    public SearchFormSubmissionsQueryHandler(IFormSubmissionRepository repository)
    {
        _repository = repository;
    }
    
    public override async Task<IEnumerable<FormSubmissionResponseDto>> HandleAsync(SearchFormSubmissionsQuery query, CancellationToken ct)
    {
        var formSubmission = await _repository.SearchAsync(query.FormName, query.Query, ct);

        return formSubmission.Select(x => new FormSubmissionResponseDto(
            x.Id,
            x.FormName,
            JsonData: FormSubmissionJsonDataSerializer.DeserializeJsonData(x.JsonData),
            x.CreatedAt));
    }
}
