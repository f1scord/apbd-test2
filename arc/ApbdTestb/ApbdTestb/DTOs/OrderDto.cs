namespace ApbdTestb.DTOs;

public class OrderDto
{
    public int      OrderId     { get; set; }
    public DateTime OrderDate   { get; set; }
    public string   Status      { get; set; } = string.Empty;
    public decimal  TotalAmount { get; set; }
    public string   User        { get; set; } = string.Empty;
    public List<PaymentDto>   Payments   { get; set; } = [];
    public List<OrderItemDto> OrderItems { get; set; } = [];
}

public class PaymentDto
{
    public int     PaymentId     { get; set; }
    public string  PaymentMethod { get; set; } = string.Empty;
    public decimal Amount        { get; set; }
    public string  PaymentStatus { get; set; } = string.Empty;
}

public class OrderItemDto
{
    public ProductDto Product  { get; set; } = null!;
    public int        Quantity { get; set; }
    public decimal    Price    { get; set; }
}

public class ProductDto
{
    public int     ProductId     { get; set; }
    public string  Name          { get; set; } = string.Empty;
    public string  Description   { get; set; } = string.Empty;
    public decimal Price         { get; set; }
    public int     StockQuantity { get; set; }
}
