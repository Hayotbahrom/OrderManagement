
using OrderManagement.Service.DTOs.Orders;

public class OrderDetailsViewDto : OrderViewDto
{
    public List<OrderItemViewDto> Items { get; set; } = new();
}

public class OrderItemViewDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal => Price * Quantity;
}