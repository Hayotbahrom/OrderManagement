using Microsoft.AspNetCore.Mvc;
using OrderManagement.Service.DTOs.Orders;
using OrderManagement.Service.Interfaces;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] OrderCreateDto dto)
    {
        var result = await _orderService.AddAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] OrderFilterDto filter)
    {
        var result = await _orderService.GetAllAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var result = await _orderService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> ChangeStatusAsync(
        [FromRoute] int id,
        [FromBody] OrderStatusUpdateDto dto)
    {
        var result = await _orderService.ChangeStatusAsync(id, dto.Status);
        return Ok(result);
    }
}