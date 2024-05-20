using Aspire.Hosting.Azure;

public class EventGridWebHookResource(string name) : AzureBicepResource(name, templateFile: "eventgridwebhook.bicep"), IResourceWithConnectionString
{
    public BicepOutputReference Endpoint => new("endpoint", this);

    public ReferenceExpression ConnectionStringExpression =>
        ReferenceExpression.Create($"{Endpoint}");
}

internal class EventGridWebHookBootstrapResource(string name) : AzureBicepResource(name, templateFile: "eventgridwebhookbootstrap.bicep")
{
}

public static class EventGridExtensions
{
    public static IResourceBuilder<EventGridWebHookResource> AddEventGridWebHook(this IDistributedApplicationBuilder builder, string name, EndpointReference endpointReference, string webhookPath)
    {
        builder.AddAzureProvisioning();

        // This is a placeholder resource that will be used to validate the EventGrid subscription
        // event grid validates the webhook by sending a validation event to the webhook during
        // deployment.
        var eventgridBootstrapResource = new EventGridWebHookBootstrapResource($"{name}-bootstrap");
        builder.AddResource(eventgridBootstrapResource)
               .WithParameter("webhookPath", webhookPath)
               .WithParameter("name", endpointReference.Resource.Name)
               .WithParameter("containerAppEnvironmentId")
               .WithManifestPublishingCallback(eventgridBootstrapResource.WriteToManifest);

        var eventGridWebHookResource = new EventGridWebHookResource(name);

        return builder.AddResource(eventGridWebHookResource)
                      .WithParameter("topicName", name.ToLowerInvariant())
                      .WithParameter(AzureBicepResource.KnownParameters.PrincipalId)
                      .WithParameter(AzureBicepResource.KnownParameters.PrincipalType)
                      .WithParameter("webHookEndpoint", () => ReferenceExpression.Create($"{endpointReference}/{webhookPath}"))
                      .WithManifestPublishingCallback(eventGridWebHookResource.WriteToManifest);
    }
}
