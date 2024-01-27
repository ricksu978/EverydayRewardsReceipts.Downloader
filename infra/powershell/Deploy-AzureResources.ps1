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

$appServiceName = $DEPLOYMENT_OUTPUT.appService.value.name
$acrName = $DEPLOYMENT_OUTPUT.acr.value.loginServer

$publicProfile = az webapp deployment list-publishing-profiles `
    --name $appServiceName `
    --resource-group $resourceGroup `
    --xml
Write-Output "::add-mask::$publicProfile"

if ($null -ne $env:GITHUB_OUTPUT) {
    "AZURE_APP_SERVICE_NAME=$appServiceName" | Out-File -Append -FilePath $env:GITHUB_OUTPUT
    "AZURE_CONTAINER_REGISTRY_NAME=$acrName" | Out-File -Append -FilePath $env:GITHUB_OUTPUT
    "AZURE_PUBLISH_PROFILE=$publicProfile" | Out-File -Append -FilePath $env:GITHUB_OUTPUT
}



