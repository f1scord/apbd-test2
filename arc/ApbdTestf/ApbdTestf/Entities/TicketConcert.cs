using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApbdTestf.Entities;

public class TicketConcert
{
    [Key]
    public int TicketConcertId { get; set; }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public int ConcertId { get; set; }
    public Concert Concert { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public ICollection<PurchasedTicket> PurchasedTickets { get; set; } = [];
}
