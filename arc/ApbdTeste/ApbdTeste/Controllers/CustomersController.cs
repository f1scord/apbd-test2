using ApbdTeste.Exceptions;
using ApbdTeste.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApbdTeste.Controllers;

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
}
