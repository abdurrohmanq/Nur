using MediatR;
using Nur.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Cafes.Commands;
using Nur.Application.UseCases.Cafes.Queries;

namespace Nur.WebAPI.Controllers.Cafes;

public class CafesController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromForm] CafeCreateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync([FromForm] CafeUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new CafeDeleteCommand(id), cancellationToken) });

    [HttpGet("get")]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetCafeQuery(), cancellationToken) });
}
