using Azure.Identity;
using Azure.Messaging.EventGrid;
using EventGrid.Publisher;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton(sp =>
{
    var endpoint = builder.Configuration.GetConnectionString("grid") ?? throw new InvalidOperationException("EventGrid endpoint not found in configuration.");
    return new EventGridPublisherClient(new Uri(endpoint), new DefaultAzureCredential());
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
