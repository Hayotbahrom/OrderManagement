using Microsoft.AspNetCore.Mvc;
using OrderManagement.Service.Interfaces;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(IReportService reportService) : ControllerBase
{
    private readonly IReportService _reportService = reportService;

    [HttpGet("products")]
    public async Task<IActionResult> GetProductSalesAsync()
    {
        var result = await _reportService.GetProductSalesAsync();
        return Ok(result);
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GetDailySalesAsync(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        var result = await _reportService.GetDailySalesAsync(from, to);
        return Ok(result);
    }
}