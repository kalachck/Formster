using System.Text.Json;
using Formster.Application.Dtos;
using Formster.Domain.Entities;
using Formster.Domain.Interfaces;

namespace Formster.Application.Sqrs.Commands;

public record AddFormSubmissionCommand(FormSubmissionRequestDto RequestDto) : ICommand;


public class AddFormSubmissionCommandHandler : CommandHandler<AddFormSubmissionCommand>
{
    private readonly IFormSubmissionRepository _repository;

    public AddFormSubmissionCommandHandler(IFormSubmissionRepository repository)
    {
        _repository = repository;
    }
    
    public override async Task HandleAsync(AddFormSubmissionCommand command, CancellationToken ct)
    {
        var request = command.RequestDto;

        await _repository.AddAsync(new FormSubmission
        {
            FormName = request.FormName,
            JsonData = JsonSerializer.Serialize(request.JsonData)
        }, ct);
    }
}
