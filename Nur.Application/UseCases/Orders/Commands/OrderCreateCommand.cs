using MediatR;
using Nur.Domain.Enums;
using AutoMapper;
using Nur.Domain.Entities.Users;
using Nur.Domain.Entities.Orders;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Payments;
using Nur.Domain.Entities.Suppliers;
using Nur.Domain.Entities.Addresses;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Orders.DTOs;
using Nur.Application.Commons.Helpers;

namespace Nur.Application.UseCases.Orders.Commands;

public class OrderCreateCommand : IRequest<OrderDTO>
{
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Status Status { get; set; }
    public decimal? DeliveryFee { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderType OrderType { get; set; }
    public string Description { get; set; }
    public long UserId { get; set; }
    public long? AddressId { get; set; }
    public long? SupplierId { get; set; }
    public long PaymentId { get; set; }
}

public class OrderCreateCommandHandler(IMapper mapper,
    IRepository<Order> orderRepository,
    IRepository<User> userRepository,
    IRepository<Supplier> supplierRepository,
    IRepository<Address> addressRepository,
    IRepository<Payment> paymentRepository) : IRequestHandler<OrderCreateCommand, OrderDTO>
{
    public async Task<OrderDTO> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.SelectAsync(u => u.Id.Equals(request.UserId))
            ?? throw new NotFoundException($"This user was not found with id: {request.UserId}");

        Supplier supplier = null;
        if (request.SupplierId != null)
        {
            supplier = await supplierRepository.SelectAsync(u => u.Id.Equals(request.SupplierId))
                ?? throw new NotFoundException($"This supplier was not found with id: {request.SupplierId}");
        }

        Address address = null;
        if (request.AddressId != null)
        {
            address = await addressRepository.SelectAsync(u => u.Id.Equals(request.AddressId))
                ?? throw new NotFoundException($"This address was not found with id: {request.AddressId}");
        }

        var payment = await paymentRepository.SelectAsync(u => u.Id.Equals(request.PaymentId))
            ?? throw new NotFoundException($"This payment was not found with id: {request.PaymentId}");

        request.StartAt = TimeHelper.GetDateTime().ToLocalTime();

        var entity = mapper.Map<Order>(request);
        entity.User = user;
        entity.Supplier = supplier;
        entity.Address = address;
        entity.Payment = payment;

        await orderRepository.InsertAsync(entity);
        await orderRepository.SaveAsync();

        return mapper.Map<OrderDTO>(entity);
    }
}
