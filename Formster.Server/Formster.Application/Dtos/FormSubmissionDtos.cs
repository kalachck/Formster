namespace Formster.Application.Dtos;

public record FormSubmissionResponseDto(
    Guid Id,
    string FormName,
    Dictionary<string, object> JsonData,
    DateTime CreatedAt);
