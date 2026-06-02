using ApbdTestb.Exceptions;
using ApbdTestb.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApbdTestb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IDbService dbService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderAsync(int id)
    {
        try
        {
            var order = await dbService.GetOrderByIdAsync(id);
            return Ok(order);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOrderAsync(int id)
    {
        try
        {
            await dbService.UpdateOrderAsync(id);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
}
