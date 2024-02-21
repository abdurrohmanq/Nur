using AutoMapper;
using Nur.Domain.Entities.Orders;
using Nur.Domain.Entities.Payments;
using Nur.Domain.Entities.Products;
using Nur.Domain.Entities.Users;
using Nur.Domain.Entities.Addresses;
using Nur.Domain.Entities.Suppliers;
using Nur.Domain.Entities.Attachments;
using Nur.Application.UseCases.Users.DTOs;
using Nur.Application.UseCases.Orders.DTOs;
using Nur.Application.UseCases.Payments.DTOs;
using Nur.Application.UseCases.Products.DTOs;
using Nur.Application.UseCases.Vehicles.DTOs;
using Nur.Application.UseCases.Suppliers.DTOs;
using Nur.Application.UseCases.Users.Commands;
using Nur.Application.UseCases.Addresses.DTOs;
using Nur.Application.UseCases.Orders.Commands;
using Nur.Application.UseCases.Attachments.DTOs;
using Nur.Application.UseCases.Payments.Commands;
using Nur.Application.UseCases.Addresses.Commands;
using Nur.Application.UseCases.Vehicles.Commands;
using Nur.Application.UseCases.Suppliers.Commands;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;
using Nur.Application.UseCases.ProductCategories.Commands;
using Nur.Application.UseCases.Orders.OrderItems.Commands;
using Nur.Application.UseCases.Carts.CartItems.DTOs;
using Nur.Domain.Entities.Carts;
using Nur.Application.UseCases.Carts.DTOs;
using Nur.Application.UseCases.Carts.CartItems.Commands;
using Nur.Application.UseCases.Cafes.Commands;
using Nur.Domain.Entities.Cafes;
using Nur.Application.UseCases.Cafes.DTOs;

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

        //Orders
        CreateMap<OrderCreateCommand, Order>().ReverseMap();
        CreateMap<OrderUpdateCommand, Order>().ReverseMap();
        CreateMap<OrderDTO, Order>().ReverseMap();

        //OrderItems
        CreateMap<OrderItemCreateCommand, OrderItem>().ReverseMap();
        CreateMap<OrderItemUpdateCommand, OrderItem>().ReverseMap();
        CreateMap<OrderItem, OrderItemDTO>().ReverseMap();

        //Carts
        CreateMap<Cart, CartDTO>().ReverseMap();

        //CartItems
        CreateMap<CartItemCreateCommand, CartItem>().ReverseMap();
        CreateMap<CartItemUpdateCommand, CartItem>().ReverseMap();
        CreateMap<CartItem, CartItemDTO>().ReverseMap();

        //Cafes
        CreateMap<CafeCreateCommand, Cafe>().ReverseMap();
        CreateMap<CafeUpdateCommand, Cafe>().ReverseMap();
        CreateMap<Cafe, CafeDTO>().ReverseMap();

        //Attachment
        CreateMap<Attachment, AttachmentDTO>().ReverseMap();
    }
}
