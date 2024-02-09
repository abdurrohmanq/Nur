﻿using AutoMapper;
using Nur.Domain.Entities.Users;
using Nur.Domain.Entities.Products;
using Nur.Domain.Entities.Attachments;
using Nur.Application.UseCases.Users.DTOs;
using Nur.Application.UseCases.Products.DTOs;
using Nur.Application.UseCases.Users.Commands;
using Nur.Application.UseCases.Attachments.Commands;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Application.UseCases.ProductCategories.Commands;
using Nur.Application.UseCases.Attachments.DTOs;
using Nur.Domain.Entities.Addresses;
using Nur.Domain.Entities.Suppliers;
using Nur.Application.UseCases.Vehicles.DTOs;
using Nur.Application.UseCases.Addresses.DTOs;
using Nur.Application.UseCases.Suppliers.DTOs;
using Nur.Application.UseCases.Addresses.Commands;
using Nur.Application.UseCases.Vehicles.Commands;
using Nur.Application.UseCases.Suppliers.Commands;
using Nur.Application.UseCases.Payments.Commands;
using Nur.Domain.Entities.Payments;
using Nur.Application.UseCases.Payments.DTOs;

namespace Nur.Application.Commons.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Users
        CreateMap<UserDTO, User>().ReverseMap();
        CreateMap<UserUpdateCommand, User>().ReverseMap();
        CreateMap<UserCreateCommand, User>().ReverseMap();

        //Products
        CreateMap<ProductDTO, Product>().ReverseMap();

        //ProductCategory
        CreateMap<ProductCategoryDTO, ProductCategory>().ReverseMap();
        CreateMap<CategoryCreateCommand, ProductCategory>().ReverseMap();
        CreateMap<CategoryUpdateCommand, ProductCategory>().ReverseMap();

        //Address
        CreateMap<AddressCreateCommand, Address>().ReverseMap();
        CreateMap<AddressUpdateCommand, Address>().ReverseMap();
        CreateMap<AddressDTO, Address>().ReverseMap();

        //Supplier
        CreateMap<SupplierCreateCommand, Supplier>().ReverseMap();
        CreateMap<SupplierUpdateCommand, Supplier>().ReverseMap();
        CreateMap<SupplierDTO, Supplier>().ReverseMap();

        //Vehicles
        CreateMap<VehicleCreateCommand, Vehicle>().ReverseMap();
        CreateMap<VehicleUpdateCommand, Vehicle>().ReverseMap();
        CreateMap<VehicleDTO, Vehicle>().ReverseMap();

        //Payments
        CreateMap<PaymentCreateCommand, Payment>().ReverseMap();
        CreateMap<PaymentUpdateCommand, Payment>().ReverseMap();
        CreateMap<PaymentDTO, Payment>().ReverseMap();

        //Attachment
        CreateMap<Attachment, AttachmentDTO>().ReverseMap();
    }
}
