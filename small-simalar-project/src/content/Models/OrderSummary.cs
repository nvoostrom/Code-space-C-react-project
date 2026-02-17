namespace ContentApi.Models;

public class OrderSummary
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

public class OrderDetail
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<OrderItemDto> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ShippingMethod { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
