namespace GroupA.DTOs;

public class CreateCustomerRequest
{
    public CreateCustomerDto    Customer  { get; set; } = null!;
    public List<CreatePurchaseDto> Purchases { get; set; } = [];
}

public class CreateCustomerDto
{
    public int     Id          { get; set; }
    public string  FirstName   { get; set; } = string.Empty;
    public string  LastName    { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}

public class CreatePurchaseDto
{
    public int     SeatNumber   { get; set; }
    public string  ConcertName  { get; set; } = string.Empty;
    public decimal Price        { get; set; }
}
