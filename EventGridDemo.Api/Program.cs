using System.Text.Json.Nodes;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

app.UseHttpLogging();

app.MapDefaultEndpoints();

app.MapPost("/hook", (JsonArray gridEvents) =>
{
    if (gridEvents.Count == 0)
    {
        return Results.NoContent();
    }

    // Webhook validation
    // https://learn.microsoft.com/en-us/azure/event-grid/webhook-event-delivery#validation-details
    var firstEvent = gridEvents[0];
    var validationCode = firstEvent?["data"]?["validationCode"];

    return Results.Ok(new { validationResponse = validationCode });
});

app.Run();
