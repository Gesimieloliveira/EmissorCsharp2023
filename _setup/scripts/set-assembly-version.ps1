using namespace System.Text.RegularExpressions
using namespace System.IO

param(
  [string]$Workspace = $pwd,
  [string]$AppVersion = ""
)

Set-Location $Workspace

if ($AppVersion -eq "") {
  Write-Host "AppVerison not defined then script get from GIT"
  $AppVersion = git -C . tag -l --points-at HEAD
  if (!$AppVersion) {
    Write-Error -Message "AppVersion not discovered"
    exit 1
  }
}

$regexPattern = "^v[0-9]{1,2}\.[0-9]{1,2}\.[0-9]{1,4}$"
$regex = [Regex]::IsMatch($AppVersion, $regexPattern)

if (-not $regex) {
  Write-Host "AppVersion does not match $regexPattern"
  exit 1;
}

$assemblyVersion = $AppVersion.Replace("v", "") + ".0";

$dirs = [Directory]::GetDirectories($Workspace)

foreach ($dir in $dirs) {
  $infoFile = [Path]::Combine($dir, "Properties", "AssemblyInfo.cs");

  if ( -not [File]::Exists($infoFile)) {
    continue;
  }

  $fileContent = [File]::ReadAllText($infoFile)

  $version = [Regex]::new("assembly: AssemblyVersion.+\)", [RegexOptions]::IgnoreCase)
  $fileVersion = [Regex]::new("assembly: AssemblyFileVersion.+\)", [RegexOptions]::IgnoreCase)
  $title = [Regex]::new("assembly: AssemblyTitle.+\)", [RegexOptions]::IgnoreCase)
  $description = [Regex]::new("assembly: AssemblyDescription.+\)", [RegexOptions]::IgnoreCase)
  $configuration = [Regex]::new("assembly: AssemblyConfiguration.+\)", [RegexOptions]::IgnoreCase)
  $company = [Regex]::new("assembly: AssemblyCompany.+\)", [RegexOptions]::IgnoreCase)
  $product = [Regex]::new("assembly: AssemblyProduct.+\)", [RegexOptions]::IgnoreCase)
  $copyRight = [Regex]::new("assembly: AssemblyCopyright.+\)", [RegexOptions]::IgnoreCase)
  $tradeMark = [Regex]::new("assembly: AssemblyTrademark.+\)", [RegexOptions]::IgnoreCase)
  $culture = [Regex]::new("assembly: AssemblyCulture.+\)", [RegexOptions]::IgnoreCase)

  # replace values with regex expressions
  $fileContent = $version.Replace($fileContent, 'assembly: AssemblyVersion("' + $assemblyVersion + '")')
  $fileContent = $fileVersion.Replace($fileContent, 'assembly: AssemblyFileVersion("' + $assemblyVersion + '")')
  $fileContent = $title.Replace($fileContent, 'assembly: AssemblyTitle("Sistema Comercial")')
  $fileContent = $description.Replace($fileContent, 'assembly: AssemblyDescription("")')
  $fileContent = $configuration.Replace($fileContent, 'assembly: AssemblyConfiguration("")')
  $fileContent = $company.Replace($fileContent, 'assembly: AssemblyCompany("ROBERTO ALVES PEREIRA")')
  $fileContent = $product.Replace($fileContent, 'assembly: AssemblyProduct("Sistema Comercial' + (Get-Date).ToString('yyyy') + '")')
  $fileContent = $copyRight.Replace($fileContent, 'assembly: AssemblyCopyright("Copyright © ROBERTO ALVES PEREIRA 2016-' + (Get-Date).ToString('yyyy') + '")')
  $fileContent = $tradeMark.Replace($fileContent, 'assembly: AssemblyTrademark("SISTEMA FUSION")')
  $fileContent = $culture.Replace($fileContent, 'assembly: AssemblyCulture("")')

  [File]::WriteAllText($infoFile, $fileContent)
  Write-Host "Change AssemblyInfo: $infoFile"
}
