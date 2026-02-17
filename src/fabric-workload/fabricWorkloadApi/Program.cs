using FabricWorkloadApi;
using FabricWorkloadApi.Services;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenForwardingHandler>();

builder.Services.AddHttpClient<ContentService>(client =>
{
    client.BaseAddress = new Uri("https+http://shared-api");
}).AddHttpMessageHandler<TokenForwardingHandler>();

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

app.UseAuthentication();
app.UseAuthorization();

// Products
app.MapGet("/api/content/products", async (ContentService svc) => await svc.GetProductsAsync()).RequireAuthorization();
app.MapGet("/api/content/products/{id:int}", async (int id, ContentService svc) =>
{
    var product = await svc.GetProductAsync(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
}).RequireAuthorization();

// Users
app.MapGet("/api/content/users", async (ContentService svc) => await svc.GetUsersAsync()).RequireAuthorization();
app.MapGet("/api/content/users/{id:int}", async (int id, ContentService svc) =>
{
    var user = await svc.GetUserAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
}).RequireAuthorization();

// Orders
app.MapGet("/api/content/orders", async (ContentService svc) => await svc.GetOrdersAsync()).RequireAuthorization();
app.MapGet("/api/content/orders/{id:int}", async (int id, ContentService svc) =>
{
    var order = await svc.GetOrderAsync(id);
    return order is not null ? Results.Ok(order) : Results.NotFound();
}).RequireAuthorization();

// Dashboard
app.MapGet("/api/content/dashboard", async (ContentService svc) => await svc.GetDashboardAsync()).RequireAuthorization();

app.Run();
