using OrderManagement.Domain.Entities;
using OrderManagement.Service.DTOs.Orders;

namespace OrderManagement.Service.Mappings;

public static class OrderMappings
{
    public static OrderViewDto ToViewDto(this Order order)
    {
        return new OrderViewDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt
        };
    }

    public static OrderDetailsViewDto ToDetailsViewDto(this Order order)
    {
        return new OrderDetailsViewDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(item => new OrderItemViewDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        };
    }
}