using Formster.Domain.Entities;
using Formster.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Formster.Infrastructure.Repositories;

public class FormSubmissionRepository : IFormSubmissionRepository
{
    private readonly DatabaseContext _context;

    public FormSubmissionRepository(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(FormSubmission formSubmission, CancellationToken ct)
    {
        await _context.AddAsync(formSubmission, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<FormSubmission>> GetAllAsync(int skip, int take, CancellationToken ct) 
        => await _context.FormSubmissions.Skip(skip).Take(take).ToListAsync(ct);

    public async Task<IEnumerable<FormSubmission>> SearchAsync(string? formName, string? query, CancellationToken ct)
    {
        var submissions = _context.FormSubmissions.AsQueryable();
        
        if (!string.IsNullOrEmpty(formName))
            submissions = submissions.Where(x => EF.Functions.Like(x.FormName, $"%{formName}%"));
        
        if (!string.IsNullOrEmpty(query))
            submissions = submissions.Where(x => x.JsonData.Contains(query));

        return await submissions.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
    }
}
