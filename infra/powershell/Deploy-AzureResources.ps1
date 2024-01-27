param(
    [string]$deploymentName,
    [string]$resourceGroup,
    [string]$bicepParamFile,
    [string]$environment,
    [string]$planName,
    [string]$globalResourceGroupName
)

# Upgrade Bicep CLI (required)
az bicep upgrade

# Create Azure Resources
$DEPLOYMENT_OUTPUT = az deployment group create `
    -n "$deploymentName" `
    -f ".\infra\main.bicep" `
    -g "$resourceGroup" `
    -p "$bicepParamFile" `
    -p environment="$environment" `
    -p planName="$planName" `
    -p globalResourceGroupName="$globalResourceGroupName" `
    --query properties.outputs | ConvertFrom-Json

# Output Azure App Service Name
$appServiceName = $DEPLOYMENT_OUTPUT.appService.value.name
"AZURE_APP_SERVICE_NAME=$appServiceName" | Out-File -Append -FilePath $env:GITHUB_OUTPUT

# Output Azure Container Registry
$acrName = $DEPLOYMENT_OUTPUT.acr.value.loginServer
"AZURE_CONTAINER_REGISTRY_NAME=$acrName" | Out-File -Append -FilePath $env:GITHUB_OUTPUT

# Output Azure Publish Profile
$AZURE_PUBLISH_PROFILE = az webapp deployment list-publishing-profiles `
    --name $appServiceName `
    --resource-group $resourceGroup `
    --xml
Write-Output "::add-mask::$AZURE_PUBLISH_PROFILE"
"AZURE_PUBLISH_PROFILE=$AZURE_PUBLISH_PROFILE" | Out-File -Append -FilePath $env:GITHUB_OUTPUT



