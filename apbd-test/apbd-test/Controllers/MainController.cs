// ============================================================
//  CONTROLLER — wires HTTP to the service and maps exceptions.
//  Controller name decides the URL:
//    PatientsController → /api/patients
//    OrdersController   → /api/orders
// ============================================================
// TODO: rename class + fix routes to match the task

using Microsoft.AspNetCore.Mvc;
using ApbdTest.DTOs;
using ApbdTest.Exceptions;
using ApbdTest.Services;

namespace ApbdTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MainController(IDbService dbService) : ControllerBase
{
    // SHAPE A — GET /api/main  (returns list, optional ?filter= param)
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] string? filter)
    {
        var result = await dbService.GetAllAsync(filter);
        return Ok(result);
    }

    // SHAPE B — GET /api/main/{id}  (returns single, 404 if missing)
    // [HttpGet("{id:int}")]
    // public async Task<IActionResult> GetByIdAsync(int id)
    // {
    //     try { return Ok(await dbService.GetByIdAsync(id)); }
    //     catch (NotFoundException e) { return NotFound(e.Message); }
    // }

    // POST — create entity + assign
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMainEntityRequest request)
    {
        try
        {
            await dbService.CreateAsync(request);
            return Created();
        }
        catch (NotFoundException e)  { return NotFound(e.Message); }
        catch (BadRequestException e){ return BadRequest(e.Message); }
        catch (ConflictException e)  { return Conflict(e.Message); }
    }

    // PUT — update entity in-place (id in URL, no body needed)
    // [HttpPut("{id:int}")]
    // public async Task<IActionResult> UpdateAsync(int id)
    // {
    //     try { await dbService.UpdateAsync(id); return Ok(); }
    //     catch (NotFoundException e) { return NotFound(e.Message); }
    //     catch (ConflictException e) { return Conflict(e.Message); }
    // }
}
