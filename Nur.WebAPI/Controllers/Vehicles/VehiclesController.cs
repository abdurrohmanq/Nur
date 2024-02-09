using MediatR;
using Nur.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Vehicles.Commands;
using Nur.Application.UseCases.Vehicles.Queries.GetVehicles;

namespace Nur.WebAPI.Controllers.Vehicles;

public class VehiclesController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromForm] VehicleCreateCommand command, CancellationToken cancellationToken)
       => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync([FromForm] VehicleUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new VehicleDeleteCommand(id), cancellationToken) });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetVehicleQuery(id), cancellationToken) });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetAllVehiclesQuery(), cancellationToken) });
}
