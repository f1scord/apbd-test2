using Microsoft.EntityFrameworkCore;

namespace ApbdTeste.Entities;

[PrimaryKey(nameof(AvailableProgramId), nameof(CustomerId))]
public class PurchaseHistory
{
    public int AvailableProgramId { get; set; }
    public AvailableProgram AvailableProgram { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime PurchaseDate { get; set; }

    public int? Rating { get; set; }
}
