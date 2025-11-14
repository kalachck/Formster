using System.Text.Json;
using Formster.Application.Dtos;
using Formster.Domain.Entities;
using Formster.Domain.Interfaces;

namespace Formster.Application.Cqrs.Commands;

public record AddFormSubmissionCommand(string FormName, Dictionary<string, object> JsonData) : ICommand;


public class AddFormSubmissionCommandHandler : CommandHandler<AddFormSubmissionCommand>
{
    private readonly IFormSubmissionRepository _repository;

    public AddFormSubmissionCommandHandler(IFormSubmissionRepository repository)
    {
        _repository = repository;
    }
    
    public override async Task HandleAsync(AddFormSubmissionCommand command, CancellationToken ct)
    {
        await _repository.AddAsync(new FormSubmission
        {
            FormName = command.FormName,
            JsonData = JsonSerializer.Serialize(command.JsonData)
        }, ct);
    }
}
