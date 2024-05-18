## Event Grid Listener using .NET Aspire

The code in this repository shows publish and subscribe to events using event grid in .NET Aspire. The code is made up of 2 applications, [EventGridDemo.Publisher](/EventGridDemo.Publisher) and [EventGridDemo.Api](/EventGridDemo.Api). The publisher application publishes events to the event grid topic, and the API application listens for events from the event grid topic.


The  `/hook`  endpoint is used for validating the webhook. When Event Grid creates a new subscription, it sends a validation event to the webhook. The validation event contains a  validationCode  property. The application reads the  validationCode  from the first event and returns it as a response. 

The [AppHost](/EventGridDemo.AppHost/) projects creates the eventgrid topic, web hook subscription, sets up the role assignment and orchestrates the deployment of the publisher and api.

### Issues

1. **Local development** - The application is not able to receive events from the Event Grid service when running locally. The application needs to be deployed to a public endpoint to receive events from the Event Grid service. Future efforts will work around this issue by setting up dev tunnels or using ngrok to expose the local endpoint to the public.

2. **Deployment** - Event grid subscriptions require the endpoint to be live. In order to get the application deployed, there must be a 2 step deployment process. Comment out the event grid subscription creation in the AppHost project, deploy the application, then uncomment the event grid subscription creation and deploy the application again. 