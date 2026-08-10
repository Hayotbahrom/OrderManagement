using OrderManagement.Domain.Entities;
using OrderManagement.Service.DTOs.Products;

namespace OrderManagement.Service.Mappings;

public static class ProductMappings
{
    public static Product ToEntity(this ProductCreateDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static ProductViewDto ToViewDto(this Product product)
    {
        return new ProductViewDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt
        };
    }
}