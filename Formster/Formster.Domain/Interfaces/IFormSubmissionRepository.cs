using Formster.Domain.Entities;

namespace Formster.Domain.Interfaces;

public interface IFormSubmissionRepository
{
    Task AddAsync(FormSubmission formSubmission, CancellationToken ct);
    
    Task<IEnumerable<FormSubmission>> GetAllAsync(int take, int skip, CancellationToken ct);
    
    Task<IEnumerable<FormSubmission>> SearchAsync(string? formName, string? query, CancellationToken ct);
}
