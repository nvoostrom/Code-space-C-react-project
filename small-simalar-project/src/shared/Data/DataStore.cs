using SharedDataApi.Models;

namespace SharedDataApi.Data;

public static class DataStore
{
    public static List<Product> Products { get; } = SeedProducts();
    public static List<User> Users { get; } = SeedUsers();
    public static List<Order> Orders { get; } = SeedOrders();

    private static List<Product> SeedProducts()
    {
        var categories = new[] { "Electronics", "Clothing", "Home & Garden", "Sports", "Books", "Toys", "Food", "Health" };
        var adjectives = new[] { "Premium", "Classic", "Modern", "Vintage", "Deluxe", "Essential", "Pro", "Ultra", "Eco", "Smart" };
        var nouns = new[] { "Widget", "Gadget", "Tool", "Device", "Kit", "Set", "Pack", "Bundle", "System", "Unit" };

        var products = new List<Product>();
        var rng = new Random(42);

        for (int i = 1; i <= 50; i++)
        {
            var category = categories[rng.Next(categories.Length)];
            products.Add(new Product
            {
                Id = i,
                Name = $"{adjectives[rng.Next(adjectives.Length)]} {nouns[rng.Next(nouns.Length)]} {i}",
                Description = $"High-quality {category.ToLower()} product. Features durable construction and excellent performance. Suitable for both professional and personal use. Comes with a 1-year warranty.",
                Category = category,
                Price = Math.Round((decimal)(rng.NextDouble() * 500 + 5), 2),
                Stock = rng.Next(0, 500),
                Tags = Enumerable.Range(0, rng.Next(2, 5))
                    .Select(_ => new[] { "bestseller", "new", "sale", "limited", "popular", "trending", "eco-friendly", "premium" }[rng.Next(8)])
                    .Distinct().ToList(),
                Metadata = new Dictionary<string, string>
                {
                    ["sku"] = $"SKU-{i:D5}",
                    ["weight"] = $"{Math.Round(rng.NextDouble() * 10 + 0.1, 2)} kg",
                    ["manufacturer"] = new[] { "AcmeCorp", "GlobalTech", "PrimeMakers", "QualityFirst", "TopBrand" }[rng.Next(5)],
                    ["origin"] = new[] { "USA", "Germany", "Japan", "UK", "Canada" }[rng.Next(5)]
                },
                CreatedAt = DateTime.UtcNow.AddDays(-rng.Next(1, 365)),
                UpdatedAt = DateTime.UtcNow.AddDays(-rng.Next(0, 30))
            });
        }

        return products;
    }

    private static List<User> SeedUsers()
    {
        var firstNames = new[] { "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace", "Henry", "Iris", "Jack", "Karen", "Leo", "Mia", "Noah", "Olivia" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
        var roles = new[] { "Admin", "Editor", "Viewer", "Manager", "Analyst" };
        var departments = new[] { "Engineering", "Marketing", "Sales", "Support", "Finance", "HR" };
        var cities = new[] { "New York", "San Francisco", "Chicago", "Austin", "Seattle", "Denver", "Boston", "Portland" };
        var states = new[] { "NY", "CA", "IL", "TX", "WA", "CO", "MA", "OR" };

        var users = new List<User>();
        var rng = new Random(42);

        for (int i = 1; i <= 30; i++)
        {
            var firstName = firstNames[rng.Next(firstNames.Length)];
            var lastName = lastNames[rng.Next(lastNames.Length)];
            var cityIdx = rng.Next(cities.Length);

            users.Add(new User
            {
                Id = i,
                Name = $"{firstName} {lastName}",
                Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com",
                Role = roles[rng.Next(roles.Length)],
                Department = departments[rng.Next(departments.Length)],
                Address = new Address
                {
                    Street = $"{rng.Next(100, 9999)} {new[] { "Main", "Oak", "Elm", "Park", "Cedar" }[rng.Next(5)]} St",
                    City = cities[cityIdx],
                    State = states[cityIdx],
                    ZipCode = $"{rng.Next(10000, 99999)}",
                    Country = "USA"
                },
                Phone = $"+1-{rng.Next(200, 999)}-{rng.Next(100, 999)}-{rng.Next(1000, 9999)}",
                Preferences = new Dictionary<string, string>
                {
                    ["theme"] = rng.Next(2) == 0 ? "light" : "dark",
                    ["language"] = "en",
                    ["notifications"] = rng.Next(2) == 0 ? "enabled" : "disabled",
                    ["timezone"] = new[] { "EST", "CST", "MST", "PST" }[rng.Next(4)]
                },
                LastLogin = DateTime.UtcNow.AddHours(-rng.Next(1, 720))
            });
        }

        return users;
    }

    private static List<Order> SeedOrders()
    {
        var statuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
        var shippingMethods = new[] { "Standard", "Express", "Overnight", "Economy" };
        var paymentMethods = new[] { "Credit Card", "Debit Card", "PayPal", "Bank Transfer" };

        var orders = new List<Order>();
        var rng = new Random(123);

        for (int i = 1; i <= 100; i++)
        {
            var items = Enumerable.Range(0, rng.Next(1, 5)).Select(_ =>
            {
                var productId = rng.Next(1, 51);
                var product = Products.First(p => p.Id == productId);
                return new OrderItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Quantity = rng.Next(1, 10),
                    UnitPrice = product.Price
                };
            }).ToList();

            var totalAmount = items.Sum(item => item.UnitPrice * item.Quantity);

            orders.Add(new Order
            {
                Id = i,
                UserId = rng.Next(1, 31),
                Items = items,
                Status = statuses[rng.Next(statuses.Length)],
                Shipping = new ShippingInfo
                {
                    Method = shippingMethods[rng.Next(shippingMethods.Length)],
                    TrackingNumber = $"TRK-{rng.Next(100000, 999999)}",
                    Address = new Address
                    {
                        Street = $"{rng.Next(100, 9999)} Delivery Ave",
                        City = "Anytown",
                        State = "CA",
                        ZipCode = $"{rng.Next(10000, 99999)}",
                        Country = "USA"
                    }
                },
                Billing = new BillingInfo
                {
                    PaymentMethod = paymentMethods[rng.Next(paymentMethods.Length)],
                    CardLastFour = $"{rng.Next(1000, 9999)}",
                    Address = new Address
                    {
                        Street = $"{rng.Next(100, 9999)} Billing Rd",
                        City = "Anytown",
                        State = "CA",
                        ZipCode = $"{rng.Next(10000, 99999)}",
                        Country = "USA"
                    }
                },
                TotalAmount = Math.Round(totalAmount, 2),
                CreatedAt = DateTime.UtcNow.AddDays(-rng.Next(1, 180)),
                UpdatedAt = DateTime.UtcNow.AddDays(-rng.Next(0, 30)),
                Notes = rng.Next(3) == 0 ? "Please handle with care" : string.Empty
            });
        }

        return orders;
    }
}
