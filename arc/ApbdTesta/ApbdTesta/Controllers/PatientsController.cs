using ApbdTesta.DTOs;
using ApbdTesta.Exceptions;
using ApbdTesta.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApbdTesta.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController(IDbService dbService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPatientsAsync([FromQuery] string? lastName)
    {
        var patients = await dbService.GetPatientsAsync(lastName);
        return Ok(patients);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatientAsync([FromBody] CreatePatientRequest request)
    {
        try
        {
            await dbService.CreatePatientAsync(request);
            return Created();
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
