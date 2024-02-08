using MediatR;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Products.Commands;

public class ProductDeleteCommand : IRequest<bool>
{
    public ProductDeleteCommand(long productId) { Id = productId; }
    public long Id { get; set; } 
}

public class ProductDeleteCommandHandler(IRepository<Product> repository) : IRequestHandler<ProductDeleteCommand, bool>
{
    public async Task<bool> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id == request.Id)
            ?? throw new NotFoundException($"This product is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}
