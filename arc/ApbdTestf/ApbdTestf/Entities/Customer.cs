using System.ComponentModel.DataAnnotations;

namespace ApbdTestf.Entities;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? PhoneNumber { get; set; }

    public ICollection<PurchasedTicket> PurchasedTickets { get; set; } = [];
}
