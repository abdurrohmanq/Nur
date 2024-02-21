using MediatR;
using Nur.Application.Exceptions;
using Nur.Application.Commons.Interfaces;
using Nur.Domain.Entities.Carts;

namespace Nur.Application.UseCases.Carts.CartItems.Commands;

public class CartItemDeleteCommand : IRequest<bool>
{
    public CartItemDeleteCommand(long id) { Id = id; }
    public long Id { get; set; }
}

public class CartItemDeleteCommandHandler(IRepository<CartItem> repository,
                                          IRepository<Cart> cartRepository) : IRequestHandler<CartItemDeleteCommand, bool>
{
    public async Task<bool> Handle(CartItemDeleteCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await repository.SelectAsync(c => c.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This cartItem was not found with id: {request.Id}");

        var cart = await cartRepository.SelectAsync(c => c.Id.Equals(cartItem.CartId));
        repository.Delete(cartItem);
        await repository.SaveAsync();

        cart.TotalPrice -= cartItem.Sum;
        cartRepository.Update(cart);
        await cartRepository.SaveAsync();
        return true;
    }
}
