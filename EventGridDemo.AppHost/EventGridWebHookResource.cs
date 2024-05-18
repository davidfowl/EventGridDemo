using Aspire.Hosting.Azure;

public class EventGridWebHookResource(string name) : AzureBicepResource(name, templateFile: "eventgridwebhook.bicep"), IResourceWithConnectionString
{
    public BicepOutputReference Endpoint => new("endpoint", this);

    public ReferenceExpression ConnectionStringExpression => 
        ReferenceExpression.Create($"{Endpoint}");
}

public static class EventGridExtensions
{
    public static IResourceBuilder<EventGridWebHookResource> AddEventGridWebHook(this IDistributedApplicationBuilder builder, string name)
    {
        builder.AddAzureProvisioning();

        var resource = new EventGridWebHookResource(name);
        return builder.AddResource(resource)
                      .WithParameter("topicName", name.ToLowerInvariant())
                      .WithParameter(AzureBicepResource.KnownParameters.PrincipalId)
                      .WithParameter(AzureBicepResource.KnownParameters.PrincipalType)
                      .WithManifestPublishingCallback(resource.WriteToManifest);
    }

    public static IResourceBuilder<EventGridWebHookResource> WithWebhookUrl(this IResourceBuilder<EventGridWebHookResource> builder, ReferenceExpression webhookExpression)
    {
        return builder.WithParameter("webHookEndpoint", () => webhookExpression);
    }

    public static IResourceBuilder<EventGridWebHookResource> WithWebhookUrl(this IResourceBuilder<EventGridWebHookResource> builder, ReferenceExpression.ExpressionInterpolatedStringHandler webhookExpression)
    {
        return builder.WithWebhookUrl(ReferenceExpression.Create(webhookExpression));
    }
}
