param(
    [string]$deploymentName,
    [string]$resourceGroup,
    [string]$bicepParamFile,
    [string]$environment,
    [string]$planName,
    [string]$globalResourceGroupName,
    [string]$acrName,
    [string]$dockerImage
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
    -p acrName="$acrName" `
    -p globalResourceGroupName="$globalResourceGroupName" `
    -p dockerImage="$dockerImage" `
    --query properties.outputs | ConvertFrom-Json

$appServiceName = $DEPLOYMENT_OUTPUT.appService.value.name
$acrLoginServer = $DEPLOYMENT_OUTPUT.acr.value.loginServer

# CI_CD_URL
$CI_CD_URL = az webapp deployment container config `
    -n "$appServiceName" `
    -g "$resourceGroup" `
    -e true `
    --query CI_CD_URL `
    --output tsv

az deployment group create `
    -n "$deploymentName-webhook" `
    -f ".\infra\acrWebhook.bicep" `
    -g "$globalResourceGroupName" `
    -p name="webhook-$appServiceName" `
    -p serviceUrl="$CI_CD_URL" `
    -p acrName="$acrName" `
    -p dockerImage="$dockerImage"

# Publish Profile
$publicProfile = az webapp deployment list-publishing-profiles `
    --name $appServiceName `
    --resource-group $resourceGroup `
    --xml
Write-Output "::add-mask::$publicProfile"

if ($null -ne $env:GITHUB_OUTPUT) {
    "AZURE_APP_SERVICE_NAME=$appServiceName" | Out-File -Append -FilePath $env:GITHUB_OUTPUT
    "AZURE_CONTAINER_REGISTRY_NAME=$acrLoginServer" | Out-File -Append -FilePath $env:GITHUB_OUTPUT
    "AZURE_PUBLISH_PROFILE=$publicProfile" | Out-File -Append -FilePath $env:GITHUB_OUTPUT
}



