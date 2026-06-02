// ============================================================
//  DbService — the actual logic. This is where the marks are.
// ============================================================

using Microsoft.EntityFrameworkCore;
using ApbdTest.Data;
using ApbdTest.DTOs;
using ApbdTest.Exceptions;
using ApbdTest.Entities;

namespace ApbdTest.Services;

public class DbService(DatabaseContext context) : IDbService
{
    // ==================== TASK 1 — GET ====================

    // SHAPE A: returns a LIST, optional string filter (Gr. A style)
    // GET /api/patients?lastName=Kow
    public async Task<List<MainEntityDto>> GetAllAsync(string? filter)
    {
        var query = context.MainEntities
            .Include(e => e.ChildEntities)
                .ThenInclude(c => c.LookupEntity)
            .Include(e => e.ChildEntities)
                .ThenInclude(c => c.JoinEntities)
                    .ThenInclude(j => j.LookupEntity)
            .AsQueryable();

        // apply optional filter
        if (!string.IsNullOrWhiteSpace(filter))
            query = query.Where(e => e.LastName.Contains(filter));

        var list = await query.ToListAsync();

        return list.Select(e => new MainEntityDto
        {
            FirstName = e.FirstName,
            LastName  = e.LastName,
            Date      = e.Date,
            Items     = e.ChildEntities.Select(c => new ChildDto
            {
                ChildId = c.ChildEntityId,
                Status  = c.Status,
                Date    = e.Date,
                Lookup  = new LookupDto { Name = c.LookupEntity.Name, Description = c.LookupEntity.Description, Price = c.LookupEntity.Price },
                JoinItems = c.JoinEntities.Select(j => new JoinDto
                {
                    Quantity = j.Quantity,
                    Price    = j.Price,
                    Lookup   = new LookupDto { Name = j.LookupEntity.Name, Description = j.LookupEntity.Description, Price = j.LookupEntity.Price },
                }).ToList()
            }).ToList()
        }).ToList();
    }

    // SHAPE B: returns a single entity by id, 404 if missing (Gr. B style)
    // GET /api/orders/{id}
    // public async Task<MainEntityDto> GetByIdAsync(int id)
    // {
    //     var entity = await context.MainEntities
    //         .Include(e => e.ChildEntities)
    //             .ThenInclude(c => c.LookupEntity)
    //         .FirstOrDefaultAsync(e => e.MainEntityId == id);
    //
    //     if (entity == null) throw new NotFoundException($"Entity {id} not found.");
    //
    //     return new MainEntityDto { ... };
    // }

    // ==================== TASK 2 — POST ====================
    // Create entity + assign to an existing lookup. All in a transaction.
    public async Task CreateAsync(CreateMainEntityRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            // 404 check: related entity must exist (look up by id or by name)
            var lookup = await context.LookupEntities
                .FirstOrDefaultAsync(l => l.LookupEntityId == request.Main.Id);
            if (lookup == null)
                throw new NotFoundException($"Lookup {request.Main.Id} not found.");

            // 400 check: date must not be in the past (if task requires it)
            if (request.Main.Date < DateTime.Now)
                throw new BadRequestException("Date cannot be in the past.");

            var entity = new MainEntity
            {
                FirstName = request.Main.FirstName,
                LastName  = request.Main.LastName,
                Date      = request.Main.Date,
            };
            context.MainEntities.Add(entity);

            // create the child that links entity → lookup
            context.ChildEntities.Add(new ChildEntity
            {
                MainEntity   = entity,
                LookupEntity = lookup,
                Status       = "Scheduled",
                Amount       = 0,
            });

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception) { await transaction.RollbackAsync(); throw; }
    }

    // ==================== TASK 2 — PUT ====================
    // Update existing entity in-place (no body, id from URL).
    // public async Task UpdateAsync(int id)
    // {
    //     await using var transaction = await context.Database.BeginTransactionAsync();
    //     try
    //     {
    //         var entity = await context.ChildEntities
    //             .Include(c => c.JoinEntities).ThenInclude(j => j.LookupEntity)
    //             .FirstOrDefaultAsync(c => c.ChildEntityId == id);
    //         if (entity == null) throw new NotFoundException($"Entity {id} not found.");
    //
    //         // 409 check (e.g. has payments)
    //         if (entity.JoinEntities.Any()) throw new ConflictException("Cannot update: has related records.");
    //
    //         entity.Status = "Processed";
    //         foreach (var j in entity.JoinEntities) {
    //             j.LookupEntity.Price = Math.Round(j.LookupEntity.Price * 0.9m, 2);
    //             j.Price = j.LookupEntity.Price;
    //         }
    //         entity.Amount = entity.JoinEntities.Sum(j => j.Price * j.Quantity);
    //
    //         await context.SaveChangesAsync();
    //         await transaction.CommitAsync();
    //     }
    //     catch (Exception) { await transaction.RollbackAsync(); throw; }
    // }
}
