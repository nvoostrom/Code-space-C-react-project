using SharedDataApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();

// Products
app.MapGet("/api/products", () => DataStore.Products);
app.MapGet("/api/products/{id:int}", (int id) =>
{
    var product = DataStore.Products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

// Users
app.MapGet("/api/users", () => DataStore.Users);
app.MapGet("/api/users/{id:int}", (int id) =>
{
    var user = DataStore.Users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// Orders
app.MapGet("/api/orders", () => DataStore.Orders);
app.MapGet("/api/orders/{id:int}", (int id) =>
{
    var order = DataStore.Orders.FirstOrDefault(o => o.Id == id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
});

app.Run();

public partial class Program { }
