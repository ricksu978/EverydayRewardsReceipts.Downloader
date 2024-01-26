param planName string
param planResourceGroup string
param environment string


// App Service Plan
resource plan 'Microsoft.Web/serverfarms@2023-01-01' existing = {
  name: planName
  scope: resourceGroup(planResourceGroup)
}

module appService './appService.bicep' = {
  name: 'appService'
  params: {
    appServiceName: 'everydayrewards-receipts-downloader-${environment}'
    planId: plan.id
    planLocation: plan.location
  }
}


output appServiceName string = appService.name
// Create Web App
