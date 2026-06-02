using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApbdTeste.Entities;

public class WashingMachine
{
    [Key]
    public int WashingMachineId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal MaxWeight { get; set; }

    [MaxLength(100)]
    public string SerialNumber { get; set; } = string.Empty;

    public ICollection<AvailableProgram> AvailablePrograms { get; set; } = [];
}
