using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTestb.Entities;

[Table("Order_Items")]
[PrimaryKey(nameof(OrderId), nameof(ProductId))]
public class OrderItem
{
    [ForeignKey(nameof(Order))]   public int OrderId   { get; set; }
    [ForeignKey(nameof(Product))] public int ProductId { get; set; }

    public int Quantity { get; set; }
    [Precision(10, 2)] public decimal Price { get; set; }

    public Order   Order   { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
