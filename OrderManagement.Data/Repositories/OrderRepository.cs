using Microsoft.EntityFrameworkCore;
using OrderManagement.Api;
using OrderManagement.Data.Contexts;
using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Data.Reposiroty;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }
    public async Task<Order?> SelectByIdWithItemsAsync(int id)
        => await Context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
}
