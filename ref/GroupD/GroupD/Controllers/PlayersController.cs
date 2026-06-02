using GroupD.DTOs;
using GroupD.Exceptions;
using GroupD.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupD.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayersController(IDbService dbService) : ControllerBase
{
    [HttpGet("{id:int}/matches")]
    public async Task<IActionResult> GetPlayerDataAsync(int id)
    {
        try
        {
            var data = await dbService.GetPlayerDataAsync(id);
            return Ok(data);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayerAsync([FromBody] CreatePlayerRequest request)
    {
        if (request.Matches.Count == 0)
            return BadRequest("At least one match should be provided.");

        try
        {
            await dbService.CreatePlayerAsync(request);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
