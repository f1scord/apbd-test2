using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTestb.Entities;

[Table("Orders")]
public class Order
{
    [Key] public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    [MaxLength(100)] public string Status { get; set; } = null!;
    [Precision(10, 2)] public decimal TotalAmount { get; set; }

    [ForeignKey(nameof(User))] public int Users_UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Payment>   Payments   { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = null!;
}
