using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.Commands;

public class OrderItemUpdateCommand : IRequest<OrderItemDTO>
{
    public long Id { get; set; }
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
}

public class OrderItemUpdateCommandHandler(IMapper mapper,
    IRepository<Order> orderRepository,
    IRepository<Product> productRepository,
    IRepository<OrderItem> orderItemRepository) : IRequestHandler<OrderItemUpdateCommand, OrderItemDTO>
{
    public async Task<OrderItemDTO> Handle(OrderItemUpdateCommand request, CancellationToken cancellationToken)
    {
        var orderItem = await orderItemRepository.SelectAsync(o => o.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This orderItem was not found with id: {request.Id}");

        if (orderItem.OrderId != request.OrderId)
        {
            orderItem.Order = await orderRepository.SelectAsync(u => u.Id.Equals(request.OrderId))
                ?? throw new NotFoundException($"This order was not found with id: {request.OrderId}");
        }

        if (orderItem.ProductId != request.ProductId)
        {
            orderItem.Product = await productRepository.SelectAsync(u => u.Id.Equals(request.ProductId))
                ?? throw new NotFoundException($"This product was not found with id: {request.ProductId}");
        }

        orderItem = mapper.Map(request, orderItem);
        orderItemRepository.Update(orderItem);
        await orderRepository.SaveAsync();

        return mapper.Map<OrderItemDTO>(orderItem);
    }
}
