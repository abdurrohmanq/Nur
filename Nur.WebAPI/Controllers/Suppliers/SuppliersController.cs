using MediatR;
using Nur.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Suppliers.Commands;
using Nur.Application.UseCases.Suppliers.Queries.GetSuppliers;

namespace Nur.WebAPI.Controllers.Suppliers;

public class SuppliersController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(SupplierCreateCommand command, CancellationToken cancellationToken)
       => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync(SupplierUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new SupplierDeleteCommand(id), cancellationToken) });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetSupplierQuery(id), cancellationToken) });
    
    [HttpGet("get-by-vehicle-id/{vehicleId:long}")]
    public async Task<IActionResult> GetByVehicleIdAsync(long vehicleId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetByVehicleIdQuery(vehicleId), cancellationToken) });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetAllSuppliersQuery(), cancellationToken) });
}
