using System.Text.Json;
using Formster.Domain.Entities;
using Formster.Domain.Interfaces;

namespace Formster.Infrastructure.Utils;

public interface IDataSeeder
{
    Task SeedAsync();
}

public class DataSeeder : IDataSeeder
{
    private readonly IFormSubmissionRepository _repository;

    public DataSeeder(IFormSubmissionRepository repository)
    {
        _repository = repository;
    }
    
    public async Task SeedAsync()
    {
        var formSubmissions = new List<FormSubmission>
        {
            new()
            {
                FormName = $"Form-{Guid.NewGuid()}",
                JsonData = JsonSerializer.Serialize(new
                {
                    Prop1 = $"Prop-{Guid.NewGuid()}",
                    Prop2 = $"Prop-{Guid.NewGuid()}",
                })
            },
            new()
            {
                FormName = $"Form-{Guid.NewGuid()}",
                JsonData = JsonSerializer.Serialize(new
                {
                    Prop1 = $"Prop-{Guid.NewGuid()}",
                    Prop2 = $"Prop-{Guid.NewGuid()}",
                    Prop3 = $"Prop-{Guid.NewGuid()}",
                })
            }
        };

        foreach (var form in formSubmissions)
        {
            await _repository.AddAsync(form, CancellationToken.None);
        }
    }
}
