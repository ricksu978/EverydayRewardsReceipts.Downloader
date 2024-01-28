param name string
param serviceUrl string
param acrName string
param dockerImage string
param location string = resourceGroup().location

resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' existing = {
  name: acrName
}

resource webhook 'Microsoft.ContainerRegistry/registries/webhooks@2023-07-01' = {
  name: name
  location: location
  parent: acr
  properties: {
    actions: ['push']
    scope: dockerImage
    serviceUri: serviceUrl
    status: 'enabled'
  }
}
