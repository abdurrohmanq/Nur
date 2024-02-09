using MediatR;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Payments;
using Nur.Application.Commons.Interfaces;

namespace Nur.Application.UseCases.Payments.Commands;

public class PaymentDeleteCommand : IRequest<bool>
{
    public PaymentDeleteCommand(long paymentId) { Id = paymentId;}
    public long Id { get; set; }
}

public class PaymentDeleteCommandHandler(IRepository<Payment> repository) : IRequestHandler<PaymentDeleteCommand, bool>
{
    public async Task<bool> Handle(PaymentDeleteCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(p => p.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This payment is not found with id: {request.Id}");

        repository.Delete(entity);
        await repository.SaveAsync();

        return true;
    }
}
