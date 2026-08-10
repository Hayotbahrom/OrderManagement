using OrderManagement.Domain.Enums;
using OrderManagement.Service.DTOs.Commons;
using OrderManagement.Service.DTOs.Orders;

namespace OrderManagement.Service.Interfaces;

public interface IOrderService
{
    Task<OrderDetailsViewDto> AddAsync(OrderCreateDto dto);
    Task<PagedResult<OrderViewDto>> GetAllAsync(OrderFilterDto filter);
    Task<OrderDetailsViewDto> GetByIdAsync(int id);
    Task<OrderViewDto> ChangeStatusAsync(int id, OrderStatus status);
}