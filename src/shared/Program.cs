using Microsoft.Identity.Web;
using SharedDataApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseAuthentication();
app.UseAuthorization();

// Products
app.MapGet("/api/products", () => DataStore.Products).RequireAuthorization();
app.MapGet("/api/products/{id:int}", (int id) =>
{
    var product = DataStore.Products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
}).RequireAuthorization();

// Users
app.MapGet("/api/users", () => DataStore.Users).RequireAuthorization();
app.MapGet("/api/users/{id:int}", (int id) =>
{
    var user = DataStore.Users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
}).RequireAuthorization();

// Orders
app.MapGet("/api/orders", () => DataStore.Orders).RequireAuthorization();
app.MapGet("/api/orders/{id:int}", (int id) =>
{
    var order = DataStore.Orders.FirstOrDefault(o => o.Id == id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
}).RequireAuthorization();

app.Run();
