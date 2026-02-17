namespace SharedDataApi.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public string Status { get; set; } = string.Empty;
    public ShippingInfo Shipping { get; set; } = new();
    public BillingInfo Billing { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class ShippingInfo
{
    public string Method { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
}

public class BillingInfo
{
    public string PaymentMethod { get; set; } = string.Empty;
    public string CardLastFour { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
}
