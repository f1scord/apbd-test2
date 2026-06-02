using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupA.Models;

[Table("Ticket")]
public class Ticket
{
    [Key] public int TicketId { get; set; }
    [MaxLength(20)] public string SerialNumber { get; set; } = null!;
    public int SeatNumber { get; set; }

    public ICollection<TicketConcert> TicketConcerts { get; set; } = null!;
}
