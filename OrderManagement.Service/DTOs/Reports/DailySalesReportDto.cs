namespace OrderManagement.Service.DTOs.Reports;

public class DailySalesReportDto
{
    public DateTime Date { get; set; }
    public int OrdersCount { get; set; }
    public int ProductsSold { get; set; }
    public decimal TotalRevenue { get; set; }
}