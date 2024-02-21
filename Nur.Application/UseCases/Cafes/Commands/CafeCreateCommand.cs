using MediatR;
using AutoMapper;
using Nur.Domain.Entities.Cafes;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Cafes.DTOs;

namespace Nur.Application.UseCases.Cafes.Commands;

public class CafeCreateCommand : IRequest<CafeDTO>
{
    public string InstagramLink { get; set; }
    public string FacebookLink { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
}

public class CafeCreateCommandHandler(IMapper mapper,
    IRepository<Cafe> repository) : IRequestHandler<CafeCreateCommand, CafeDTO>
{
    public async Task<CafeDTO> Handle(CafeCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Cafe>(request);
        await repository.InsertAsync(entity);
        await repository.SaveAsync();

        return mapper.Map<CafeDTO>(entity);
    }
}
