// Managed Identity

param planName string
param planResourceGroup string

// Create App Service Plan
resource plan 'Microsoft.Web/serverfarms@2023-01-01' existing = {
  name: planName
  scope: resourceGroup(planResourceGroup)
}

output planName string = plan.name
// Create Web App
