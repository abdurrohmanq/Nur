using MediatR;
using Nur.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nur.WebAPI.Controllers.Commons;
using Nur.Application.UseCases.Products.Commands;
using Nur.Application.UseCases.Products.Queries.GetProducts;

namespace Nur.WebAPI.Controllers.Products;

public class ProductsController(IMediator mediator) : BaseController
{
    [HttpPost("create")]
    public async Task<IActionResult> PostAsync([FromForm] ProductCreateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpPut("update")]
    public async Task<IActionResult> ModifyAsync([FromForm] ProductUpdateCommand command, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(command, cancellationToken) });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new ProductDeleteCommand(id), cancellationToken) });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetProductQuery(id), cancellationToken) });

    [HttpGet("get-by-category-id/{categoryId:long}")]
    public async Task<IActionResult> GetByCategoryIdAsync(long categoryId, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetByCategoryIdQuery(categoryId), cancellationToken) });
    
    [HttpGet("get-by-category-name")]
    public async Task<IActionResult> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetByCategoryNameQuery(categoryName), cancellationToken) });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(new Response { Data = await mediator.Send(new GetAllProductsQuery(), cancellationToken) });
}
