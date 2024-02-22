using MediatR;
using System.Reflection;
using Nur.Application.Commons.Mappers;
using Nur.Domain.Entities.Attachments;
using Nur.Application.UseCases.Carts.DTOs;
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
using Nur.Application.UseCases.Carts.CartItems.DTOs;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;
using Nur.Application.UseCases.Carts.Queries.GetCarts;
using Nur.Application.UseCases.ProductCategories.DTOs;
using Nur.Application.UseCases.Users.Queries.GetUsers;
using Nur.Application.UseCases.Carts.CartItems.Commands;
using Nur.Application.UseCases.Orders.Queries.GetOrders;
using Nur.Application.UseCases.ProductCategories.Queries;
using Nur.Application.UseCases.Orders.OrderItems.Commands;
using Nur.Application.UseCases.ProductCategories.Commands;
using Nur.Application.UseCases.Products.Queries.GetProducts;
using Nur.Application.UseCases.Vehicles.Queries.GetVehicles;
using Nur.Application.UseCases.Suppliers.Queries.GetSuppliers;
using Nur.Application.UseCases.Carts.CartItems.Queries.GetCartItems;
using Nur.Application.UseCases.Orders.OrderItems.Queries.GetOrderItems;
using Nur.Application.UseCases.Cafes.Commands;
using Nur.Application.UseCases.Cafes.DTOs;
using Nur.Application.UseCases.Cafes.Queries;

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
        services.AddScoped<IRequestHandler<GetProductByNameQuery, ProductDTO>, GetProductByNameQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>, GetAllProductQueryHandler>();
        services.AddScoped<IRequestHandler<GetByCategoryIdQuery, IEnumerable<ProductDTO>>, GetByCategoryIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetByCategoryNameQuery, IEnumerable<ProductDTO>>, GetByCategoryNameQueryHandler>();

        //ProductCategories
        services.AddScoped<IRequestHandler<CategoryCreateCommand, ProductCategoryDTO>, CategoryCreateCommandHandler>();
        services.AddScoped<IRequestHandler<CategoryUpdateCommand, ProductCategoryDTO>, CategoryUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<CategoryDeleteCommand, bool>, CategoryDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetCategoryQuery, ProductCategoryDTO>, GetCategoryQueryHandler>();
        services.AddScoped<IRequestHandler<GetCategoryByNameQuery, ProductCategoryDTO>, GetCategoryByNameQueryHandler>();
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
        services.AddScoped<IRequestHandler<GetByVehicleIdQuery, IEnumerable<SupplierDTO>>, GetByVehicleIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDTO>>, GetAllSupplierQueryHandler>();

        //Orders
        services.AddScoped<IRequestHandler<OrderCreateCommand, OrderDTO>, OrderCreateCommandHandler>();
        services.AddScoped<IRequestHandler<OrderUpdateCommand, OrderDTO>, OrderUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<OrderDeleteCommand, bool>, OrderDeleteCommandHandler>();

        services.AddScoped<IRequestHandler<GetOrderQuery, OrderDTO>, GetOrderQueryHandler>();
        services.AddScoped<IRequestHandler<GetByUserIdQuery, IEnumerable<OrderDTO>>, GetByUserIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetBySupplierIdQuery, IEnumerable<OrderDTO>>, GetBySupplierIdQueryHandler>();
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

        //OrderItems
        services.AddScoped<IRequestHandler<OrderItemCreateCommand, OrderItemDTO>, OrderItemCreateCommandHandler>();
        services.AddScoped<IRequestHandler<OrderItemUpdateCommand, OrderItemDTO>, OrderItemUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<OrderItemDeleteCommand, bool>, OrderItemDeleteCommandHandler>(); 

        services.AddScoped<IRequestHandler<GetOrderItemQuery, OrderItemDTO>, GetOrderItemQueryHandler>();
        services.AddScoped<IRequestHandler<GetByOrderIdQuery, IEnumerable<OrderItemDTO>>, GetByOrderIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetOrderItemByProductIdQuery, IEnumerable<OrderItemDTO>>, GetOrderItemByProductIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllOrderItemsQuery, IEnumerable<OrderItemDTO>>, GetAllOrderItemQueryHandler>();

        //Cart
        services.AddScoped<IRequestHandler<GetCartByUserIdQuery, CartDTO>, GetCartQueryHandler>();

        //CartItems
        services.AddScoped<IRequestHandler<CartItemCreateCommand, CartItemDTO>, CartItemCreateCommandHandler>();
        services.AddScoped<IRequestHandler<CartItemUpdateCommand, CartItemDTO>, CartItemUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<CartItemDeleteCommand, bool>, CartItemDeleteCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteAllCartItemsCommand, bool>, DeleteAllCartItemsCommandHandler>();
        services.AddScoped<IRequestHandler<CartItemDeleteByProductNameCommand, bool>, CartItemDeleteByProductNameCommandHandler>();

        services.AddScoped<IRequestHandler<GetCartItemQuery, CartItemDTO>, GetCartItemQueryHandler>();
        services.AddScoped<IRequestHandler<GetByCartIdQuery, IEnumerable<CartItemDTO>>, GetByCartIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetByProductIdQuery, CartItemDTO>, GetByProductIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllCartItemsQuery, IEnumerable<CartItemDTO>>, GetAllCartItemsQueryHandler>();

        //Cafes
        services.AddScoped<IRequestHandler<CafeCreateCommand, CafeDTO>, CafeCreateCommandHandler>();
        services.AddScoped<IRequestHandler<CafeUpdateCommand, CafeDTO>, CafeUpdateCommandHandler>();
        services.AddScoped<IRequestHandler<CafeDeleteCommand, bool>, CafeDeleteCommandHandler>();

        services.AddScoped <IRequestHandler<GetCafeQuery, IEnumerable<CafeDTO>>, GetCafeQueryHandler>();

        //Attachment
        services.AddScoped<IRequestHandler<AttachmentCreateCommand, Attachment>, AttachmentCreateCommandHandler>();
        services.AddScoped<IRequestHandler<AttachmentRemoveCommand, bool>, AttachmentRemoveCommandHandler>();

        return services;
    }
}
