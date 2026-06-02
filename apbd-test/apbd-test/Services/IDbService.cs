// ============================================================
//  IDbService — method signatures for Task 1 + Task 2.
//
//  Task 1 is GET — two shapes:
//    (A) returns List<MainEntityDto>           — all entities, optional filter
//    (B) returns MainEntityDto                 — single by id, throws 404
//
//  Task 2 is POST or PUT:
//    POST — create new entity + assign to something (throws 404/400)
//    PUT  — update existing entity in-place (throws 404/409)
// ============================================================
// TODO: rename methods + keep only the signatures your task needs

using ApbdTest.DTOs;

namespace ApbdTest.Services;

public interface IDbService
{
    // SHAPE A — GET returns a list (optional string filter)
    Task<List<MainEntityDto>> GetAllAsync(string? filter);

    // SHAPE B — GET returns a single entity by id (404 if missing)
    // Task<MainEntityDto> GetByIdAsync(int id);

    // POST — create entity + assign to existing lookup
    Task CreateAsync(CreateMainEntityRequest req);

    // PUT — update existing entity in-place (no body, id from URL)
    // Task UpdateAsync(int id);
}
