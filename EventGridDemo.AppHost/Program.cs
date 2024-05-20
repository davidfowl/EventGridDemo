var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.EventGridDemo_Api>("api")
                     .WithExternalHttpEndpoints();

var eventGrid = builder.AddEventGridWebHook("grid", api.GetEndpoint("https"), "hook");

builder.AddProject<Projects.EventGridDemo_Publisher>("publisher")
       .WithReference(eventGrid);

builder.Build().Run();
