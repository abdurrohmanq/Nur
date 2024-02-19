using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Carts;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Carts.CartItems.DTOs;

namespace Nur.Application.UseCases.Carts.CartItems.Commands;

public class CartItemUpdateCommand : IRequest<CartItemDTO>
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public long CartId { get; set; }
    public long ProductId { get; set; }
}

public class CartItemUpdateCommandHandler(IMapper mapper,
    IRepository<CartItem> cartItemRepository,
    IRepository<Cart> cartRepository,
    IRepository<Product> productRepository) : IRequestHandler<CartItemUpdateCommand, CartItemDTO>
{
    public async Task<CartItemDTO> Handle(CartItemUpdateCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await cartItemRepository.SelectAsync(c => c.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This cartItem was not found with id: {request.Id}");

        if (request.CartId != cartItem.CartId)
        {
            cartItem.Cart = await cartRepository.SelectAsync(c => c.Id.Equals(request.CartId))
                ?? throw new NotFoundException($"This cart was not found with id: {request.CartId}");
        }
        if (request.ProductId != cartItem.ProductId)
        {
            cartItem.Product = await productRepository.SelectAsync(c => c.Id.Equals(request.ProductId))
                ?? throw new NotFoundException($"This product was not found with id: {request.ProductId}");
        }

        cartItem = mapper.Map(request, cartItem);
        cartItem.Sum = (decimal)cartItem.Quantity * cartItem.Price;
        cartItem.Cart.TotalPrice += cartItem.Sum;

        cartRepository.Update(cartItem.Cart);
        cartItemRepository.Update(cartItem);
        await cartItemRepository.SaveAsync();

        return mapper.Map<CartItemDTO>(cartItem);
    }
}
