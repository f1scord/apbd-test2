using ApbdTeste.DTOs;
using ApbdTeste.Exceptions;
using ApbdTeste.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApbdTeste.Controllers;

[Route("api/washing-machines")]
[ApiController]
public class WashingMachinesController(IDbService dbService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateWashingMachineAsync([FromBody] CreateWashingMachineRequest request)
    {
        try
        {
            await dbService.CreateWashingMachineAsync(request);
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
