using System.ComponentModel.DataAnnotations;

namespace ApbdTeste.Entities;

public class Program
{
    [Key]
    public int ProgramId { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public int TemperatureCelsius { get; set; }

    public ICollection<AvailableProgram> AvailablePrograms { get; set; } = [];
}
