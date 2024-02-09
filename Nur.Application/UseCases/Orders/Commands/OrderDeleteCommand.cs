using MediatR;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Orders;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Orders.Commands;

public class OrderDeleteCommand : IRequest<bool>
{
    public OrderDeleteCommand(long orderId) { Id = orderId; }
    public long Id { get; set; }
}

public class OrderDeleteCommandHandler(IRepository<Order> repository) : IRequestHandler<OrderDeleteCommand, bool>
{
    public async Task<bool> Handle(OrderDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(entity => entity.Id == request.Id)
            ?? throw new NotFoundException($"This order is not found with id: {request.Id}");

        repository.Delete(entity);
        return await repository.SaveAsync() > 0;
    }
}