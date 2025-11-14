namespace Formster.Presentation.Models;

public record GetAllFormSubmissionsModel(int Take, int Skip);

public record SearchFormSubmissionsModel(string? FormName, string? Query);

public record AddFormSubmissionModel(string FormName, Dictionary<string, object> JsonData);
