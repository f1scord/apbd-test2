using System.ComponentModel.DataAnnotations;

namespace ApbdTestf.Entities;

public class Concert
{
    [Key]
    public int ConcertId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public int AvailableTickets { get; set; }

    public ICollection<TicketConcert> TicketConcerts { get; set; } = [];
}
