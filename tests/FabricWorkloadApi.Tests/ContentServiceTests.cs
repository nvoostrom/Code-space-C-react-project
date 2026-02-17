using System.Net;
using System.Text;
using System.Text.Json;
using FabricWorkloadApi.Services;
using Xunit;

namespace FabricWorkloadApi.Tests;

public class ContentServiceTests
{
    private static ContentService CreateServiceWithResponse(string path, string jsonContent)
    {
        var handler = new MockHttpMessageHandler(path, jsonContent);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost") };
        return new ContentService(httpClient);
    }

    [Fact]
    public async Task GetProductsAsync_ReturnsMappedProducts()
    {
        var json = JsonSerializer.Serialize(new[]
        {
            new { id = 1, name = "Widget", category = "Electronics", price = 29.99 },
            new { id = 2, name = "Gadget", category = "Toys", price = 9.99 },
        });

        var svc = CreateServiceWithResponse("/api/products", json);
        var products = await svc.GetProductsAsync();

        Assert.Equal(2, products.Count);
        Assert.Equal("Widget", products[0].Name);
        Assert.Equal("Electronics", products[0].Category);
        Assert.Equal(29.99m, products[0].Price);
    }

    [Fact]
    public async Task GetUsersAsync_ReturnsMappedUsers()
    {
        var json = JsonSerializer.Serialize(new[]
        {
            new { id = 1, name = "Alice", email = "alice@example.com", role = "Admin" },
        });

        var svc = CreateServiceWithResponse("/api/users", json);
        var users = await svc.GetUsersAsync();

        Assert.Single(users);
        Assert.Equal("Alice", users[0].Name);
        Assert.Equal("alice@example.com", users[0].Email);
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsMappedOrders()
    {
        var json = JsonSerializer.Serialize(new[]
        {
            new { id = 1, userId = 5, totalAmount = 100.50, status = "Pending", createdAt = "2024-01-01T00:00:00Z" },
        });

        var svc = CreateServiceWithResponse("/api/orders", json);
        var orders = await svc.GetOrdersAsync();

        Assert.Single(orders);
        Assert.Equal(5, orders[0].UserId);
        Assert.Equal(100.50m, orders[0].TotalAmount);
        Assert.Equal("Pending", orders[0].Status);
    }

    [Fact]
    public async Task GetProductAsync_ReturnsNullFor404()
    {
        var handler = new MockHttpMessageHandler("/api/products/999", "", HttpStatusCode.NotFound);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost") };
        var svc = new ContentService(httpClient);

        var result = await svc.GetProductAsync(999);
        Assert.Null(result);
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _matchPath;
        private readonly string _responseContent;
        private readonly HttpStatusCode _statusCode;

        public MockHttpMessageHandler(string matchPath, string responseContent, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _matchPath = matchPath;
            _responseContent = responseContent;
            _statusCode = statusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_responseContent, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }
}
