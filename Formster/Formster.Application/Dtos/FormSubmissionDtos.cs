namespace Formster.Application.Dtos;

public record FormSubmissionRequestDto(
    string FormName,
    Dictionary<string, object> JsonData);

public record FormSubmissionResponseDto(
    Guid Id,
    string FormName,
    Dictionary<string, object> JsonData,
    DateTime CreatedAt);


