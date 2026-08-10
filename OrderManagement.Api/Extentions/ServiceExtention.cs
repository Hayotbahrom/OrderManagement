using OrderManagement.Data.Reposiroty;
using OrderManagement.Service.Interfaces;
using OrderManagement.Service.Services;

namespace OrderManagement.Api.Extentions;

public static class ServiceExtention
{
    public static void AddServiceExtention(this IServiceCollection service)
    {
        // repo
        service.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        service.AddScoped<IOrderRepository, OrderRepository>();

        //services
        service.AddScoped<IOrderService, OrderService>();
        service.AddScoped<IProductService, ProductService>();
    }
}
