using MediatR;
using Nur.Domain.Entities.Carts;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Carts.CartItems.Commands;

public class DeleteAllCartItemsCommand : IRequest<bool>
{
    public DeleteAllCartItemsCommand(long cartId) { CartId = cartId;}
    public long CartId { get; set; }
}

public class DeleteAllCartItemsCommandHandler(IRepository<CartItem> repository,
    IRepository<Cart> cartRepository) : IRequestHandler<DeleteAllCartItemsCommand, bool>
{
    public async Task<bool> Handle(DeleteAllCartItemsCommand request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.SelectAsync(c => c.Id.Equals(request.CartId));
        if(cart is null)
            return false;

        repository.Delete(c => c.CartId.Equals(request.CartId));
        cart.TotalPrice = 0;
        cartRepository.Update(cart);
        await repository.SaveAsync();
        return true;
    }
}
