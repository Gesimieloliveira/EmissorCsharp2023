param(
  [string]$SetupName = $(throw 'Parameter SetupName is required'),
  [string]$Workspace = $pwd,
  [string]$AppVersion = "",
  [bool]$SetupFull = 0,
  [bool]$WhiteLabel = 0,
  [string]$AssistentFilesPath = ""
)

Set-Location $Workspace

Write-Host "- Make Setup Start With Params:"
Write-Host "Workspace: $Workspace"
Write-Host "AppVersion: $AppVersion"
Write-Host "SetupFull: $SetupFull"
Write-Host "WhiteLabel: $WhiteLabel"
Write-Host "AssistentFilesPath: $AssistentFilesPath"

Write-Host ""
Write-Host "- Discover AppVersion"

if ($AppVersion -eq "") {
  Write-Host "AppVerison not defined then script get from GIT"
  $AppVersion = git -C . tag -l --points-at HEAD
  if (!$AppVersion) {
    Write-Error -Message "AppVersion not discovered"
    exit 1
  }
}

$FileVersion = $AppVersion.Replace(".", "-")
$SetupNameOutput = $SetupName + "-" + $FileVersion
$ParamWhiteLabel = $WhiteLabel ? "WhiteLabel=1" : ""

Write-Host "AppVersion: $AppVersion"
Write-Host "FileVersion: $FileVersion"
Write-Host "SetupNameOutput: $SetupNameOutput"
Write-Host ""

if ($SetupFull -eq 0) {
  Write-Host "- Make Setup"
  .\_setup\InnoSetup\ISCC.exe ".\_setup\fusionSetup.iss" /Q /F"$SetupNameOutput" /D"Workspace=$Workspace" /D"Release" /D"AppVersion=$AppVersion" /D"$ParamWhiteLabel"
}

if ($SetupFull -eq 1) {
  Write-Host "- Make Full Setup"
  .\_setup\InnoSetup\ISCC.exe ".\_setup\fusionSetup.iss" /Q /F"$SetupNameOutput" /D"Workspace=$Workspace" /D"Release" /D"AppVersion=$AppVersion" /D"Full" /D"ThirdPath=$AssistentFilesPath" /D"$ParamWhiteLabel"
}