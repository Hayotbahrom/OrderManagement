using OrderManagement.Domain.Enums;

namespace OrderManagement.Service.DTOs.Orders;

public class OrderStatusUpdateDto
{
    public OrderStatus Status { get; set; }
}