using MediatR;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.Exceptions;

namespace Nur.Application.UseCases.ProductCategories.Commands;

public class CategoryDeleteCommand : IRequest<bool>
{
    public CategoryDeleteCommand(long categoryId) { Id = categoryId; }
    public long Id { get; set; }
}

public class CategoryDeleteCommandHandler(IRepository<ProductCategory> repository) : IRequestHandler<CategoryDeleteCommand, bool>
{
    public async Task<bool> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This category is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}
