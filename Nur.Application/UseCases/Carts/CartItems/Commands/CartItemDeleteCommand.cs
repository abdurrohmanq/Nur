using MediatR;
using Nur.Application.Exceptions;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Carts.CartItems.Commands;

public class CartItemDeleteCommand : IRequest<bool>
{
    public CartItemDeleteCommand(long id) { Id = id; }
    public long Id { get; set; }
}

public class CartItemDeleteCommandHandler(IRepository<CartItem> repository) : IRequestHandler<CartItemDeleteCommand, bool>
{
    public async Task<bool> Handle(CartItemDeleteCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await repository.SelectAsync(c => c.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This cartItem was not found with id: {request.Id}");

        repository.Delete(cartItem);
        await repository.SaveAsync();
        return true;
    }
}
