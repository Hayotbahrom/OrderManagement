using OrderManagement.Service.DTOs.Reports;

namespace OrderManagement.Service.Interfaces;

public interface IReportService
{
    Task<List<ProductSalesReportDto>> GetProductSalesAsync();
    Task<List<DailySalesReportDto>> GetDailySalesAsync(DateTime from, DateTime to);
}