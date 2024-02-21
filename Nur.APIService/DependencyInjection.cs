using Nur.APIService.Services;
using Nur.APIService.Constants;
using Nur.APIService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Nur.APIService;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpClient<IUserService, UserService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Users/");
        });
        
        services.AddHttpClient<IAddressService, AddressService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Addresses/");
        });
        
        services.AddHttpClient<IOrderService, OrderService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Orders/");
        });
        
        services.AddHttpClient<IOrderItemService, OrderItemService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/OrderItems/");
        });
        
        services.AddHttpClient<IProductService, ProductService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Products/");
        });
        
        services.AddHttpClient<IProductCategoryService, ProductCategoryService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/ProductCategories/");
        });
        
        services.AddHttpClient<IPaymentService, PaymentService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Payments/");
        });
        
        services.AddHttpClient<ISupplierService, SupplierService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Suppliers/");
        });
        
        services.AddHttpClient<IVehicleService, VehicleService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Vehicles/");
        });
        
        services.AddHttpClient<ICartService, CartService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Carts/");
        });
        
        services.AddHttpClient<ICartItemService, CartItemService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/CartItems/");
        });
        
        services.AddHttpClient<ICafeService, CafeService>(client =>
        {
            client.BaseAddress = new Uri($"{HttpConstant.BaseLink}api/Cafes/");
        });

        return services;
    }
}
