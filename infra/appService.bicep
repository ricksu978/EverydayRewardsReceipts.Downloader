param appServiceName string
param planId string
param planLocation string

resource appSvc 'Microsoft.Web/sites@2023-01-01' = {
  name: appServiceName
  location: planLocation

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
