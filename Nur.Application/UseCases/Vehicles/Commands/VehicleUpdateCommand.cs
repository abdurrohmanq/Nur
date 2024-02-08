using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nur.Application.Exceptions;
using Nur.Domain.Entities.Suppliers;
using Nur.Domain.Entities.Attachments;
using Nur.Application.Commons.Interfaces;
using Nur.Application.UseCases.Vehicles.DTOs;
using Nur.Application.UseCases.Attachments.Commands;

namespace Nur.Application.UseCases.Vehicles.Commands;

public class VehicleUpdateCommand : IRequest<VehicleDTO>
{
    public long Id { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public string CarNumber { get; set; }
    public string Color { get; set; }
    public IFormFile Attachment { get; set; }
}

public class VehicleUpdateCommandHandler(IMapper mapper,
    IRepository<Vehicle> repository,
    IRequestHandler<AttachmentCreateCommand, Attachment> attachmentCreateHandler,
     IRequestHandler<AttachmentRemoveCommand, bool> attachmentRemoveHandler) : IRequestHandler<VehicleUpdateCommand, VehicleDTO>
{
    public async Task<VehicleDTO> Handle(VehicleUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(v => v.Id.Equals(request.Id))
            ?? throw new NotFoundException($"This vehicle is not found with id: {request.Id}");

        entity.Model = request.Model;
        entity.Brand = request.Brand;
        entity.CarNumber = request.CarNumber;
        entity.Color = request.Color;

        if (request.Attachment is not null)
        {
            var attachment = await attachmentCreateHandler.Handle(new AttachmentCreateCommand { FormFile = request.Attachment }, cancellationToken);
            await attachmentRemoveHandler.Handle(new AttachmentRemoveCommand { Attachment = entity.Attachment }, cancellationToken);
            entity.Attachment = attachment;
            entity.AttachmentId = attachment.Id;
        }

        repository.Update(entity);
        await repository.SaveAsync();

        return mapper.Map<VehicleDTO>(entity);
    }
}
