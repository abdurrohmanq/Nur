using MediatR;
using Nur.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Orders.Commands;
using Nur.Application.UseCases.Orders.Queries.GetOrders;

namespace Nur.WebAPI.Controllers.Orders;

public class OrdersController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromForm] OrderCreateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync([FromForm] OrderUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new OrderDeleteCommand(id), cancellationToken) });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetOrderQuery(id), cancellationToken) });

    [HttpGet("get-by-user-id/{userId:long}")]
    public async Task<IActionResult> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetByUserIdQuery(userId), cancellationToken) });
    
    [HttpGet("get-by-supplier-id/{supplierId:long}")]
    public async Task<IActionResult> GetBySupplierIdAsync(long supplierId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetBySupplierIdQuery(supplierId), cancellationToken) });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetAllOrdersQuery(), cancellationToken) });
}
