using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTestb.Entities;

[Table("Products")]
public class Product
{
    [Key] public int ProductId { get; set; }
    [MaxLength(100)] public string Name        { get; set; } = null!;
    [MaxLength(100)] public string Description { get; set; } = null!;
    [Precision(10, 2)] public decimal Price    { get; set; }
    public int StockQuantity { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = null!;
}
