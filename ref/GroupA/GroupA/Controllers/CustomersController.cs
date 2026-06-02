using GroupA.DTOs;
using GroupA.Exceptions;
using GroupA.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupA.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(IDbService dbService) : ControllerBase
{
    [HttpGet("{customerId:int}/purchases")]
    public async Task<IActionResult> GetCustomerDataAsync(int customerId)
    {
        try
        {
            var data = await dbService.GetCustomerDataAsync(customerId);
            return Ok(data);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest request)
    {
        if (request.Purchases.Count is 0 or > 5)
            return BadRequest("Purchases count must be between 1 and 5.");

        try
        {
            await dbService.CreateCustomerAsync(request);
            return Created();
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
