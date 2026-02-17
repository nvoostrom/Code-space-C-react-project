using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FabricWorkloadApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FabricWorkloadApi.Tests;

public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace the ContentService's HttpClient with one backed by mock data
                services.AddHttpClient<ContentService>(client =>
                {
                    client.BaseAddress = new Uri("http://localhost");
                }).ConfigurePrimaryHttpMessageHandler(() => new FakeSharedApiHandler());
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetContentProducts_Returns200()
    {
        var response = await _client.GetAsync("/api/content/products");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetContentProducts_ReturnsList()
    {
        var products = await _client.GetFromJsonAsync<List<object>>("/api/content/products");
        Assert.NotNull(products);
        Assert.Equal(2, products.Count);
    }

    [Fact]
    public async Task GetContentProductById_Returns200()
    {
        var response = await _client.GetAsync("/api/content/products/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetContentProductById_Returns404ForMissing()
    {
        var response = await _client.GetAsync("/api/content/products/9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetContentDashboard_Returns200()
    {
        var response = await _client.GetAsync("/api/content/dashboard");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>
    /// Mock handler that simulates SharedDataApi responses for the ContentService HttpClient.
    /// </summary>
    private class FakeSharedApiHandler : HttpMessageHandler
    {
        private static readonly string ProductsJson = JsonSerializer.Serialize(new[]
        {
            new { id = 1, name = "Widget", description = "A widget", category = "Electronics", price = 29.99, stock = 100, tags = new[] { "new" }, metadata = new Dictionary<string, string>(), createdAt = "2024-01-01T00:00:00Z", updatedAt = "2024-01-01T00:00:00Z" },
            new { id = 2, name = "Gadget", description = "A gadget", category = "Toys", price = 9.99, stock = 50, tags = new[] { "sale" }, metadata = new Dictionary<string, string>(), createdAt = "2024-01-01T00:00:00Z", updatedAt = "2024-01-01T00:00:00Z" },
        });

        private static readonly string UsersJson = JsonSerializer.Serialize(new[]
        {
            new { id = 1, name = "Alice", email = "alice@example.com", role = "Admin", department = "Engineering", phone = "+1-555-1234", lastLogin = "2024-01-01T00:00:00Z" },
        });

        private static readonly string OrdersJson = JsonSerializer.Serialize(new[]
        {
            new { id = 1, userId = 1, totalAmount = 100.50, status = "Pending", createdAt = "2024-01-01T00:00:00Z", updatedAt = "2024-01-01T00:00:00Z", notes = "", items = new[] { new { productId = 1, productName = "Widget", quantity = 2, unitPrice = 29.99 } }, shipping = new { method = "Standard", trackingNumber = "TRK-123", address = new { street = "123 Main", city = "NYC", state = "NY", zipCode = "10001", country = "USA" } }, billing = new { paymentMethod = "Credit Card", cardLastFour = "1234", address = new { street = "123 Main", city = "NYC", state = "NY", zipCode = "10001", country = "USA" } } },
        });

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = request.RequestUri?.AbsolutePath ?? "";
            string content;
            var status = HttpStatusCode.OK;

            if (path == "/api/products")
                content = ProductsJson;
            else if (path.StartsWith("/api/products/"))
            {
                var id = int.Parse(path.Split('/').Last());
                content = id <= 2
                    ? JsonSerializer.Serialize(new { id, name = $"Product {id}", description = "Desc", category = "Electronics", price = 29.99, stock = 100, tags = new[] { "new" }, metadata = new Dictionary<string, string>(), createdAt = "2024-01-01T00:00:00Z", updatedAt = "2024-01-01T00:00:00Z" })
                    : "";
                if (id > 2) status = HttpStatusCode.NotFound;
            }
            else if (path == "/api/users")
                content = UsersJson;
            else if (path.StartsWith("/api/users/"))
            {
                var id = int.Parse(path.Split('/').Last());
                content = id == 1
                    ? JsonSerializer.Serialize(new { id = 1, name = "Alice", email = "alice@example.com", role = "Admin", department = "Engineering", phone = "+1-555-1234", lastLogin = "2024-01-01T00:00:00Z" })
                    : "";
                if (id != 1) status = HttpStatusCode.NotFound;
            }
            else if (path == "/api/orders")
                content = OrdersJson;
            else if (path.StartsWith("/api/orders/"))
            {
                var id = int.Parse(path.Split('/').Last());
                content = id == 1
                    ? OrdersJson.TrimStart('[').TrimEnd(']')
                    : "";
                if (id != 1) status = HttpStatusCode.NotFound;
            }
            else
            {
                content = "";
                status = HttpStatusCode.NotFound;
            }

            return Task.FromResult(new HttpResponseMessage(status)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });
        }
    }
}
