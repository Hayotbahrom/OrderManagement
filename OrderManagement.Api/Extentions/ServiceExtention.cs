using OrderManagement.Data.Reposiroty;

namespace OrderManagement.Api.Extentions;

public static class ServiceExtention
{
    public static void AddServiceExtention(this IServiceCollection service)
    {
        // repo
        service.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        //services
    }
}
