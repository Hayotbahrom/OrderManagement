using Microsoft.EntityFrameworkCore;
using OrderManagement.Data.Reposiroty;
using OrderManagement.Domain.Entities;
using OrderManagement.Service.DTOs.Products;
using OrderManagement.Service.Exceptions;
using OrderManagement.Service.Interfaces;
using OrderManagement.Service.Mappings;

namespace OrderManagement.Service.Services;

public class ProductService(
    IRepository<Product> repository,
    IRepository<OrderItem> orderItemRepository) : IProductService
{
    private readonly IRepository<Product> _repository = repository;
    private readonly IRepository<OrderItem> _orderItemRepository = orderItemRepository;

    public async Task<ProductViewDto> AddAsync(ProductCreateDto dto)
    {
        var product = dto.ToEntity();

        await _repository.InsertAsync(product);
        await _repository.SaveChangesAsync();

        return product.ToViewDto();
    }

    public async Task<IEnumerable<ProductViewDto>> GetAllAsync()
    {
        var products = await _repository.SelectAll()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return products.Select(p => p.ToViewDto()).ToList();
    }

    public async Task<ProductViewDto> GetByIdAsync(int id)
    {
        var product = await _repository.SelectAsync(p => p.Id == id);
        if (product is null)
            throw new CustomException(404, $"Product with id {id} not found");

        return product.ToViewDto();
    }

    public async Task<ProductViewDto> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var product = await _repository.SelectByIdAsync(id);
        if (product is null)
            throw new CustomException(404, $"Product with id {id} not found");

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;

        _repository.Update(product);
        await _repository.SaveChangesAsync();

        return product.ToViewDto();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _repository.SelectByIdAsync(id);
        if (product is null)
            throw new CustomException(404, $"Product with id {id} not found");

        var isUsedInOrders = await _orderItemRepository.SelectAll()
            .AnyAsync(item => item.ProductId == id);

        if (isUsedInOrders)
            throw new CustomException(409, "This product is used in orders and cannot be deleted");

        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}