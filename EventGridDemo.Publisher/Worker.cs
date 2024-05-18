using Azure.Messaging.EventGrid;

namespace EventGrid.Publisher;

public class Worker(EventGridPublisherClient client, ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var payload = new BinaryData(new DateEvent(DateTimeOffset.UtcNow));

            logger.LogInformation("Sending event at {time}", DateTimeOffset.UtcNow);

            // Send the events
            await client.SendEventsAsync([
                new EventGridEvent("ExampleEventSubject", "Example.EventType", "1.0", payload)
                ], 
                stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }

    record DateEvent(DateTimeOffset Time);
}

