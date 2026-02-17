using SharedDataApi.Data;
using Xunit;

namespace SharedDataApi.Tests;

public class DataStoreTests
{
    [Fact]
    public void Products_Has50Items()
    {
        Assert.Equal(50, DataStore.Products.Count);
    }

    [Fact]
    public void Users_Has30Items()
    {
        Assert.Equal(30, DataStore.Users.Count);
    }

    [Fact]
    public void Orders_Has100Items()
    {
        Assert.Equal(100, DataStore.Orders.Count);
    }

    [Fact]
    public void Products_HaveUniqueIds()
    {
        var ids = DataStore.Products.Select(p => p.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void Users_HaveValidEmails()
    {
        Assert.All(DataStore.Users, u => Assert.Contains("@", u.Email));
    }

    [Fact]
    public void Orders_HavePositiveTotals()
    {
        Assert.All(DataStore.Orders, o => Assert.True(o.TotalAmount > 0));
    }
}
