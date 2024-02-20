using MediatR;
using AutoMapper;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Domain.Entities.Products;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.OrderItems.Commands;

public class OrderItemCreateCommand : IRequest<OrderItemDTO>
{
    public double Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Sum { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
}

public class OrderItemCreateCommandHandler(IMapper mapper,
    IRepository<Order> orderRepository,
    IRepository<Product> productRepository,
    IRepository<OrderItem> orderItemRepository) : IRequestHandler<OrderItemCreateCommand, OrderItemDTO>
{
    public async Task<OrderItemDTO> Handle(OrderItemCreateCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.SelectAsync(o => o.Id.Equals(request.OrderId))
            ?? throw new NotFoundException($"This order was not found with id: {request.OrderId}");

        var product = await productRepository.SelectAsync(o => o.Id.Equals(request.ProductId))
            ?? throw new NotFoundException($"This product was not found with id: {request.ProductId}");

        var orderItem = mapper.Map<OrderItem>(request);
        orderItem.Product = product;
        orderItem.Order = order;

        await orderItemRepository.InsertAsync(orderItem);
        await orderItemRepository.SaveAsync();

        return mapper.Map<OrderItemDTO>(orderItem);
    }
}
