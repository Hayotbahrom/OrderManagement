namespace OrderManagement.Service.DTOs.Reports;

public class ProductSalesReportDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int SoldQuantity { get; set; }
    public decimal TotalRevenue { get; set; }
}