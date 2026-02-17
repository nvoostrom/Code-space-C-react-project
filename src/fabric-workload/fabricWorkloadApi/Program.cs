using FabricWorkloadApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var sharedApiBaseUrl = builder.Configuration["SharedApi:BaseUrl"] ?? "https+http://shared-api";
builder.Services.AddHttpClient<ContentService>(client =>
{
    client.BaseAddress = new Uri(sharedApiBaseUrl);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseCors();

// Products
app.MapGet("/api/content/products", async (ContentService svc) => await svc.GetProductsAsync());
app.MapGet("/api/content/products/{id:int}", async (int id, ContentService svc) =>
{
    var product = await svc.GetProductAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

// Users
app.MapGet("/api/content/users", async (ContentService svc) => await svc.GetUsersAsync());
app.MapGet("/api/content/users/{id:int}", async (int id, ContentService svc) =>
{
    var user = await svc.GetUserAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// Orders
app.MapGet("/api/content/orders", async (ContentService svc) => await svc.GetOrdersAsync());
app.MapGet("/api/content/orders/{id:int}", async (int id, ContentService svc) =>
{
    var order = await svc.GetOrderAsync(id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
});

// Dashboard
app.MapGet("/api/content/dashboard", async (ContentService svc) => await svc.GetDashboardAsync());

app.Run();

public partial class Program { }
