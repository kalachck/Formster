using Formster.Application.Dtos;
using Formster.Application.Sqrs;
using Formster.Application.Sqrs.Commands;
using Formster.Application.Sqrs.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Formster.Presentation.Controllers;

[ApiController]
[Route("formster/api/form-submission")]
public class FormSubmissionController : ControllerBase
{
    private readonly ILocalMessageBus _localMessageBus;

    public FormSubmissionController(ILocalMessageBus localMessageBus)
    {
        _localMessageBus = localMessageBus;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll(
        int take,
        int skip,
        CancellationToken ct)
    {
        var formSubmissions = await _localMessageBus.DispatchAsync(new GetAllForSubmissionsQuery(take, skip), ct);
        
        return Ok(formSubmissions);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        string? formName,
        string? query,
        CancellationToken ct)
    {
        var formSubmissions = await _localMessageBus.DispatchAsync(new SearchFormSubmissionsQuery(formName, query), ct);
        
        return Ok(formSubmissions);
    }

    [HttpPost]
    public async Task<IActionResult> Add(
        FormSubmissionRequestDto request,
        CancellationToken ct)
    {
        await _localMessageBus.DispatchAsync(new AddFormSubmissionCommand(request), ct);
        
        return Ok();
    }
}
