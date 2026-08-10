using Microsoft.EntityFrameworkCore;
using OrderManagement.Api;
using OrderManagement.Data.Reposiroty;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Service.DTOs.Commons;
using OrderManagement.Service.DTOs.Orders;
using OrderManagement.Service.Exceptions;
using OrderManagement.Service.Interfaces;
using OrderManagement.Service.Mappings;

namespace OrderManagement.Service.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IRepository<Product> productRepository) : IOrderService
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IRepository<Product> _productRepository = productRepository;

    public async Task<OrderDetailsViewDto> AddAsync(OrderCreateDto dto)
    {
        if (dto.Items is null || dto.Items.Count == 0)
            throw new CustomException(400, "Order must contain at least one product");

        var order = new Order
        {
            CustomerName = dto.CustomerName,
            Status = OrderStatus.New,
            CreatedAt = DateTime.UtcNow
        };

        decimal totalAmount = 0;

        foreach (var item in dto.Items)
        {
            var product = await _productRepository.SelectByIdAsync(item.ProductId);
            if (product is null)
                throw new CustomException(404, $"Product with id {item.ProductId} not found");

            if (product.StockQuantity < item.Quantity)
                throw new CustomException(400,
                    $"Not enough stock for '{product.Name}'. Available: {product.StockQuantity}, requested: {item.Quantity}");

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                Price = product.Price
            });

            product.StockQuantity -= item.Quantity;
            totalAmount += product.Price * item.Quantity;
        }

        order.TotalAmount = totalAmount;

        await _orderRepository.InsertAsync(order);
        await _orderRepository.SaveChangesAsync();

        return await GetByIdAsync(order.Id);
    }

    public async Task<PagedResult<OrderViewDto>> GetAllAsync(OrderFilterDto filter)
    {
        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize < 1 ? 20 : filter.PageSize;

        var query = _orderRepository.SelectAll();

        if (filter.Status.HasValue)
            query = query.Where(o => o.Status == filter.Status.Value);

        if (filter.From.HasValue)
        {
            var from = DateTime.SpecifyKind(filter.From.Value.Date, DateTimeKind.Utc);
            query = query.Where(o => o.CreatedAt >= from);
        }

        if (filter.To.HasValue)
        {
            var to = DateTime.SpecifyKind(filter.To.Value.Date, DateTimeKind.Utc).AddDays(1);
            query = query.Where(o => o.CreatedAt < to);
        }

        var totalCount = await query.CountAsync();

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<OrderViewDto>
        {
            Items = orders.Select(o => o.ToViewDto()).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<OrderDetailsViewDto> GetByIdAsync(int id)
    {
        var order = await _orderRepository.SelectByIdWithItemsAsync(id);
        if (order is null)
            throw new CustomException(404, $"Order with id {id} not found");

        return order.ToDetailsViewDto();
    }

    public async Task<OrderViewDto> ChangeStatusAsync(int id, OrderStatus status)
    {
        var order = await _orderRepository.SelectByIdAsync(id);
        if (order is null)
            throw new CustomException(404, $"Order with id {id} not found");

        if (order.Status != OrderStatus.New)
            throw new CustomException(400,
                $"Order status cannot be changed from '{order.Status}'");

        order.Status = status;

        _orderRepository.Update(order);
        await _orderRepository.SaveChangesAsync();

        return order.ToViewDto();
    }
}