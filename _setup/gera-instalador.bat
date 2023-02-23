@echo off
setlocal

if not exist "%INNO_SETUP%" (
	echo Variavel de ambiente INNO_SETUP nao definida para ICCS.exe
	goto end
)

if not exist "%GIT_CMD%" (
	echo Variavel de ambiente GIT_CMD nao definida para git.exe
	goto end
)

if not exist "%MSBUILD_CMD%" (
	echo Variavel de ambiente MSBUILD_CMD nao definida ou invalida
	goto end
)

if not exist "%PROJETO_FUSION%\Fusion.sln" (
	echo Não foi localizado o arquivo Fusion.sln em "%PROJETO_FUSION%"
	goto end
)

echo -
echo MSBUILD_CMD: "%MSBUILD_CMD%"
echo INNO_SETUP: "%INNO_SETUP%"
echo GIT_CMD: "%GIT_CMD%"
echo PROJETO_FUSION: "%PROJETO_FUSION%"

set "setup=0"
set "setupFull=0"
set "withSetup=0"
set "setupName="
set "buildMode=Debug"
set "whiteLabelFile="
set "gitVersion=NO-VERSION"
set "setupFolder="
set "nuget_exe="
set "fusion_sln="
set "whitelabel_csproj="
set "whitelabel_exe="
set "dotNetInstaller="
set "sqlServerInstaller="
set "ignoreWhiteLabel=0"
set "versaoAssembly_prj="
set "versaoAssembly_exe="
set "ignoreVersion=0"

:parse
	if "%~1"=="" goto :configure

	if /i "%~1"=="--setup" set "setup=1" & shift & goto :parse
	if /i "%~1"=="--setupFull" set "setupFull=1" & shift & goto :parse
	if /i "%~1"=="--release" set "buildMode=Release" & shift & goto :parse
	if /i "%~1"=="--ignore-version" set "ignoreVersion=1" & shift & goto :parse
	if /i "%~1"=="--wl-ignore" set "ignoreWhiteLabel=1" & shift & goto :parse

	if /i "%~1"=="--wl-file" set "whiteLabelFile=%~2" & shift & shift & goto :parse
	if /i "%~1"=="--net-installer" set "dotNetInstaller=%~2" & shift & shift & goto :parse
	if /i "%~1"=="--sql-installer" set "sqlServerInstaller=%~2" & shift & shift & goto :parse
	if /i "%~1"=="--setup-name" set "setupName=%~2" & shift & shift & goto :parse

	shift
	goto parse

:configure
	if "%setup%"=="1" set "withSetup=1"
	if "%setupFull%"=="1" set "withSetup=1"

	if "%withSetup%"=="1" if "[%setupName%]"=="[]" (
		echo Necessario informar o nome do setup ex: --setup-name ?
		goto end
	)

	if not exist "%whiteLabelFile%" if "%ignoreWhiteLabel%"=="0" (
		echo Arquivo white label nao definido ou nao localizado em: %whiteLabelFile%
		goto end
	)

	set "nuget_exe=%PROJETO_FUSION%\.nuget\NuGet.exe"
	set "fusion_sln=%PROJETO_FUSION%\Fusion.sln"
	set "setupFolder=%PROJETO_FUSION%\_setup"
	set "outputDir=%setupFolder%\Output"

	set "whitelabel_csproj=%PROJETO_FUSION%\_devs\Dev.WhiteLabel\Dev.WhiteLabel.csproj"
	set "whitelabel_exe=%PROJETO_FUSION%\_devs\Dev.WhiteLabel\bin\Build\Dev.WhiteLabel.exe"

	set "versaoAssembly_prj=%PROJETO_FUSION%\_devs\Dev.VersaoAssemblies\Dev.VersaoAssemblies.csproj"
	set "versaoAssembly_exe=%PROJETO_FUSION%\_devs\Dev.VersaoAssemblies\bin\Build\Dev.VersaoAssemblies.exe"

	if not exist "%nuget_exe%" (
		echo Nao foi possivel localizaro o nuget.exe em "%PROJETO_FUSION%"
		goto end
	)

	if not exist "%fusion_sln%" (
		echo Nao encontrei o arquivo de projeto no workspace: "%PROJETO_FUSION%"
		goto end
	)

	if not exist "%outputDir%" (
		mkdir "%outputDir%"
	)

	if "%ignoreVersion%"=="0" (
		"%GIT_CMD%" -C "%PROJETO_FUSION%" tag -l --points-at HEAD > "%outputDir%"\git-version.tmp
		set /P gitVersion=<"%outputDir%\git-version.tmp"
	)

	echo -
	echo WhiteLabel File em: %whiteLabelFile%
	echo Diretorio saida: %outputDir%
	echo Versao GIT: %gitVersion%
	echo Build mode is: %buildMode%

:build
	echo -
	echo Building...

	"%nuget_exe%" restore "%fusion_sln%

	if "%buildMode%"=="Release" if "%ignoreVersion%"=="0" (
		echo -
		echo Building versao-assemblies
		"%MSBUILD_CMD%" "%versaoAssembly_prj%" /t:build /p:OutDir=bin\Build /p:RestorePackages=false /p:PlatformTarget=anycpu /p:Configuration="%buildMode%" /maxcpucount
	
		echo Executando VersaoAssemblies
		"%versaoAssembly_exe%" /build="%gitVersion%" || goto :end
	)

	if "%ignoreWhiteLabel%"=="0" (
		echo -
		echo Building WhiteLabel...
		"%MSBUILD_CMD%" "%whitelabel_csproj%" /t:build /p:OutDir=bin\Build /p:RestorePackages=false /p:PlatformTarget=anycpu /p:Configuration="%buildMode%" /maxcpucount

		echo Executando WhiteLabel...
		"%whitelabel_exe%" --fusion-path="%PROJETO_FUSION%" --white-label-cfg="%whiteLabelFile%" --skip-confirm
	)
	
	echo -
	echo Building Fusion...
	"%MSBUILD_CMD%" "%fusion_sln%" /t:build /p:OutDir=bin\Build /p:RestorePackages=false /p:PlatformTarget=anycpu /p:Configuration="%buildMode%" /maxcpucount

:make-setup
	if "%setup%"=="1" (
		echo Gerando Instalador
		"%INNO_SETUP%" "%setupFolder%\fusionSetup.iss" /Q /D"Workspace=%PROJETO_FUSION%" /D"%buildMode%" /F"%setupName%" /D"AppVersion=%gitVersion%"
	)

	if "%setupFull%"=="1" (
		echo Gerando Instalador-full
		"%INNO_SETUP%" "%setupFolder%\fusionSetup.iss" /Q /D"Workspace=%PROJETO_FUSION%" /D"%buildMode%" /F"%setupName%-full" /D"AppVersion=%gitVersion%" /D"Full"
	)

:end
	call :cleanup
	exit /B

:cleanup
	set "setup=0"
	set "setupFull=0"
	set "withSetup=0"
	set "setupName="
	set "buildMode=Debug"
	set "whiteLabelFile="
	set "gitVersion="
	set "setupFolder="
	set "nuget_exe="
	set "fusion_sln="
	set "whitelabel_csproj="
	set "whitelabel_exe="
	set "dotNetInstaller="
	set "sqlServerInstaller="
	set "ignoreWhiteLabel=0"
	set "versaoAssembly_prj="
	set "versaoAssembly_exe="
	set "ignoreVersion=0"

	goto :eof