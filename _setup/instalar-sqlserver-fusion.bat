@echo off
setlocal

echo ## AUXILIAR DE INSTALACAO SQL SERVER FUSION ##
echo ## SQL Server 2008 ##
echo --
echo Tentarei instalar o SQL Server utilizando as configuracoes recomendadas. 
echo Preciso que acompanhe e confirme as janelas

echo --

:install
echo Iniciando instalacao do SQL Server
.\SQLSetup2008_R2.exe /UIMODE=Normal /IACCEPTSQLSERVERLICENSETERMS /HIDECONSOLE /ACTION=Install /INSTANCENAME=FUSION /SAPWD=Fusion@ag4 /ASCOLLATION=Latin1_General_CI_AI /SQLCOLLATION=Latin1_General_CI_AI /SECURITYMODE=SQL /TCPENABLED=1 /BROWSERSVCSTARTUPTYPE=Automatic /FEATURES=SQLEngine

:end