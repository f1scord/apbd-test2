using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTestb.Entities;

[Table("Payments")]
public class Payment
{
    [Key] public int PaymentId { get; set; }
    [MaxLength(100)] public string PaymentMethod  { get; set; } = null!;
    [Precision(10, 2)] public decimal Amount      { get; set; }
    [MaxLength(100)] public string PaymentStatus  { get; set; } = null!;

    [ForeignKey(nameof(Order))] public int Orders_OrderId { get; set; }
    public Order Order { get; set; } = null!;
}
