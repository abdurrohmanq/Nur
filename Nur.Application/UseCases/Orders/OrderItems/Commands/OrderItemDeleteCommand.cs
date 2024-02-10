using MediatR;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Orders.OrderItems.Commands;

public class OrderItemDeleteCommand : IRequest<bool>
{
    public OrderItemDeleteCommand(long orderItemId) { Id = orderItemId; }
    public long Id { get; set; }
}

public class OrderItemDeleteCommandHandler(IRepository<Order> repository) : IRequestHandler<OrderItemDeleteCommand, bool>
{
    public async Task<bool> Handle(OrderItemDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id == request.Id)
            ?? throw new NotFoundException($"This order item is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}