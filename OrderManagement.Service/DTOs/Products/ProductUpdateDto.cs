using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Service.DTOs.Products;

public class ProductUpdateDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price 0 dan katta bo'lishi kerak")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
}