param location string = resourceGroup().location
param containerAppEnvironmentId string
param webhookPath string
param name string

resource webhookbootstrapApp 'Microsoft.App/containerApps@2024-03-01' = {
  name: name
  location: location
  properties: {
    environmentId: containerAppEnvironmentId
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
      }
    }
    template: {
      containers: [
        {
          name: name
          image: 'davidfowl/eventgridwebhookbootstrap'
          args: ['--webhookPath', webhookPath]
        }
      ]
    }
  }
}
