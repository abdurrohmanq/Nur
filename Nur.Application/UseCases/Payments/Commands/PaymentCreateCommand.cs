using MediatR;
using AutoMapper;
using Nur.Domain.Enums;
using Nur.Domain.Entities.Payments;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Payments.DTOs;

namespace Nur.Application.UseCases.Payments.Commands;

public class PaymentCreateCommand : IRequest<PaymentDTO>
{
    public PaymentType Type { get; set; }
    public decimal Amount { get; set; }
}

public class PaymentCreateCommandHandler(IMapper mapper,
    IRepository<Payment> repository) : IRequestHandler<PaymentCreateCommand, PaymentDTO>
{
    public async Task<PaymentDTO> Handle(PaymentCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Payment>(request);

        await repository.InsertAsync(entity);
        await repository.SaveAsync();

        return mapper.Map<PaymentDTO>(entity);
    }
}