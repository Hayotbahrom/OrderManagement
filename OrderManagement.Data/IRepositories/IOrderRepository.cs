using OrderManagement.Data.Reposiroty;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Api;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> SelectByIdWithItemsAsync(int id);
}
