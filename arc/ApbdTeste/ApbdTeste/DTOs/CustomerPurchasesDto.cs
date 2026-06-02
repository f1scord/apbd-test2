namespace ApbdTeste.DTOs;

public class CustomerPurchasesDto
{
    public string  FirstName   { get; set; } = string.Empty;
    public string  LastName    { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public List<PurchaseDto> Purchases { get; set; } = [];
}

public class PurchaseDto
{
    public DateTime Date    { get; set; }
    public int?     Rating  { get; set; }
    public decimal  Price   { get; set; }
    public WashingMachineDto WashingMachine { get; set; } = null!;
    public ProgramDto        Program        { get; set; } = null!;
}

public class WashingMachineDto
{
    public string  Serial    { get; set; } = string.Empty;
    public decimal MaxWeight { get; set; }
}

public class ProgramDto
{
    public string Name     { get; set; } = string.Empty;
    public int    Duration { get; set; }
}
