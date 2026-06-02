using ApbdTestf.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApbdTestf.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Ticket>          Tickets          { get; set; }
    public DbSet<Concert>         Concerts         { get; set; }
    public DbSet<TicketConcert>   TicketConcerts   { get; set; }
    public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
    public DbSet<Customer>        Customers        { get; set; }

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Concert>().HasData(new List<Concert>
        {
            new() { ConcertId = 1, Name = "Concert 1",  Date = DateTime.Parse("2025-06-07T09:00:00"), AvailableTickets = 200 },
            new() { ConcertId = 2, Name = "Concert 14", Date = DateTime.Parse("2025-06-10T09:00:00"), AvailableTickets = 150 },
        });

        modelBuilder.Entity<Customer>().HasData(new List<Customer>
        {
            new() { CustomerId = 1, FirstName = "John", LastName = "Doe",   PhoneNumber = null      },
            new() { CustomerId = 2, FirstName = "Jane", LastName = "Smith", PhoneNumber = "555000111" },
        });

        modelBuilder.Entity<Ticket>().HasData(new List<Ticket>
        {
            new() { TicketId = 1, SerialNumber = "TK2034/S4531/12",  SeatNumber = 124 },
            new() { TicketId = 2, SerialNumber = "TK2027/S4831/133", SeatNumber = 330 },
        });

        modelBuilder.Entity<TicketConcert>().HasData(new List<TicketConcert>
        {
            new() { TicketConcertId = 1, TicketId = 1, ConcertId = 1, Price = 33.4m  },
            new() { TicketConcertId = 2, TicketId = 2, ConcertId = 2, Price = 48.4m  },
        });

        modelBuilder.Entity<PurchasedTicket>().HasData(new List<PurchasedTicket>
        {
            new() { TicketConcertId = 1, CustomerId = 1, PurchaseDate = DateTime.Parse("2025-06-03T09:00:00") },
            new() { TicketConcertId = 2, CustomerId = 1, PurchaseDate = DateTime.Parse("2025-06-03T09:00:00") },
        });
    }
}
