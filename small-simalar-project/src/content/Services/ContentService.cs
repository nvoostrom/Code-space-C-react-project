using System.Text.Json;
using ContentApi.Models;

namespace ContentApi.Services;

public class ContentService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ContentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProductSummary>> GetProductsAsync()
    {
        var products = await _httpClient.GetFromJsonAsync<List<JsonElement>>("/api/products", JsonOptions) ?? [];
        return products.Select(p => new ProductSummary
        {
            Id = p.GetProperty("id").GetInt32(),
            Name = p.GetProperty("name").GetString() ?? "",
            Category = p.GetProperty("category").GetString() ?? "",
            Price = p.GetProperty("price").GetDecimal()
        }).ToList();
    }

    public async Task<ProductDetail?> GetProductAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/products/{id}");
        if (!response.IsSuccessStatusCode) return null;

        var p = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        return new ProductDetail
        {
            Id = p.GetProperty("id").GetInt32(),
            Name = p.GetProperty("name").GetString() ?? "",
            Description = p.GetProperty("description").GetString() ?? "",
            Category = p.GetProperty("category").GetString() ?? "",
            Price = p.GetProperty("price").GetDecimal(),
            Stock = p.GetProperty("stock").GetInt32(),
            Tags = p.GetProperty("tags").EnumerateArray().Select(t => t.GetString() ?? "").ToList()
        };
    }

    public async Task<List<UserSummary>> GetUsersAsync()
    {
        var users = await _httpClient.GetFromJsonAsync<List<JsonElement>>("/api/users", JsonOptions) ?? [];
        return users.Select(u => new UserSummary
        {
            Id = u.GetProperty("id").GetInt32(),
            Name = u.GetProperty("name").GetString() ?? "",
            Email = u.GetProperty("email").GetString() ?? "",
            Role = u.GetProperty("role").GetString() ?? ""
        }).ToList();
    }

    public async Task<UserDetail?> GetUserAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/users/{id}");
        if (!response.IsSuccessStatusCode) return null;

        var u = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        return new UserDetail
        {
            Id = u.GetProperty("id").GetInt32(),
            Name = u.GetProperty("name").GetString() ?? "",
            Email = u.GetProperty("email").GetString() ?? "",
            Role = u.GetProperty("role").GetString() ?? "",
            Department = u.GetProperty("department").GetString() ?? "",
            Phone = u.GetProperty("phone").GetString() ?? "",
            LastLogin = u.GetProperty("lastLogin").GetDateTime()
        };
    }

    public async Task<List<OrderSummary>> GetOrdersAsync()
    {
        var orders = await _httpClient.GetFromJsonAsync<List<JsonElement>>("/api/orders", JsonOptions) ?? [];
        return orders.Select(o => new OrderSummary
        {
            Id = o.GetProperty("id").GetInt32(),
            UserId = o.GetProperty("userId").GetInt32(),
            TotalAmount = o.GetProperty("totalAmount").GetDecimal(),
            Status = o.GetProperty("status").GetString() ?? "",
            Date = o.GetProperty("createdAt").GetDateTime()
        }).ToList();
    }

    public async Task<OrderDetail?> GetOrderAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/orders/{id}");
        if (!response.IsSuccessStatusCode) return null;

        var o = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        return new OrderDetail
        {
            Id = o.GetProperty("id").GetInt32(),
            UserId = o.GetProperty("userId").GetInt32(),
            TotalAmount = o.GetProperty("totalAmount").GetDecimal(),
            Status = o.GetProperty("status").GetString() ?? "",
            ShippingMethod = o.GetProperty("shipping").GetProperty("method").GetString() ?? "",
            TrackingNumber = o.GetProperty("shipping").GetProperty("trackingNumber").GetString() ?? "",
            Date = o.GetProperty("createdAt").GetDateTime(),
            Notes = o.GetProperty("notes").GetString() ?? "",
            Items = o.GetProperty("items").EnumerateArray().Select(i => new OrderItemDto
            {
                ProductId = i.GetProperty("productId").GetInt32(),
                ProductName = i.GetProperty("productName").GetString() ?? "",
                Quantity = i.GetProperty("quantity").GetInt32(),
                UnitPrice = i.GetProperty("unitPrice").GetDecimal()
            }).ToList()
        };
    }

    public async Task<DashboardData> GetDashboardAsync()
    {
        var productsTask = _httpClient.GetFromJsonAsync<List<JsonElement>>("/api/products", JsonOptions);
        var usersTask = _httpClient.GetFromJsonAsync<List<JsonElement>>("/api/users", JsonOptions);
        var ordersTask = _httpClient.GetFromJsonAsync<List<JsonElement>>("/api/orders", JsonOptions);

        await Task.WhenAll(productsTask, usersTask, ordersTask);

        var products = await productsTask ?? [];
        var users = await usersTask ?? [];
        var orders = await ordersTask ?? [];

        return new DashboardData
        {
            ProductCount = products.Count,
            UserCount = users.Count,
            OrderCount = orders.Count,
            TotalRevenue = orders.Sum(o => o.GetProperty("totalAmount").GetDecimal()),
            PendingOrders = orders.Count(o => o.GetProperty("status").GetString() == "Pending"),
            DeliveredOrders = orders.Count(o => o.GetProperty("status").GetString() == "Delivered"),
            CancelledOrders = orders.Count(o => o.GetProperty("status").GetString() == "Cancelled")
        };
    }
}
