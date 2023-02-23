# Script Params
param(
    [string]$Uri=$(throw 'Parameter Uri is required'),
    [string]$Jwt=$(throw 'Parameter Jwt is required'),
    [string]$AppVersion=""
)

if ($AppVersion -eq "") {
    Write-Host "AppVerison not defined then script get from GIT"
    $AppVersion = git -C . tag -l --points-at HEAD
    if (!$AppVersion) {
        Write-Error -Message "AppVersion not discovered"
        exit 1
    }
}

# Script Body
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Authorization", "Bearer $Jwt")

$response = Invoke-RestMethod "$Uri/api/v1/releases" -Method 'GET' -Headers $headers
if ($response.error) {
    Write-Host $response.message
    exit 1
}

$releaseData;
foreach ($res in $response.payload.docs) {
    if ($res.version -eq $AppVersion) {
        $releaseData = $res;
        break
    }
}

if (!$releaseData) {
    $body = @{
        title = "Sistema Fusion $AppVersion"
        version = $AppVersion
        change_log = "..."
    }
    
    $releaseResponse = Invoke-RestMethod "$Uri/api/v1/releases" -Method 'POST' -Headers $headers -Body $body
    if ($releaseResponse.error) {
        Write-Host $response.message
        exit 1
    }

    $releaseData = $releaseResponse.payload
}

$currentReleaseId = $releaseData.id
$releaseJsonData = ConvertTo-Json -Compress $releaseData

Write-Host "##vso[task.setvariable variable=CurrentRelease;isSecret=false;isOutput=true;]$releaseJsonData"
Write-Host "##vso[task.setvariable variable=CurrentReleaseId;isSecret=false;isOutput=true;]$currentReleaseId"