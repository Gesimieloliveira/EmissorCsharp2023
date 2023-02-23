@echo off

set "version=v0.0.0"
set "nomeInstalador=SistemaCafe-full"
set "pastaFiles=%PROJETO_FUSION%\_setup\files"
set "setupFull=1"
set "whiteLabel=0"

echo "Adicionando versão nas dll"
Powershell.exe -executionpolicy remotesigned -Command %PROJETO_FUSION%\_setup\scripts\set-assembly-version.ps1 %PROJETO_FUSION% %version%

echo "Gerando arquivos White Label"
Powershell.exe -executionpolicy remotesigned -Command %PROJETO_FUSION%\_setup\scripts\prepare-wl-files.ps1 %PROJETO_FUSION%

echo "Gerando Instalador Normal"
Powershell.exe -executionpolicy remotesigned -Command %PROJETO_FUSION%\_setup\scripts\make-setup.ps1 %nomeInstalador% %PROJETO_FUSION% %version% %setupFull% %whiteLabel%

timeout 30