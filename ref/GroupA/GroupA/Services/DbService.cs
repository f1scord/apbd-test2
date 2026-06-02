using GroupA.Data;
using GroupA.DTOs;
using GroupA.Exceptions;
using GroupA.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupA.Services;

public class DbService(DatabaseContext context) : IDbService
{
    public async Task<CustomerDataDto> GetCustomerDataAsync(int customerId)
    {
        var customer = await context.Customers
            .Include(c => c.PurchasedTickets)
                .ThenInclude(pt => pt.TicketConcert)
                    .ThenInclude(tc => tc.Ticket)
            .Include(c => c.PurchasedTickets)
                .ThenInclude(pt => pt.TicketConcert)
                    .ThenInclude(tc => tc.Concert)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (customer == null)
            throw new NotFoundException($"Customer with id {customerId} not found.");

        return new CustomerDataDto
        {
            FirstName   = customer.FirstName,
            LastName    = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Purchases   = customer.PurchasedTickets.Select(pt => new PurchaseDto
            {
                Date  = pt.PurchaseDate,
                Price = pt.TicketConcert.Price,
                Ticket = new TicketDto
                {
                    Serial     = pt.TicketConcert.Ticket.SerialNumber,
                    SeatNumber = pt.TicketConcert.Ticket.SeatNumber,
                },
                Concert = new ConcertDto
                {
                    Name = pt.TicketConcert.Concert.Name,
                    Date = pt.TicketConcert.Concert.Date,
                },
            }).ToList()
        };
    }

    public async Task CreateCustomerAsync(CreateCustomerRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var existing = await context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == request.Customer.Id);

            if (existing != null)
                throw new ConflictException($"Customer with id {request.Customer.Id} already exists.");

            var customer = new Customer
            {
                FirstName   = request.Customer.FirstName,
                LastName    = request.Customer.LastName,
                PhoneNumber = request.Customer.PhoneNumber,
            };
            context.Customers.Add(customer);

            foreach (var purchase in request.Purchases)
            {
                var concert = await context.Concerts
                    .FirstOrDefaultAsync(c => c.Name == purchase.ConcertName);

                if (concert == null)
                    throw new NotFoundException($"Concert '{purchase.ConcertName}' not found.");

                var ticket = new Ticket
                {
                    SerialNumber = $"SER{Guid.NewGuid().ToString()[..5].ToUpper()}",
                    SeatNumber   = purchase.SeatNumber,
                };
                context.Tickets.Add(ticket);

                var ticketConcert = new TicketConcert
                {
                    Price   = purchase.Price,
                    Ticket  = ticket,
                    Concert = concert,
                };
                context.TicketConcerts.Add(ticketConcert);

                var purchasedTicket = new PurchasedTicket
                {
                    TicketConcert = ticketConcert,
                    Customer      = customer,
                    PurchaseDate  = DateTime.Now,
                };
                context.PurchasedTickets.Add(purchasedTicket);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
