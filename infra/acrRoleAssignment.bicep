

param acrName string
param principalId string
@allowed(['ACR Pull'])
param roleName string
@allowed(['Device', 'ForeignGroup', 'Group', 'ServicePrincipal', 'User'])
param principalType string = 'ServicePrincipal'


var roleIdMapping = {
  'ACR Pull': '7f951dda-4ed3-4680-a7ca-43fe172d538d'
}

resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' existing =  {
  name: acrName
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(acr.id, principalId, roleIdMapping[roleName])
  scope: acr
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roleIdMapping[roleName])
    principalId: principalId
    principalType: principalType
  }
}
