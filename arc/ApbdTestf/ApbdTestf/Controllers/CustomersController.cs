using ApbdTestf.DTOs;
using ApbdTestf.Exceptions;
using ApbdTestf.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApbdTestf.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(IDbService dbService) : ControllerBase
{
    [HttpGet("{id:int}/purchases")]
    public async Task<IActionResult> GetPurchasesAsync(int id)
    {
        try
        {
            var result = await dbService.GetCustomerPurchasesAsync(id);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest request)
    {
        try
        {
            await dbService.CreateCustomerAsync(request);
            return Created();
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}
