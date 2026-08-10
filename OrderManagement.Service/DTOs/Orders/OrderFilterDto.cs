using OrderManagement.Domain.Enums;

namespace OrderManagement.Service.DTOs.Orders;

public class OrderFilterDto
{
    public OrderStatus? Status { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}