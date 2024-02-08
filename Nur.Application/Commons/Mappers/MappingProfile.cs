using AutoMapper;
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
using Nur.Application.UseCases.Addresses.Commands;
using Nur.Domain.Entities.Addresses;
using Nur.Application.UseCases.Addresses.DTOs;

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
        //Attachment
        CreateMap<Attachment, AttachmentDTO>().ReverseMap();
    }
}
