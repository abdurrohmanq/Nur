using MediatR;
using Nur.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Carts.Queries.GetCarts;

namespace Nur.WebAPI.Controllers.Carts;

public class CartsController(IMediator mediator) : BaseController
{
    [HttpGet("get-by-user-id/{userId:long}")]
    public async Task<IActionResult> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetCartByUserIdQuery(userId), cancellationToken) });
}
