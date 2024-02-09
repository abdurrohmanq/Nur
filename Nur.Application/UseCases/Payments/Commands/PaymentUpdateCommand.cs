using MediatR;
using AutoMapper;
using Nur.Domain.Enums;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Payments;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Payments.DTOs;

namespace Nur.Application.UseCases.Payments.Commands;

public class PaymentUpdateCommand : IRequest<PaymentDTO>
{
    public long Id { get; set; }
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
}

public class PaymentUpdateCommandHandler(IMapper mapper,
    IRepository<Payment> repository) : IRequestHandler<PaymentUpdateCommand, PaymentDTO>
{
    public async Task<PaymentDTO> Handle(PaymentUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(p => p.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This payment is not found with id: {request.Id}");

        entity = mapper.Map(request, entity);

        repository.Update(entity);
        await repository.SaveAsync();

        return mapper.Map<PaymentDTO>(entity);
    }
}
