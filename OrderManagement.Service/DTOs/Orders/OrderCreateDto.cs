using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Service.DTOs.Orders;

public class OrderCreateDto
{
    [Required, MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, MinLength(1, ErrorMessage = "Kamida bitta product bo'lishi kerak")]
    public List<OrderItemCreateDto> Items { get; set; } = new();
}

public class OrderItemCreateDto
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity 0 dan katta bo'lishi kerak")]
    public int Quantity { get; set; }
}