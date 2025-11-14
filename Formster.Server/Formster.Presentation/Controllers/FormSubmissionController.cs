using Formster.Application.Cqrs;
using Formster.Application.Cqrs.Commands;
using Formster.Application.Cqrs.Queries;
using Formster.Presentation.Models;
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
        [FromQuery] GetAllFormSubmissionsModel model,
        CancellationToken ct)
    {
        var formSubmissions = await _localMessageBus.DispatchAsync(new GetAllForSubmissionsQuery(model.Take, model.Skip), ct);
        
        return Ok(formSubmissions);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] SearchFormSubmissionsModel model,
        CancellationToken ct)
    {
        var formSubmissions = await _localMessageBus.DispatchAsync(new SearchFormSubmissionsQuery(model.FormName, model.Query), ct);
        
        return Ok(formSubmissions);
    }

    [HttpPost]
    public async Task<IActionResult> Add(
        AddFormSubmissionModel model,
        CancellationToken ct)
    {
        await _localMessageBus.DispatchAsync(new AddFormSubmissionCommand(model.FormName, model.JsonData), ct);
        
        return Ok();
    }
}
