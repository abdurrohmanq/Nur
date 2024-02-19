using MediatR;
using Nur.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Carts.CartItems.Commands;
using Nur.Application.UseCases.Carts.CartItems.Queries.GetCartItems;

namespace Nur.WebAPI.Controllers.Carts;

public class CartItemsController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromForm] CartItemCreateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync([FromForm] CartItemUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new CartItemDeleteCommand(id), cancellationToken) });
    
    [HttpDelete("delete-by-product-name")]
    public async Task<IActionResult> DeleteByProductNameAsync(string productName, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new CartItemDeleteByProductNameCommand(productName), cancellationToken) });
    
    [HttpDelete("delete-all/{cartId:long}")]
    public async Task<IActionResult> DeleteAllAsync(long cartId,CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new DeleteAllCartItemsCommand(cartId), cancellationToken) });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetCartItemQuery(id), cancellationToken) });

    [HttpGet("get-by-cart-id/{cartId:long}")]
    public async Task<IActionResult> GetByCartIdAsync(long cartId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetByCartIdQuery(cartId), cancellationToken) });
    
    [HttpGet("get-by-product-id/{productId:long}")]
    public async Task<IActionResult> GetByProductIdAsync(long productId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetByProductIdQuery(productId), cancellationToken) });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetAllCartItemsQuery(), cancellationToken) });
}
