using System.ComponentModel.DataAnnotations;

namespace ApbdTestf.DTOs;

public class CreateCustomerRequest
{
    public CreateCustomerDto       Customer  { get; set; } = null!;
    public List<CreatePurchaseDto> Purchases { get; set; } = [];
}

public class CreateCustomerDto
{
    public int     Id          { get; set; }

    [Required, MaxLength(50)]
    public string  FirstName   { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string  LastName    { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? PhoneNumber { get; set; }
}

public class CreatePurchaseDto
{
    public int     SeatNumber  { get; set; }

    [Required]
    public string  ConcertName { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price       { get; set; }
}
