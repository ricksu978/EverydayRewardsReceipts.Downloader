param appServiceName string
param planId string
param planLocation string
param principalId string

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
      linuxFxVersion: 'DOTNETCORE|8.0'
      alwaysOn: true
      http20Enabled: true

    }
  }
}

output name string = appSvc.name
