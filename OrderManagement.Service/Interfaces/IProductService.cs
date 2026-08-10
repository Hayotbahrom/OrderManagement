using OrderManagement.Service.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Service.Interfaces;

public interface IProductService
{
    Task<ProductViewDto> AddAsync(ProductCreateDto dto);
    Task<IEnumerable<ProductViewDto>> GetAllAsync();
    Task<ProductViewDto> GetByIdAsync(int id);
    Task<ProductViewDto> UpdateAsync(int id, ProductUpdateDto dto);
    Task DeleteAsync(int id);
}
