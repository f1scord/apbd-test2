// ============================================================
//  DATABASE CONTEXT
//  - one DbSet<> per entity (PLURAL names)
//  - seed data in OnModelCreating — no seed = -50%
//  - insert order: LookupEntity → MainEntity → ChildEntity → JoinEntity
// ============================================================

using Microsoft.EntityFrameworkCore;
using ApbdTest.Entities;

namespace ApbdTest.Data;

public class DatabaseContext : DbContext
{
    // TODO: rename DbSets to match your entities
    public DbSet<MainEntity>   MainEntities   { get; set; }
    public DbSet<LookupEntity> LookupEntities { get; set; }
    public DbSet<ChildEntity>  ChildEntities  { get; set; }
    public DbSet<JoinEntity>   JoinEntities   { get; set; }

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: fill with real data matching your ER diagram
        modelBuilder.Entity<LookupEntity>().HasData(new List<LookupEntity>
        {
            new() { LookupEntityId = 1, Name = "Item 1", Description = "Desc 1", Price = 100 },
            new() { LookupEntityId = 2, Name = "Item 2", Description = "Desc 2", Price = 200 },
        });

        modelBuilder.Entity<MainEntity>().HasData(new List<MainEntity>
        {
            new() { MainEntityId = 1, FirstName = "John", LastName = "Doe", Date = DateTime.Today },
            new() { MainEntityId = 2, FirstName = "Jane", LastName = "Doe", Date = DateTime.Today },
        });

        modelBuilder.Entity<ChildEntity>().HasData(new List<ChildEntity>
        {
            new() { ChildEntityId = 1, MainEntityId = 1, LookupEntityId = 1, Status = "Active", Amount = 100 },
        });

        modelBuilder.Entity<JoinEntity>().HasData(new List<JoinEntity>
        {
            new() { ChildEntityId = 1, LookupEntityId = 1, Quantity = 1, Price = 100 },
        });
    }
}
