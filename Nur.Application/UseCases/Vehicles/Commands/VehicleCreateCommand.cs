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

public class VehicleCreateCommand : IRequest<VehicleDTO>
{
    public string Model { get; set; }
    public string Brand { get; set; }
    public string CarNumber { get; set; }
    public string Color { get; set; }
    public IFormFile Attachment { get; set; }
}

public class VehicleCreateCommandHandler(IMapper mapper,
    IRepository<Vehicle> repository,
    IRequestHandler<AttachmentCreateCommand, Attachment> attachmentHandler) : IRequestHandler<VehicleCreateCommand, VehicleDTO>
{
    public async Task<VehicleDTO> Handle(VehicleCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.SelectAsync(v => v.CarNumber.Equals(request.CarNumber));
        if (entity is not null)
            throw new AlreadyExistException($"This vehicle already exist with number: {request.CarNumber}");

        entity = new Vehicle()
        {
            Model = request.Model,
            Brand = request.Brand,
            CarNumber = request.CarNumber,
            Color = request.Color,
        };

        if (request.Attachment is not null)
        {
            var attachment = await attachmentHandler.Handle(new AttachmentCreateCommand { FormFile = request.Attachment }, cancellationToken);
            entity.Attachment = attachment;
            entity.AttachmentId = attachment.Id;
        }

        await repository.InsertAsync(entity);
        await repository.SaveAsync();

        return mapper.Map<VehicleDTO>(entity);
    }
}
