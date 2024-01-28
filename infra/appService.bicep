param appServiceName string
param planId string
param planLocation string
param principalId string
param dockerImage string
param acrServer string
param environment string


resource appSvc 'Microsoft.Web/sites@2023-01-01' = {
  name: appServiceName
  location: planLocation
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${principalId}': {}
    }
  }
  properties: {
    serverFarmId: planId
    httpsOnly: true

    siteConfig: {
      linuxFxVersion: 'DOCKER|${acrServer}/${dockerImage}'
      alwaysOn: true
      http20Enabled: true
      appSettings: [
        {
          name: 'DOCKER_REGISTRY_SERVER_URL'
          value: acrServer
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: environment
        }
      ]

    }
  }
}

output name string = appSvc.name
