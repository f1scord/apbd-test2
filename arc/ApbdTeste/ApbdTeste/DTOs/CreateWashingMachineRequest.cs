using System.ComponentModel.DataAnnotations;

namespace ApbdTeste.DTOs;

public class CreateWashingMachineRequest
{
    public WashingMachineCreateDto             WashingMachine    { get; set; } = null!;
    public List<AvailableProgramCreateDto>     AvailablePrograms { get; set; } = [];
}

public class WashingMachineCreateDto
{
    [Range(8, double.MaxValue, ErrorMessage = "MaxWeight must be at least 8.")]
    public decimal MaxWeight    { get; set; }

    [Required]
    public string  SerialNumber { get; set; } = string.Empty;
}

public class AvailableProgramCreateDto
{
    [Required]
    public string  ProgramName { get; set; } = string.Empty;

    [Range(0.01, 25, ErrorMessage = "Price must not exceed 25.")]
    public decimal Price       { get; set; }
}
