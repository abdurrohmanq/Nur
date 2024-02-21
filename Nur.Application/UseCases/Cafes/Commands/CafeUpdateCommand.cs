using AutoMapper;
using MediatR;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Cafes.DTOs;
using Nur.Domain.Entities.Cafes;

namespace Nur.Application.UseCases.Cafes.Commands;

public class CafeUpdateCommand : IRequest<CafeDTO>
{
    public long Id { get; set; }
    public string InstagramLink { get; set; }
    public string FacebookLink { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
}

public class CafeUpdateCommandHandler(IMapper mapper,
    IRepository<Cafe> repository) : IRequestHandler<CafeUpdateCommand, CafeDTO>
{
    public async Task<CafeDTO> Handle(CafeUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(u => u.Id.Equals(request.Id));
        entity = mapper.Map(request, entity);

        repository.Update(entity);
        await repository.SaveAsync();

        return mapper.Map<CafeDTO>(entity);
    }
}
