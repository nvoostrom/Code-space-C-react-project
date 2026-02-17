using CommunityToolkit.Aspire.Hosting.Dapr;

namespace AppHost;

public static class DistributedApplicationBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> AddService<TProject>(
        this IDistributedApplicationBuilder builder,
        string serviceName) where TProject : IProjectMetadata, new()
    {
        return builder.AddProject<TProject>($"{serviceName}-service")
                      .WithDaprSidecar(new DaprSidecarOptions
                      {
                          AppId = serviceName,
                          PlacementHostAddress = "",
                          SchedulerHostAddress = "",
                      });
    }
}
