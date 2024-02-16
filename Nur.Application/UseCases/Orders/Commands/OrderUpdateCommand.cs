using MediatR;
using AutoMapper;
using Nur.Domain.Enums;
using Nur.Domain.Entities.Users;
using Nur.Domain.Entities.Orders;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Payments;
using Nur.Domain.Entities.Products;
using Nur.Domain.Entities.Suppliers;
using Nur.Domain.Entities.Addresses;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.DTOs;

namespace Nur.Application.UseCases.Orders.Commands;

public class OrderUpdateCommand : IRequest<OrderDTO>
{
    public long Id { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Status Status { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderType OrderType { get; set; }
    public long UserId { get; set; }
    public long AddressId { get; set; }
    public long SupplierId { get; set; }
    public long PaymentId { get; set; }
}

public class OrderUpdateCommandHandler(IMapper mapper,
    IRepository<Order> orderRepository,
    IRepository<User> userRepository,
    IRepository<Supplier> supplierRepository,
    IRepository<Address> addressRepository,
    IRepository<Payment> paymentRepository,
    IRepository<Product> productRepository) : IRequestHandler<OrderUpdateCommand, OrderDTO>
{
    public async Task<OrderDTO> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.SelectAsync(o => o.Id.Equals(request.Id), includes: new[] {"OrderItems.Product"})
            ?? throw new NotFoundException($"This order was not found with id: {request.Id}");

        if (order.UserId != request.UserId)
        {
            order.User = await userRepository.SelectAsync(u => u.Id == request.UserId) 
                ?? throw new NotFoundException($"This user was not found with id: {request.UserId}");
        }
        if (order.SupplierId != request.SupplierId)
        {
            order.Supplier = await supplierRepository.SelectAsync(u => u.Id.Equals(request.SupplierId))
                ?? throw new NotFoundException($"This supplier was not found with id: {request.SupplierId}");
        }
        if (order.AddressId != request.AddressId)
        {
            order.Address = await addressRepository.SelectAsync(u => u.Id.Equals(request.AddressId))
                ?? throw new NotFoundException($"This address was not found with id: {request.AddressId}");
        }
        if (order.PaymentId != request.PaymentId)
        {
            order.Payment = await paymentRepository.SelectAsync(u => u.Id.Equals(request.PaymentId))
                ?? throw new NotFoundException($"This payment was not found with id: {request.PaymentId}");
        }
        if(request.Status == Status.Delivered)
        {
            foreach(var item in order.OrderItems)
            {
                item.Product.Quantity -= item.Quantity;
                productRepository.Update(item.Product);
            }
        }
        order = mapper.Map(request, order);
        orderRepository.Update(order);
        await orderRepository.SaveAsync();

        return mapper.Map<OrderDTO>(order);
    }
}
