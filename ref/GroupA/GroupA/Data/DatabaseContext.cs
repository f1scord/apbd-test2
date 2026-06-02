using GroupA.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupA.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Concert>         Concerts         { get; set; }
    public DbSet<Ticket>          Tickets          { get; set; }
    public DbSet<TicketConcert>   TicketConcerts   { get; set; }
    public DbSet<Customer>        Customers        { get; set; }
    public DbSet<PurchasedTicket> PurchasedTickets { get; set; }

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Concert>().HasData(new List<Concert>
        {
            new() { ConcertId = 1, Name = "Concert 1", Date = DateTime.Today, AvailableTickets = 1000 },
            new() { ConcertId = 2, Name = "Concert 2", Date = DateTime.Today, AvailableTickets = 500 },
        });

        modelBuilder.Entity<Ticket>().HasData(new List<Ticket>
        {
            new() { TicketId = 1, SerialNumber = "SER12345", SeatNumber = 10 },
            new() { TicketId = 2, SerialNumber = "SER54321", SeatNumber = 11 },
        });

        modelBuilder.Entity<TicketConcert>().HasData(new List<TicketConcert>
        {
            new() { TicketConcertId = 1, TicketId = 1, ConcertId = 1, Price = 10 },
            new() { TicketConcertId = 2, TicketId = 2, ConcertId = 2, Price = 15 },
        });

        modelBuilder.Entity<Customer>().HasData(new List<Customer>
        {
            new() { CustomerId = 1, FirstName = "John", LastName = "Doe",  PhoneNumber = null },
            new() { CustomerId = 2, FirstName = "Jane", LastName = "Doe",  PhoneNumber = "+123456789" },
        });

        modelBuilder.Entity<PurchasedTicket>().HasData(new List<PurchasedTicket>
        {
            new() { TicketConcertId = 1, CustomerId = 1, PurchaseDate = DateTime.Today },
            new() { TicketConcertId = 2, CustomerId = 1, PurchaseDate = DateTime.Today },
            new() { TicketConcertId = 1, CustomerId = 2, PurchaseDate = DateTime.Today },
            new() { TicketConcertId = 2, CustomerId = 2, PurchaseDate = DateTime.Today },
        });
    }
}
