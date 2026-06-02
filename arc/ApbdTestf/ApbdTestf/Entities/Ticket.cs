using System.ComponentModel.DataAnnotations;

namespace ApbdTestf.Entities;

public class Ticket
{
    [Key]
    public int TicketId { get; set; }

    [MaxLength(50)]
    public string SerialNumber { get; set; } = string.Empty;

    public int SeatNumber { get; set; }

    public ICollection<TicketConcert> TicketConcerts { get; set; } = [];
}
