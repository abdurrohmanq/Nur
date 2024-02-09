using MediatR;
using System.Reflection;
using Nur.Application.Commons.Mappers;
using Nur.Domain.Entities.Attachments;
using Nur.Application.UseCases.Users.DTOs;
using Nur.Application.UseCases.Orders.DTOs;
using Nur.Application.UseCases.Products.DTOs;
using Nur.Application.UseCases.Payments.DTOs;
using Nur.Application.UseCases.Vehicles.DTOs;
using Nur.Application.UseCases.Suppliers.DTOs;
using Nur.Application.UseCases.Addresses.DTOs;
using Nur.Application.UseCases.Users.Commands;
using Microsoft.Extensions.DependencyInjection;
using Nur.Application.UseCases.Orders.Commands;
using Nur.Application.UseCases.Products.Commands;
using Nur.Application.UseCases.Vehicles.Commands;
using Nur.Application.UseCases.Addresses.Queries;
using Nur.Application.UseCases.Payments.Commands;
using Nur.Application.UseCases.Suppliers.Commands;
using Nur.Application.UseCases.Addresses.Commands;
using Nur.Application.UseCases.Attachments.Commands;
using Nur.Application.UseCases.Users.Queries.GetUsers;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Application.UseCases.Orders.Queries.GetOrders;
using Nur.Application.UseCases.ProductCategories.Queries;
using Nur.Application.UseCases.ProductCategories.Commands;
using Nur.Application.UseCases.Vehicles.Queries.GetVehicles;
using Nur.Application.UseCases.Products.Queries.GetProducts;
using Nur.Application.UseCases.Suppliers.Queries.GetSuppliers;

namespace Nur.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMediatR(Assembly.GetExecutingAssembly());

        //Users
        services.AddScoped<IRequestHandler<UserCreateCommand, UserDTO>, UserCreateCommandHandler>();
        services.AddScoped<IRequestHandler<UserUpdateCommand, UserDTO>, UserUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<UserDeleteCommand, bool>, UserDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetUserQuery, UserDTO>, GetUserQueryHandler>();
        services.AddScoped<IRequestHandler<GetUserByTelegramIdQuery, UserDTO>, GetUserByTelegramIdHandler>();
        services.AddScoped<IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>, GetAllUsersQueryHandler>();

        //Products
        services.AddScoped<IRequestHandler<ProductCreateCommand, ProductDTO>, ProductCreateCommandHandler>();
        services.AddScoped<IRequestHandler<ProductUpdateCommand, ProductDTO>, ProductUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<ProductDeleteCommand, bool>, ProductDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetProductQuery, ProductDTO>, GetProductQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>, GetAllProductQueryHandler>();
        services.AddScoped<IRequestHandler<GetByCategoryIdQuery, IEnumerable<ProductDTO>>, GetByCategoryIdQueryHandler>();

        //ProductCategories
        services.AddScoped<IRequestHandler<CategoryCreateCommand, ProductCategoryDTO>, CategoryCreateCommandHandler>();
        services.AddScoped<IRequestHandler<CategoryUpdateCommand, ProductCategoryDTO>, CategoryUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<CategoryDeleteCommand, bool>, CategoryDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetCategoryQuery, ProductCategoryDTO>, GetCategoryQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllProductCategoriesQuery, IEnumerable<ProductCategoryDTO>>, GetAllProductCategoriesQueryHandler>();

        //Address
        services.AddScoped<IRequestHandler<AddressCreateCommand, AddressDTO>, AddressCreateCommandHandler>();
        services.AddScoped<IRequestHandler<AddressUpdateCommand, AddressDTO>, AddressUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<AddressDeleteCommand, bool>, AddressDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetAddressQuery, AddressDTO>, GetAddressQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllAddressesQuery, IEnumerable<AddressDTO>>, GetAllAddressesQueryHandler>();

        //Supplier
        services.AddScoped<IRequestHandler<SupplierCreateCommand, SupplierDTO>, SupplierCreateCommandHandler>();
        services.AddScoped<IRequestHandler<SupplierUpdateCommand, SupplierDTO>, SupplierUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<SupplierDeleteCommand, bool>, SupplierDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetSupplierQuery, SupplierDTO>, GetSupplierQueryHandler>();
        services.AddScoped<IRequestHandler<GetByVehicleIdQuery, SupplierDTO>, GetByVehicleIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDTO>>, GetAllSupplierQueryHandler>();

        //Orders
        services.AddScoped<IRequestHandler<OrderCreateCommand, OrderDTO>, OrderCreateCommandHandler>();
        services.AddScoped<IRequestHandler<OrderUpdateCommand, OrderDTO>, OrderUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<OrderDeleteCommand, bool>, OrderDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetOrderQuery, OrderDTO>, GetOrderQueryHandler>();
        services.AddScoped<IRequestHandler<GetByUserIdQuery, OrderDTO>, GetByUserIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetBySupplierIdQuery, OrderDTO>, GetBySupplierIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDTO>>, GetAllOrderQueryHandler>();

        //Vehicles
        services.AddScoped<IRequestHandler<VehicleCreateCommand, VehicleDTO>, VehicleCreateCommandHandler>();
        services.AddScoped<IRequestHandler<VehicleUpdateCommand, VehicleDTO>, VehicleUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<VehicleDeleteCommand, bool>, VehicleDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetVehicleQuery, VehicleDTO>, GetVehicleQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleDTO>>, GetAllVehicleQueryHandler>();
       
        //Payments
        services.AddScoped<IRequestHandler<PaymentCreateCommand, PaymentDTO>, PaymentCreateCommandHandler>();
        services.AddScoped<IRequestHandler<PaymentUpdateCommand, PaymentDTO>, PaymentUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<PaymentDeleteCommand, bool>, PaymentDeleteCommandHandler>();

        //Attachment
        services.AddScoped<IRequestHandler<AttachmentCreateCommand, Attachment>, AttachmentCreateCommandHandler>();
        services.AddScoped<IRequestHandler<AttachmentRemoveCommand, bool>, AttachmentRemoveCommandHandler>();

        return services;
    }
}
