using Microsoft.EntityFrameworkCore;

namespace ApbdTestf.Entities;

[PrimaryKey(nameof(TicketConcertId), nameof(CustomerId))]
public class PurchasedTicket
{
    public int TicketConcertId { get; set; }
    public TicketConcert TicketConcert { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime PurchaseDate { get; set; }
}
