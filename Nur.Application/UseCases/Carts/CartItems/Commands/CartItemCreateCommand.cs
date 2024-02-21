using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Carts;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Carts.CartItems.DTOs;

namespace Nur.Application.UseCases.Carts.CartItems.Commands;

public class CartItemCreateCommand : IRequest<CartItemDTO>
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public long CartId { get; set; }
    public long ProductId { get; set; }
}

public class CartItemCreateCommandHandler(IMapper mapper,
    IRepository<CartItem> cartItemRepository,
    IRepository<Cart> cartRepository,
    IRepository<Product> productRepository) : IRequestHandler<CartItemCreateCommand, CartItemDTO>
{
    public async Task<CartItemDTO> Handle(CartItemCreateCommand request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.SelectAsync(c => c.Id.Equals(request.CartId))
            ?? throw new NotFoundException($"This cart was not found with id: {request.CartId}");
        
        var product = await productRepository.SelectAsync(c => c.Id.Equals(request.ProductId))
            ?? throw new NotFoundException($"This product was not found with id: {request.ProductId}");

        if (product.Quantity != null)
        {
            if (request.Quantity > product.Quantity)
                return null;
        }

        var cartItem = mapper.Map<CartItem>(request);
        cartItem.Sum = (decimal)cartItem.Quantity * cartItem.Price;
        cartItem.Cart = cart;
        cartItem.Product = product;

        await cartItemRepository.InsertAsync(cartItem);
        await cartItemRepository.SaveAsync();

        cart.TotalPrice += cartItem.Sum;
        cartRepository.Update(cart);
        await cartRepository.SaveAsync();

        return mapper.Map<CartItemDTO>(cartItem);
    }
}
