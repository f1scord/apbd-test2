using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApbdTeste.Entities;

public class AvailableProgram
{
    [Key]
    public int AvailableProgramId { get; set; }

    public int WashingMachineId { get; set; }
    public WashingMachine WashingMachine { get; set; } = null!;

    public int ProgramId { get; set; }
    public Program Program { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public ICollection<PurchaseHistory> PurchaseHistories { get; set; } = [];
}
