param planName string
param globalResourceGroupName string
param environment string
param dockerImage string
param location string = resourceGroup().location

var globalResourceGroup = resourceGroup(globalResourceGroupName)

// App Service Plan
resource plan 'Microsoft.Web/serverfarms@2023-01-01' existing = {
  name: planName
  scope: globalResourceGroup
}

// Azure Container Registry
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' existing = {
  name: 'ricksu'
  scope: globalResourceGroup
}

// Managed Identity
resource id 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'id-errd-${environment}'
  location: location
}

module acrra './acrRoleAssignment.bicep' = {
  name: 'acr-ra'
  scope: globalResourceGroup
  params:{
    acrName: acr.name
    principalId: id.properties.principalId
    roleName: 'ACR Pull'
  }
}

module appService './appService.bicep' = {
  name: 'appService'
  params: {
    appServiceName: 'appsvc-errd-${environment}'
    planId: plan.id
    planLocation: plan.location
    principalId: id.id
    dockerImage: dockerImage
    acrServer: acr.properties.loginServer
    environment: environment
  }
}

output appService object = {
  name: appService.outputs.name
}

output acr object = {
  name: acr.name
  loginServer: acr.properties.loginServer
}
