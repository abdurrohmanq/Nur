using MediatR;
using Nur.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Orders.OrderItems.Commands;
using Nur.Application.UseCases.Orders.OrderItems.Queries.GetOrderItems;

namespace Nur.WebAPI.Controllers.Orders;

public class OrderItemsController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromForm] OrderItemCreateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync([FromForm] OrderItemUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new OrderItemDeleteCommand(id), cancellationToken) });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetOrderItemQuery(id), cancellationToken) });

    [HttpGet("get-by-order-id/{orderId:long}")]
    public async Task<IActionResult> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetByOrderIdQuery(userId), cancellationToken) });

    [HttpGet("get-by-product-id/{productId:long}")]
    public async Task<IActionResult> GetBySupplierIdAsync(long supplierId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetOrderItemByProductIdQuery(supplierId), cancellationToken) });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetAllOrderItemsQuery(), cancellationToken) });
}
