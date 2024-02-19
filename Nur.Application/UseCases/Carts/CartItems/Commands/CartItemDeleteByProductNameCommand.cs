using MediatR;
using Nur.Domain.Entities.Carts;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Carts.CartItems.Commands;

public class CartItemDeleteByProductNameCommand : IRequest<bool>
{
    public CartItemDeleteByProductNameCommand(string productName) { ProductName = productName; }
    public string ProductName { get; set; }
}

public class CartItemDeleteByProductNameCommandHandler(IRepository<Cart> cartRepository, 
    IRepository<CartItem> repository) : IRequestHandler<CartItemDeleteByProductNameCommand, bool>
{
    public async Task<bool> Handle(CartItemDeleteByProductNameCommand request, CancellationToken cancellationToken)
    {
        request.ProductName = request.ProductName.Trim('❌', ' ');

        var result = await repository.SelectAsync(c => c.Product.Name.Equals(request.ProductName));
        if(result is null)
            return false;

        var cart = await cartRepository.SelectAsync(c => c.Id.Equals(result.CartId));
        cart.TotalPrice -= result.Sum;
        repository.Delete(result);
        cartRepository.Update(cart);

        await repository.SaveAsync();
        return true;
    }
}
