using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SharedDataApi.Tests;

public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_Returns200()
    {
        var response = await _client.GetAsync("/api/products");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProducts_Returns50Items()
    {
        var products = await _client.GetFromJsonAsync<List<object>>("/api/products");
        Assert.NotNull(products);
        Assert.Equal(50, products.Count);
    }

    [Fact]
    public async Task GetProductById_Returns200ForExistingId()
    {
        var response = await _client.GetAsync("/api/products/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProductById_Returns404ForMissingId()
    {
        var response = await _client.GetAsync("/api/products/9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_Returns200()
    {
        var response = await _client.GetAsync("/api/users");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_Returns30Items()
    {
        var users = await _client.GetFromJsonAsync<List<object>>("/api/users");
        Assert.NotNull(users);
        Assert.Equal(30, users.Count);
    }

    [Fact]
    public async Task GetUserById_Returns404ForMissingId()
    {
        var response = await _client.GetAsync("/api/users/9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetOrders_Returns200()
    {
        var response = await _client.GetAsync("/api/orders");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetOrders_Returns100Items()
    {
        var orders = await _client.GetFromJsonAsync<List<object>>("/api/orders");
        Assert.NotNull(orders);
        Assert.Equal(100, orders.Count);
    }

    [Fact]
    public async Task GetOrderById_Returns404ForMissingId()
    {
        var response = await _client.GetAsync("/api/orders/9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
