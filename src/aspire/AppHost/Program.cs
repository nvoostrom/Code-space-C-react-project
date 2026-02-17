var builder = DistributedApplication.CreateBuilder(args);

var sharedApi = builder.AddProject<Projects.SharedDataApi>("shared-api");

var fabricWorkloadApi = builder.AddProject<Projects.FabricWorkloadApi>("content-api")
    .WithReference(sharedApi);

builder.AddNpmApp("frontend", "../../fabric-workload/App", "dev")
    .WithReference(fabricWorkloadApi)
    .WithHttpEndpoint(env: "PORT");

builder.Build().Run();
