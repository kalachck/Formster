using Formster.Application.Dtos;
using Formster.Application.Utils;
using Formster.Domain.Interfaces;

namespace Formster.Application.Cqrs.Queries;

public record GetAllForSubmissionsQuery(int Take, int Skip) : IQuery<IEnumerable<FormSubmissionResponseDto>>;

public class GetAllForSubmissionsQueryHandler : QueryHandler<GetAllForSubmissionsQuery, IEnumerable<FormSubmissionResponseDto>>
{
    private readonly IFormSubmissionRepository _repository;

    public GetAllForSubmissionsQueryHandler(IFormSubmissionRepository repository)
    {
        _repository = repository;
    }
    
    public override async Task<IEnumerable<FormSubmissionResponseDto>> HandleAsync(GetAllForSubmissionsQuery query, CancellationToken ct)
    {
        var formSubmission = await _repository.GetAllAsync(query.Take, query.Skip, ct);

        return formSubmission.Select(x => new FormSubmissionResponseDto(
            x.Id,
            x.FormName,
            JsonData: FormSubmissionJsonDataSerializer.DeserializeJsonData(x.JsonData),
            x.CreatedAt));
    }
}
