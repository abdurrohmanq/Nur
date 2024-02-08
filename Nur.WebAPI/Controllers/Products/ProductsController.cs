using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Models;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Products.Commands;
using Nur.Application.UseCases.Products.Queries.GetProducts;

namespace Nur.WebAPI.Controllers.Products;

public class ProductsController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync(ProductCreateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync(ProductUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new ProductDeleteCommand(id), cancellationToken) });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetProductQuery(id), cancellationToken) });

    [HttpGet("get-by-category-id/{categoryId:long}")]
    public async Task<IActionResult> GetByTelegramIdAsync(long categoryId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetByCategoryIdQuery(categoryId), cancellationToken) });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetAllProductsQuery(), cancellationToken) });
}
