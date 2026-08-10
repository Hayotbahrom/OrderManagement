using Microsoft.EntityFrameworkCore;
using OrderManagement.Data.Reposiroty;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using OrderManagement.Service.DTOs.Reports;
using OrderManagement.Service.Exceptions;
using OrderManagement.Service.Interfaces;

namespace OrderManagement.Service.Services;

public class ReportService(
    IRepository<Order> orderRepository,
    IRepository<OrderItem> orderItemRepository) : IReportService
{
    private readonly IRepository<Order> _orderRepository = orderRepository;
    private readonly IRepository<OrderItem> _orderItemRepository = orderItemRepository;

    public async Task<List<ProductSalesReportDto>> GetProductSalesAsync()
    {
        return await _orderItemRepository.SelectAll()
            .Where(item => item.Order.Status == OrderStatus.Completed)
            .GroupBy(item => new { item.ProductId, item.Product.Name })
            .Select(group => new ProductSalesReportDto
            {
                ProductId = group.Key.ProductId,
                ProductName = group.Key.Name,
                SoldQuantity = group.Sum(item => item.Quantity),
                TotalRevenue = group.Sum(item => item.Price * item.Quantity)
            })
            .OrderByDescending(report => report.TotalRevenue)
            .ToListAsync();
    }

    public async Task<List<DailySalesReportDto>> GetDailySalesAsync(DateTime from, DateTime to)
    {
        if (from > to)
            throw new CustomException(400, "'from' date cannot be later than 'to' date");

        var fromUtc = DateTime.SpecifyKind(from.Date, DateTimeKind.Utc);
        var toUtc = DateTime.SpecifyKind(to.Date, DateTimeKind.Utc).AddDays(1);

        var orderStats = await _orderRepository.SelectAll()
            .Where(order => order.Status == OrderStatus.Completed)
            .Where(order => order.CreatedAt >= fromUtc && order.CreatedAt < toUtc)
            .GroupBy(order => order.CreatedAt.Date)
            .Select(group => new
            {
                Date = group.Key,
                OrdersCount = group.Count(),
                TotalRevenue = group.Sum(order => order.TotalAmount)
            })
            .ToListAsync();

        var itemStats = await _orderItemRepository.SelectAll()
            .Where(item => item.Order.Status == OrderStatus.Completed)
            .Where(item => item.Order.CreatedAt >= fromUtc && item.Order.CreatedAt < toUtc)
            .GroupBy(item => item.Order.CreatedAt.Date)
            .Select(group => new
            {
                Date = group.Key,
                ProductsSold = group.Sum(item => item.Quantity)
            })
            .ToListAsync();

        var soldByDate = itemStats.ToDictionary(stat => stat.Date, stat => stat.ProductsSold);

        return orderStats
            .Select(stat => new DailySalesReportDto
            {
                Date = stat.Date,
                OrdersCount = stat.OrdersCount,
                ProductsSold = soldByDate.TryGetValue(stat.Date, out var sold) ? sold : 0,
                TotalRevenue = stat.TotalRevenue
            })
            .OrderBy(report => report.Date)
            .ToList();
    }
}