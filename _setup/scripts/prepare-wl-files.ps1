using namespace System.IO

param(
  [string]$Workspace = $pwd
)

Set-Location $Workspace

Write-Host "Script Worskpace: $Workspace"

try {
  $dirs = [Directory]::GetDirectories("$Workspace\Fusion.WhiteLabel\Marcas")
  $iniBuilder = New-Object System.Text.StringBuilder

  foreach ($dir in $dirs) {
    $fileName = [Path]::GetFileName($dir)

    if (-not $fileName.StartsWith("_")) {
      continue
    }

    $jsonConfig = [Path]::Combine($dir, "marca.json")
    if (-not [File]::Exists($jsonConfig)) {
      Write-Host "Arquivo configuração white label $jsonConfig não encontrado"
      exit 1
    }

    Write-Host "Arquivo encontrado: $jsonConfig"

    $cnpj = $fileName.TrimStart("_")
    $jsonContent = [File]::ReadAllText($jsonConfig) | ConvertFrom-Json

    Write-Host "Arquivo json: $jsonContent"

    $outupt = $iniBuilder.AppendLine("[$cnpj]")
    $outupt = $iniBuilder.AppendLine("NomeAtalhoAdm=" + $jsonContent.NomeAtalhoAdm)
    $outupt = $iniBuilder.AppendLine("NomeAtalhoNfce=" + $jsonContent.NomeAtalhoNfce)
    $outupt = $iniBuilder.AppendLine("NomeAtalhoPdv=" + $jsonContent.NomeAtalhoPdv)

    Write-Host "outupt: $outupt"

    [File]::Copy([Path]::Combine($dir, $jsonContent.IconeAtalhoAdm), "$Workspace\_setup\wl\icons\$cnpj-adm.ico", 1)
    [File]::Copy([Path]::Combine($dir, $jsonContent.IconeAtalhoPdv), "$Workspace\_setup\wl\icons\$cnpj-pdv.ico", 1)
    [File]::Copy([Path]::Combine($dir, $jsonContent.IconeAtalhoNfce), "$Workspace\_setup\wl\icons\$cnpj-nfce.ico", 1)
    [File]::WriteAllText("$Workspace\_setup\wl\profiles\$cnpj.profile", $cnpj)
  }

  [File]::WriteAllText("$Workspace\_setup\wl\wl-config.ini", $iniBuilder.ToString())
}
catch {
  Write-Error -Message "Ocorreu um erro ao preparar os arquivos white label"
  Write-Error -Message $_
  exit 1
}