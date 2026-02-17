using AppHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sharedData = builder.AddService<SharedDataApi>("shared-data");

var contentApi = builder.AddService<ContentApi>("content-api")
                        .WaitFor(sharedData);

builder.AddNpmApp("fabric-workload-frontend", "../../fabric-workload", "dev")
       .WithHttpEndpoint(5173, isProxied: false)
       .WaitFor(contentApi);

builder.Build()
       .Run();
