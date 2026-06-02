using ApbdTeste.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApbdTeste.Data;

public class DatabaseContext : DbContext
{
    public DbSet<WashingMachine>  WashingMachines  { get; set; }
    public DbSet<Entities.Program> Programs        { get; set; }
    public DbSet<AvailableProgram> AvailablePrograms { get; set; }
    public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
    public DbSet<Customer>        Customers         { get; set; }

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Program>().HasData(new List<Entities.Program>
        {
            new() { ProgramId = 1, Name = "Quick Wash",    DurationMinutes = 69,  TemperatureCelsius = 30 },
            new() { ProgramId = 2, Name = "Cotton Cycle",  DurationMinutes = 143, TemperatureCelsius = 60 },
            new() { ProgramId = 3, Name = "Synthetic",     DurationMinutes = 90,  TemperatureCelsius = 40 },
        });

        modelBuilder.Entity<WashingMachine>().HasData(new List<WashingMachine>
        {
            new() { WashingMachineId = 1, MaxWeight = 32.23m, SerialNumber = "WM2012/S431/12" },
            new() { WashingMachineId = 2, MaxWeight = 52.23m, SerialNumber = "WM2014/S491/28" },
        });

        modelBuilder.Entity<Customer>().HasData(new List<Customer>
        {
            new() { CustomerId = 1, FirstName = "John", LastName = "Doe",   PhoneNumber = null },
            new() { CustomerId = 2, FirstName = "Jane", LastName = "Smith", PhoneNumber = "555000111" },
        });

        modelBuilder.Entity<AvailableProgram>().HasData(new List<AvailableProgram>
        {
            new() { AvailableProgramId = 1, WashingMachineId = 1, ProgramId = 1, Price = 12.99m },
            new() { AvailableProgramId = 2, WashingMachineId = 2, ProgramId = 2, Price = 17.29m },
        });

        modelBuilder.Entity<PurchaseHistory>().HasData(new List<PurchaseHistory>
        {
            new() { AvailableProgramId = 1, CustomerId = 1, PurchaseDate = DateTime.Parse("2025-06-03T09:00:00"), Rating = 4    },
            new() { AvailableProgramId = 2, CustomerId = 1, PurchaseDate = DateTime.Parse("2025-06-04T09:00:00"), Rating = null },
        });
    }
}
