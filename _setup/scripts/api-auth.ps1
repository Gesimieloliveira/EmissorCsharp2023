# Script Params
param(
    [string]$Uri=$(throw 'Parameter Uri is required'),
    [string]$UserEmail=$(throw 'Parameter UserEmail is required'),
    [string]$UserPass=$(throw 'Parameter UserPass is required')
)

# Script body
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")

$body = "{`"email`": `"$UserEmail`",`"password`": `"$UserPass`"}"

$response = Invoke-RestMethod "$Uri/api/v1/auth" -Method 'POST' -Headers $headers -Body $body

if ($response.error) {
    Write-Host $response.message;
    exit 1;
}

$token = $response.payload.acess_token

Write-Host "##vso[task.setvariable variable=AuthToken;isSecret=false;isOutput=true;]$token"