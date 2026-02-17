namespace FabricWorkloadApi.Models;

public class DashboardData
{
    public int ProductCount { get; set; }
    public int UserCount { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingOrders { get; set; }
    public int DeliveredOrders { get; set; }
    public int CancelledOrders { get; set; }
}
