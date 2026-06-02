namespace ApbdTestf.DTOs;

public class CustomerPurchasesDto
{
    public string  FirstName   { get; set; } = string.Empty;
    public string  LastName    { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public List<PurchaseDto> Purchases { get; set; } = [];
}

public class PurchaseDto
{
    public DateTime   Date    { get; set; }
    public decimal    Price   { get; set; }
    public TicketDto  Ticket  { get; set; } = null!;
    public ConcertDto Concert { get; set; } = null!;
}

public class TicketDto
{
    public string Serial     { get; set; } = string.Empty;
    public int    SeatNumber { get; set; }
}

public class ConcertDto
{
    public string   Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
