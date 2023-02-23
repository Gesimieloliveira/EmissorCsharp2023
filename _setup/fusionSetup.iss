; SETUP
; Propriedades do instalador
; Customizadas
#include "fusionSetup.fusion.iss"
#ifdef WhiteLabel
  #include "fusionSetup.wl.iss"
#endif
; Fixas
#define MyAppPublisher "ROBERTO ALVES PEREIRA"
#define MyDefaultDirName "c:\SistemaFusion"
#define NomeExePdv "FusionPdv.exe"
#define NomeExeNfce "FusionNfce.exe"
#define NomeExeAdm "Fusion.exe"
#define NomeExeBackgroudApp "Fusion.Background.App.exe"
#define DotNetSetupName = "NetFramework462.exe"
#define SQLSetupName = "SQLSetup2008_R2.exe"

#ifdef Release
  #define BuildPath "build"
#endif

#ifndef Release
  // #define Full "" ;Ativar para simular instalador full
  // #define ThirdPath "C:\ProgramData\FusionSetup\3rd" ;Definir pasta a uxiliar para executaveis
  #define AppVersion "v0.0.0"
  #define BuildPath "build"
  #define Workspace GetEnv("PROJETO_FUSION")
#endif

; define fusion path
#define FusionCorePath Workspace + "\FusionCore\" + BuildPath
#define FusionPath Workspace + "\Fusion\" + BuildPath
#define FusionBackgroundPath Workspace + "\Fusion.Background.App\" + BuildPath
#define FusionPdvSyncPath Workspace + "\PdvSincronizador\" + BuildPath
#define FusionPdvPath Workspace + "\FusionPdv\" + BuildPath
#define FusionNfceSyncPath Workspace + "\FusionNfceSincronizador\" + BuildPath
#define FusionNfcePath Workspace + "\FusionNfce\" + BuildPath
#define FusionSpedPath Workspace + "\Sped\" + BuildPath
#define ConversorPath Workspace + "\Fusion.Conversor\" + BuildPath
#define ControladorServicos Workspace + "\Fusion.ControladorServicos\" + BuildPath

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{73AC6506-89BA-44EA-BEBB-A7500480E5EC}
SetupMutex=Global\1755C08A-D68F-47D0-AF46-D8B45C1932C5
SignedUninstaller=no
AppName={#MyAppName}
AppVersion={#AppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={#MyDefaultDirName}
DefaultGroupName={#MyAppName}
OutputBaseFilename=SetupSistema
PrivilegesRequired=poweruser
LicenseFile=files\license.txt
WizardSmallImageFile={#WizSmallImage}
WizardImageFile={#WizLeftImage}
SetupIconFile={#SetupIconFile}
AppCopyright=Copyright (C) 2015-2021 Agil4 Tecnologia LTDA - ME
DisableDirPage=yes
DisableProgramGroupPage=yes
DisableWelcomePage=no
DisableReadyPage=no
CompressionThreads=auto
LZMANumBlockThreads=4
LZMAUseSeparateProcess=yes
LZMABlockSize=262144

#ifdef Release
Compression=lzma2/ultra64
InternalCompressLevel=ultra64
SolidCompression=yes
#else
Compression=none
InternalCompressLevel=none
#endif

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Dirs]
Name: "{app}"; Permissions: everyone-full
Name: "{app}\.restore"; Permissions: everyone-full
Name: "{app}\Backup"; Permissions: everyone-full
Name: "{app}\BancoDados"; Permissions: everyone-full
Name: "{app}\Utils"; Permissions: everyone-full

[Files]
#ifdef Full
  Source: "{#ThirdPath}\{#DotNetSetupName}"; DestName: "{#DotNetSetupName}"; DestDir: "{app}\.installers"; Flags: ignoreversion noencryption nocompression;
  Source: "{#ThirdPath}\{#SQLSetupName}"; DestName: "{#SQLSetupName}"; DestDir: "{app}\.installers"; Flags: ignoreversion noencryption nocompression;
  Source: "instalar-sqlserver-fusion.bat"; DestDir: "{app}\.installers"; Flags: ignoreversion noencryption nocompression;
#endif

;;;;;;;;;;;;;; WHITE LABEL
Source: "{#Workspace}\_setup\wl\wl-config.ini"; DestDir: "{tmp}"; Flags: dontcopy;
Source: "{#Workspace}\_setup\wl\icons\*.ico"; DestDir: "{tmp}"; Flags: dontcopy;
Source: "{#Workspace}\_setup\wl\profiles\*.profile"; DestDir: "{tmp}"; Flags: dontcopy;
;;;;;;;;;;;;;; WHITE LABEL

;FONTS
Source: "files\Fontes\OpenSans-CondBold.ttf"; DestDir: "{fonts}"; FontInstall: "OpenSans-CondBold"; Flags: onlyifdoesntexist uninsneveruninstall
Source: "files\Fontes\OpenSans-CondLight.ttf"; DestDir: "{fonts}"; FontInstall: "OpenSans-CondLight"; Flags: onlyifdoesntexist uninsneveruninstall
Source: "files\Fontes\OpenSans-CondLightItalic.ttf"; DestDir: "{fonts}"; FontInstall: "OpenSans-CondLightItalic"; Flags: onlyifdoesntexist uninsneveruninstall
Source: "files\Fontes\UbuntuCondensed-Regular.ttf"; DestDir: "{fonts}"; FontInstall: "UbuntuCondensed-Regular"; Flags: onlyifdoesntexist uninsneveruninstall

;BANCOS DE DADOS
Source: "files\FusionAdm.bak"; DestDir: "{app}\.restore"; Flags: ignoreversion; Check: DbServidorCheck;
Source: "files\FusionRelatorio.bak"; DestDir: "{app}\.restore"; Flags: ignoreversion; Check: DbRelatorioCheck;
Source: "files\FusionPdv.bak"; DestDir: "{app}\.restore"; Flags: ignoreversion; Check: DbFusionPdvCheck;
Source: "files\FusionNfce.bak"; DestDir: "{app}\.restore"; Flags: ignoreversion; Check: DbFusionNfceCheck;

;ACBR MONITOR
Source: "files\AcbrMonitor\*.dll"; DestDir: "{app}\FusionAcbr"; Flags: ignoreversion replacesameversion; Check: InstallAcbrCheck
Source: "files\AcbrMonitor\*.ini"; DestDir: "{app}\FusionAcbr"; Flags: onlyifdoesntexist; Check: InstallAcbrCheck
Source: "files\AcbrMonitor\ACBrMonitor.exe"; DestDir: "{app}\FusionAcbr"; Flags: ignoreversion replacesameversion; Check: InstallAcbrCheck

;;; FUSION UTILS
Source: "files\TaskRecuperadorServicos.xml"; DestDir: "{app}\Utils"; Flags: ignoreversion replacesameversion;
Source: "{#ControladorServicos}\FSChecador.exe"; DestDir: "{app}\Utils"; Flags: ignoreversion replacesameversion;

;FUSION ADMINISTRADOR
;;; FUSION CORE
Source: "{#FusionCorePath}\*.dll"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallServidorCheck
Source: "{#FusionCorePath}\x86\*"; DestDir: "{app}\Fusion\x86"; Flags: ignoreversion replacesameversion; Check: InstallServidorCheck
;;; FUSION
Source: "{#FusionCorePath}\*.dll"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionCorePath}\x86\*"; DestDir: "{app}\Fusion\x86"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Assets\Schemas.Nfe\*"; DestDir: "{app}\Fusion\Assets\Schemas.Nfe"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Assets\Schemas.Cte\*"; DestDir: "{app}\Fusion\Assets\Schemas.Cte"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Assets\Schemas.Mdfe\*"; DestDir: "{app}\Fusion\Assets\Schemas.Mdfe"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Assets\Schemas.CteOs\*"; DestDir: "{app}\Fusion\Assets\Schemas.CteOs"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\NFe\*.frx"; DestDir: "{app}\Fusion\NFe"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\pt-BR\*"; DestDir: "{app}\Fusion\pt-BR"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallAdmCheck
Source: "{#FusionPath}\x86\*"; DestDir: "{app}\Fusion\x86"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallAdmCheck
Source: "{#FusionPath}\*.dll"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\*.xml"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\*.config"; Excludes: "*.vshost.exe.config"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Report\Fusion\*.dll"; DestDir: "{app}\Fusion\Report\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Report\Fusion\*.lic"; DestDir: "{app}\Fusion\Report\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Report\Fusion\*.ini"; DestDir: "{app}\Fusion\Report\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Report\Fusion\taskreport.exe"; DestDir: "{app}\Fusion\Report\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\Report\Fusion\TaskReportFusion.exe"; DestDir: "{app}\Fusion\Report\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionPath}\{#NomeExeAdm}"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionBackgroundPath}\{#NomeExeBackgroudApp}"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallServidorCheck
Source: "{#FusionBackgroundPath}\icon.ico"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallServidorCheck
Source: "files\dlls-extras\Microsoft.SqlServer.SqlEnum.dll"; DestDir: "{app}\Fusion"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck

;FUSION CONVERSOR
;;; FUSION CORE
Source: "{#FusionCorePath}\*.dll"; DestDir: "{app}\Conversor"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#FusionCorePath}\x86\*"; DestDir: "{app}\Conversor\x86"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
;;; CONVERSOR
Source: "{#ConversorPath}\*.dll"; DestDir: "{app}\Conversor"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#ConversorPath}\*.xml"; DestDir: "{app}\Conversor"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#ConversorPath}\pt-BR\*"; DestDir: "{app}\Conversor\pt-BR"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallAdmCheck
Source: "{#ConversorPath}\x86\*"; DestDir: "{app}\Conversor\x86"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallAdmCheck
Source: "{#ConversorPath}\*.config"; Excludes: "*.vshost.exe.config"; DestDir: "{app}\Conversor"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "{#ConversorPath}\Conversor.exe"; DestDir: "{app}\Conversor"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck
Source: "files\FusionAdm.bak"; DestDir: "{app}\Conversor\Resources"; Flags: ignoreversion replacesameversion; Check: InstallAdmCheck

;FUSION PDV
;;; PDV SYNC
Source: "{#FusionPdvSyncPath}\*.dll"; DestDir: "{app}\FusionPdv"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck
Source: "{#FusionPdvSyncPath}\*.xml"; DestDir: "{app}\FusionPdv"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck
Source: "{#FusionPdvSyncPath}\*.config"; DestDir: "{app}\FusionPdv"; Excludes: "*.vshost.exe.config"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck
Source: "{#FusionPdvSyncPath}\FusionSincronizador.exe"; DestDir: "{app}\FusionPdv"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck
;;; PDV
Source: "{#FusionPdvPath}\x86\*.dll"; DestDir: "{app}\FusionPdv\x86"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallPdvCheck
Source: "{#FusionPdvPath}\pt-BR\*"; DestDir: "{app}\FusionPdv\pt-BR"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallPdvCheck
Source: "{#FusionPdvPath}\*.dll"; DestDir: "{app}\FusionPdv"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck
Source: "{#FusionPdvPath}\*.xml"; DestDir: "{app}\FusionPdv"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck
Source: "{#FusionPdvPath}\*.config"; DestDir: "{app}\FusionPdv"; Excludes: "*.vshost.exe.config"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck
Source: "{#FusionPdvPath}\{#NomeExePdv}"; DestDir: "{app}\FusionPdv"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck
;;; PDV EXTRAS
Source: "files\dlls-extras\Microsoft.SqlServer.SqlEnum.dll"; DestDir: "{app}\FusionPdv"; Flags: ignoreversion replacesameversion; Check: InstallPdvCheck

;FUSION NFC-E
;;; FUSION CORE
Source: "{#FusionCorePath}\*.dll"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "{#FusionCorePath}\x86\*"; DestDir: "{app}\FusionNfce\x86"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
;;; FUSION SYNC NFC-E
Source: "{#FusionNfceSyncPath}\*.dll"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "{#FusionNfceSyncPath}\*.xml"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "{#FusionNfceSyncPath}\*.config"; DestDir: "{app}\FusionNfce"; Excludes: "*.vshost.exe.config"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "{#FusionNfceSyncPath}\FusionNfceSincronizador.exe"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
;;; NFC-E
Source: "{#FusionNfcePath}\Assets\Schemas.Nfe\*"; DestDir: "{app}\FusionNfce\Assets\Schemas.Nfe"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "{#FusionNfcePath}\pt-BR\*"; DestDir: "{app}\FusionNfce\pt-BR"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallNfceCheck
Source: "{#FusionNfcePath}\SATdll\*.dll"; DestDir: "{app}\FusionNfce\SATdll"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallNfceCheck
Source: "{#FusionNfcePath}\x86\*.dll"; DestDir: "{app}\FusionNfce\x86"; Flags: ignoreversion replacesameversion recursesubdirs; Check: InstallNfceCheck
Source: "{#FusionNfcePath}\*.dll"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "{#FusionNfcePath}\*.xml"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "{#FusionNfcePath}\*.config"; Excludes: "*.vshost.exe.config"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "{#FusionNfcePath}\{#NomeExeNfce}"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
;;; NFC-E EXTRAS
Source: "files\dlls-extras\Microsoft.SqlServer.SqlEnum.dll"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck
Source: "files\dlls-extras\zlib.dll"; DestDir: "{app}\FusionNfce"; Flags: ignoreversion replacesameversion; Check: InstallNfceCheck

[Registry]
; AcbrMonitor startup
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "FusionAcbr"; ValueData: "{app}\FusionAcbr\ACBrMonitor.exe"; Check: InstallAcbrCheck; Flags: uninsdeletekey

; Fusion App Serviços Startup
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "FusionBackgroundApp"; ValueData: "{app}\Fusion\{#NomeExeBackgroudApp}"; Check: InstallServidorCheck; Flags: uninsdeletekey

; Fusion
Root: HKLM; Subkey: "Software\Agil4\SistemaFusion"
Root: HKLM; Subkey: "Software\Agil4\SistemaFusion\Setup"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Agil4\SistemaFusion\Setup"; ValueType: string; ValueName: "Versao"; ValueData: "{#AppVersion}"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Agil4\SistemaFusion\Setup"; ValueType: string; ValueName: "IsServidor"; ValueData: "{code:RegIsServidorValue}"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Agil4\SistemaFusion\Setup"; ValueType: string; ValueName: "IsAdm"; ValueData: "{code:RegIsAdmValue}"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Agil4\SistemaFusion\Setup"; ValueType: string; ValueName: "IsPdv"; ValueData: "{code:RegIsPdvValue}"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Agil4\SistemaFusion\Setup"; ValueType: string; ValueName: "IsNfce"; ValueData: "{code:RegIsNfceValue}"; Flags: uninsdeletekey

[Run]
; install services
Filename: {sys}\sc.exe; Parameters: "create FusionNfceSincronizador start= auto DisplayName= ""Fusion NFC-E Sincronizador"" binPath= ""{app}\FusionNfce\FusionNfceSincronizador.exe"""; Flags: runhidden; Check:InstallNfceCheck
Filename: {sys}\sc.exe; Parameters: "create FusionPdvSincronizador start= auto DisplayName= ""Fusion PDV Sincronizador"" binPath= ""{app}\FusionPdv\FusionSincronizador.exe"""; Flags: runhidden; Check:InstallPdvCheck

Filename: {sys}\sc.exe; Parameters: "failure FusionNfceSincronizador reset= 0 actions= restart/60000"; Flags: runhidden; Check:InstallNfceCheck
Filename: {sys}\sc.exe; Parameters: "failure FusionPdvSincronizador reset= 0 actions= restart/60000"; Flags: runhidden; Check:InstallPdvCheck

Filename: {sys}\sc.exe; Parameters: "start FusionNfceSincronizador"; Flags: runhidden; Check:InstallNfceCheck
Filename: {sys}\sc.exe; Parameters: "start FusionPdvSincronizador"; Flags: runhidden; Check:InstallPdvCheck

; create tasks - manter o delete das antigas
;; deleta existentes antes
Filename: {sys}\schtasks.exe; Parameters: "/Delete /F /TN SistemaFusion\FusionServidor"; Flags: runhidden;
Filename: {sys}\schtasks.exe; Parameters: "/Delete /F /TN SistemaFusion\FusionNFCE"; Flags: runhidden;
Filename: {sys}\schtasks.exe; Parameters: "/Delete /F /TN SistemaFusion\FusionPDV"; Flags: runhidden;
Filename: {sys}\schtasks.exe; Parameters: "/Delete /F /TN SistemaFusion\RecuperadorServicos"; Flags: runhidden;
;; criar novas
Filename: {sys}\schtasks.exe; Parameters: "/Create /F /RU SYSTEM /TN SistemaFusion\RecuperadorServicos /XML {app}/Utils/TaskRecuperadorServicos.xml"; Flags: runhidden;

; firewall
Filename: {sys}\netsh; Parameters: "advfirewall firewall delete rule name=""SQL Server (FUSION)"" dir=in"; Check:InstallServidorCheck; Flags: runhidden;
Filename: {sys}\netsh; Parameters: "advfirewall firewall add rule name=""SQL Server (FUSION)"" dir=in action=allow protocol=tcp localport=1433"; Check:InstallServidorCheck; Flags: runhidden;
Filename: {sys}\netsh; Parameters: "advfirewall firewall add rule name=""Fusion API"" dir=in action=allow protocol=tcp localport=8561"; Check:InstallServidorCheck; Flags: runhidden;

; iniciar acbr monitor
Filename: "{app}\FusionAcbr\ACBrMonitor.exe"; Check:InstallAcbrCheck; Flags: nowait runhidden;

; iniciar FusionBackroundApp
Filename: "{app}\Fusion\{#NomeExeBackgroudApp}"; Check:InstallServidorCheck; Flags: nowait runhidden;

[UninstallRun]
; Existe codigo no evento InitializeUninstallProgressForm()
Filename: {sys}\sc.exe; Parameters: "stop FusionNfceSincronizador"; Flags: runhidden; Check:InstallNfceCheck
Filename: {sys}\sc.exe; Parameters: "stop FusionPdvSincronizador"; Flags: runhidden; Check:InstallPdvCheck

Filename: {sys}\sc.exe; Parameters: "delete FusionNfceSincronizador"; Flags: runhidden; Check:InstallNfceCheck
Filename: {sys}\sc.exe; Parameters: "delete FusionPdvSincronizador"; Flags: runhidden; Check:InstallPdvCheck

Filename: {sys}\netsh; Parameters: "advfirewall firewall delete rule name= ""SQL Server (FUSION)"" dir= in"; Check:InstallServidorCheck; Flags: runhidden

; Deletar agendamentos e tarefas - manter o delete das antigas
Filename: {sys}\schtasks.exe; Parameters: "/Delete /F /TN SistemaFusion\RecuperadorServicos"; Flags: runhidden;
Filename: {sys}\schtasks.exe; Parameters: "/Delete /F /TN SistemaFusion\FusionServidor"; Flags: runhidden;
Filename: {sys}\schtasks.exe; Parameters: "/Delete /F /TN SistemaFusion\FusionNFCE"; Flags: runhidden;
Filename: {sys}\schtasks.exe; Parameters: "/Delete /F /TN SistemaFusion\FusionPDV"; Flags: runhidden;

[Icons]
Name: "{commondesktop}\{code:NomeAtalhoAdm}"; Filename: "{app}\Fusion\{#NomeExeAdm}"; IconFilename: "{app}\adm.ico"; Check: InstallAdmCheck
Name: "{commondesktop}\{code:NomeAtalhoPdv}"; Filename: "{app}\FusionPdv\{#NomeExePdv}"; IconFilename: "{app}\pdv.ico"; Check: InstallPdvCheck
Name: "{commondesktop}\{code:NomeAtalhoNfce}"; Filename: "{app}\FusionNfce\{#NomeExeNfce}"; IconFilename: "{app}\nfce.ico"; Check: InstallNfceCheck
Name: "{group}\{code:NomeAtalhoAdm}"; Filename: "{app}\Fusion\{#NomeExeAdm}"; IconFilename: "{app}\adm.ico"; Check: InstallAdmCheck
Name: "{group}\{code:NomeAtalhoAdm}"; Filename: "{app}\Fusion\{#NomeExeAdm}"; IconFilename: "{app}\adm.ico"; Check: InstallAdmCheck
Name: "{group}\{code:NomeAtalhoPdv}"; Filename: "{app}\FusionPdv\{#NomeExePdv}"; IconFilename: "{app}\pdv.ico"; Check: InstallPdvCheck
Name: "{group}\{code:NomeAtalhoNfce}"; Filename: "{app}\FusionNfce\{#NomeExeNfce}"; IconFilename: "{app}\nfce.ico"; Check: InstallNfceCheck
; Uninstall icon
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}";

[code]
const
  REG_ROOT_KEY = HKEY_LOCAL_MACHINE;
  REG_SUB_KEY = 'Software\Agil4\SistemaFusion\Setup';  
  REG_SUB_KEY_IS_UPDATE = 'Software\Agil4\SistemaFusion\Setup';
  REG_INSTANCE_KEY = 'Software\Agil4\SistemaFusion\Setup\SQL';

var
  CheckListFusion, CheckListPdv, CheckListNfce, CheckListServidor : TNewCheckListBox;
  LastTop : Integer;
  IsServidor, IsAdm, IsPdv, IsNfce, IsUpdate : Boolean;
  Page1, Page1_1, Page2, Page3 : TWizardPage;
  EditCnpj: TNewEdit;
  CnpjParceiro: String;
  ArquivosWhiteLabelCopiados: Boolean; 

function VerificaNomeComputadorIgualNomeUsuario(): Boolean;
begin
    Result := Uppercase(GetComputerNameString()) = Uppercase(GetUserNameString());
end;

function InstallServidorCheck(): Boolean;
begin
  Result := IsServidor or (CheckListServidor.Checked[0]);
end;

function InstallAdmCheck(): Boolean;
begin
  Result := IsAdm or CheckListFusion.Checked[0];
end;

function InstallPdvCheck(): Boolean;
begin
  Result := IsPdv or CheckListPdv.Checked[0];
end;

function InstallNfceCheck(): Boolean;
begin
  Result := IsNfce or CheckListNfce.Checked[0];
end;

function InstallAcbrCheck(): Boolean;
begin
  Result := InstallAdmCheck() or InstallNfceCheck();
end;

function PossuiInstaladorSQL() : Boolean;
begin
  Result := false;

  #ifdef Full
    Result := true;
  #endif
end;

function PossuiInstaladorDotNet() : Boolean;
begin
  Result := false;

  #ifdef Full
    Result := true;
  #endif
end;

function DbServidorCheck() : Boolean;
begin
  Result := (not IsServidor) and InstallServidorCheck();
end;

function DbRelatorioCheck() : Boolean;
begin
  Result := InstallServidorCheck();
end;

function DbFusionNfceCheck() : Boolean;
begin
  Result := (not IsNfce) and InstallNfceCheck();
end;

function DbFusionPdvCheck() : Boolean;
begin
  Result := (not IsPdv) and InstallPdvCheck();
end;

function RegIsServidorValue(Param: string) : string;
begin            
  Result := 'no';
  if (InstallServidorCheck()) then
    Result := 'yes';
end;

function RegIsAdmValue(Param: string) : string;
begin            
  Result := 'no';
  if (InstallAdmCheck()) then
    Result := 'yes';
end;

function RegIsPdvValue(Param: string) : string;
begin            
  Result := 'no';
  if (InstallPdvCheck()) then
    Result := 'yes';
end;

function RegIsNfceValue(Param: string) : string;
begin            
  Result := 'no';
  if (InstallNfceCheck()) then
    Result := 'yes';
end;

function ObtemVersaoSQLServer() : String;
  var
    Success: Boolean;
    RegValue, SubKey : String;
begin
  SubKey  := 'SOFTWARE\Microsoft\Microsoft SQL Server\FUSION\MSSQLServer\CurrentVersion';
  Success := RegQueryStringValue(HKEY_LOCAL_MACHINE, SubKey, 'CurrentVersion', RegValue);
  Result := '';

  if (Success = True) then begin
    Result := RegValue;
  end;
end;

function ObtemSenhaPadraoSQL() : String; 
var Versao : String;
begin
  Result := 'Fusion@ag4';
  Versao := copy(ObtemVersaoSQLServer, 1, 2);
  if (Versao = '10') then begin
    Result := 'fusion';
  end;
end;

function SQLServerInstalado() : Boolean;
begin
  Result := True;  
  if (ObtemVersaoSQLServer = '') then begin
      Result := False;
  end;
end;

function PrecisaInstalarSQL() : Boolean;
begin
  Result := (InstallServidorCheck() or InstallPdvCheck() or InstallNfceCheck());
end;

function DotNetNaoInstalado(): Boolean;
var
  bSuccess: Boolean;
  regVersion: Cardinal;
begin
  Result := True;

  bSuccess := RegQueryDWordValue(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', regVersion);

  if (True = bSuccess) and (regVersion >= 378389) then begin
    Result := False;
  end;
end;

function NenhumRecursoSelecionado(): Boolean;
begin
  Result := true;

  if InstallServidorCheck() then Result := false;
  if InstallAdmCheck() then Result := false;
  if InstallPdvCheck() then Result := false;
  if InstallNfceCheck() then Result := false;
end;

function InstaladorWhiteLabel(): Boolean;
begin
  Result := False;
  #ifdef WhiteLabel
    Result := True;
  #endif
end;

function NomeAtalhoAdm(Param: String): String;
begin
  Result := ExpandConstant('{ini:{tmp}\wl-config.ini,' + CnpjParceiro + ',NomeAtalhoAdm|Fusion Gestor}');
end;

function NomeAtalhoNfce(Param: String): String;
begin
  Result := ExpandConstant('{ini:{tmp}\wl-config.ini,' + CnpjParceiro + ',NomeAtalhoNfce|Fusion NFC-E}');
end;

function NomeAtalhoPdv(Param: String): String;
begin
  Result := ExpandConstant('{ini:{tmp}\wl-config.ini,' + CnpjParceiro + ',NomeAtalhoPdv|Fusion PDV}');
end;

procedure PrepararWhiteLabel();
  var cnpj: String;
begin
  cnpj := CnpjParceiro;
  ExtractTemporaryFile('wl-config.ini');

  try
    ExtractTemporaryFile(cnpj + '.profile');
    if InstallAdmCheck then ExtractTemporaryFile(cnpj + '-adm.ico');
    if InstallPdvCheck then ExtractTemporaryFile(cnpj+'-pdv.ico');
    if InstallNfceCheck then ExtractTemporaryFile(cnpj+'-nfce.ico');
  except
    RaiseException('Erro ao prepara instalação white label para o CNPJ');
  end;
end;

procedure CopiarArquivosWhiteLabel();
  var cnpj: String;
begin
  cnpj := CnpjParceiro;

  if not ArquivosWhiteLabelCopiados then begin
    try
      if InstallAdmCheck then begin 
        FileCopy(ExpandConstant('{tmp}\'+cnpj+ '-adm.ico'), ExpandConstant('{app}\adm.ico'), False);
        FileCopy(ExpandConstant('{tmp}\'+cnpj+ '-adm.ico'), ExpandConstant('{app}\Fusion\icon.ico'), False);
        FileCopy(ExpandConstant('{tmp}\'+cnpj+ '.profile'), ExpandConstant('{app}\Fusion\wlcfg'), False);
      end;

      if InstallPdvCheck then begin 
        FileCopy(ExpandConstant('{tmp}\'+cnpj+'-pdv.ico'), ExpandConstant('{app}\pdv.ico'), False);
        FileCopy(ExpandConstant('{tmp}\'+cnpj+ '.profile'), ExpandConstant('{app}\FusionPdv\wlcfg'), False);
      end;

      if InstallNfceCheck then begin
        FileCopy(ExpandConstant('{tmp}\'+cnpj+'-nfce.ico'), ExpandConstant('{app}\nfce.ico'), False);
        FileCopy(ExpandConstant('{tmp}\'+cnpj+ '.profile'), ExpandConstant('{app}\FusionNfce\wlcfg'), False);
      end;

      ArquivosWhiteLabelCopiados := True;
    except
      MsgBox('Erro ao copiar os arquivos de WhiteLabel', mbError, MB_OK);
    end;
  end;
end;

procedure SetTop(Control : TControl);
begin
  Control.Top := LastTop;
  LastTop := Control.Top + Control.Height + ScaleY(1);
end;

procedure IncrementaTop(Incremento : Integer);
begin
  LastTop := LastTop + Incremento;
end;

procedure CheckItem(CheckListBox : TNewCheckListBox; Index : Integer; Operation : TCheckItemOperation);
  var Count : Integer;
begin
  try
    Count := CheckListBox.SelCount;
    CheckListBox.CheckItem(Index, Operation);
  except
    //ignore
  end;
end;

procedure EnableItem(CheckListBox : TNewCheckListBox; Index : Integer; Enabled : Boolean);
begin
  try
    CheckListBox.ItemEnabled[Index] := Enabled;
  except
    //ignore
  end;
end;

procedure ServidorCheckHandler(Sender : TObject);
begin
  CheckItem(CheckListServidor, 1, coUncheck);
  CheckItem(CheckListServidor, 2, coUncheck);
  CheckItem(CheckListServidor, 3, coUncheck);

  if not IsAdm then
  begin
    EnableItem(CheckListFusion, 0, true);
  end;
  
  if (CheckListServidor.Checked[0]) then
  begin
    CheckItem(CheckListServidor, 1, coCheckWithChildren);
    CheckItem(CheckListServidor, 2, coCheckWithChildren);
    CheckItem(CheckListServidor, 3, coCheckWithChildren);      

    //Check Fusion
    CheckItem(CheckListFusion, 0, coCheckWithChildren);
    EnableItem(CheckListFusion, 0, false);
  end;   
end;

procedure MontarPaginaWhiteLabel(Page : TWizardPage);
var
  Text : TNewStaticText;
  Edit: TNewEdit; 
begin
  Text := TNewStaticText.Create(Page);
  Text.Width := Page.SurfaceWidth;
  Text.Parent := Page.Surface;
  Text.Caption := 'Para continuar informe seu CNPJ de Parceiro!';
  SetTop(Text);

  IncrementaTop(5);

  Edit := TNewEdit.Create(Page);
  Edit.Width := Page.SurfaceWidth;
  Edit.Parent := Page.Surface;
  Edit.MaxLength := 14;
  SetTop(Edit);

  EditCnpj := Edit;
end;

procedure CriarOpcaoServidor(Page : TWizardPage);
var
  Text : TNewStaticText; 
begin
  Text := TNewStaticText.Create(Page);
  Text.Width := Page.SurfaceWidth;
  Text.Parent := Page.Surface;
  Text.Caption := 'Vamos iniciar com os recursos de servidor.';

  if IsServidor then
  begin
    Text.Caption := 'Essa máquina já foi marcada como servidora. Agora será Atualizada!';
  end;

  SetTop(Text);
  IncrementaTop(5);

  if not IsServidor then
  begin
    CheckListServidor := TNewCheckListBox.Create(Page);  
    CheckListServidor.Width := Page.SurfaceWidth;
    CheckListServidor.Height := ScaleY(85);
    CheckListServidor.Flat := True;
    CheckListServidor.Parent := Page.Surface;
    CheckListServidor.OnClickCheck := @ServidorCheckHandler;

    CheckListServidor.AddCheckBox('Essa máquina será o servidor do Sistema', '', 0, False, True, True, True, nil);//0
    CheckListServidor.AddCheckBox('Instalar o Servidor de Licensas', 'Obrigatório', 1, False, True, True, True, nil);//1
    CheckListServidor.AddCheckBox('Instalar o Monitor Auxiliar', 'Obrigatório', 1, False, True, True, True, nil);//2
    
    if PossuiInstaladorSQL() then
      CheckListServidor.AddCheckBox('Instalar o SQL Server', 'Obrigatório', 1, False, True, True, True, nil);//3

    if PossuiInstaladorSQL() and SQLServerInstalado() then
      CheckListServidor.ItemSubItem[3] := 'Já foi instalado';

    SetTop(CheckListServidor);
  end
end;

procedure CriarBlocoOpcaoFusion(Page: TWizardPage);
var
  Text : TNewStaticText;
begin
  CheckListFusion := TNewCheckListBox.Create(Page);  
  CheckListFusion.Width := Page.SurfaceWidth;
  CheckListFusion.Height := ScaleY(20);
  CheckListFusion.Flat := True;
  CheckListFusion.Parent := Page.Surface;

  CheckListFusion.AddCheckBox('Instalar o Administrador', '', 0, False, True, True, True, nil);

  if IsAdm then
  begin
    CheckListFusion.ItemCaption[0] := 'Atualizar o Administrador';
    EnableItem(CheckListFusion, 0, false);
    CheckItem(CheckListFusion, 0, coCheckWithChildren);
  end;
  
  SetTop(CheckListFusion);
end;

procedure CheckListPdvCheckHandler(Sender : TObject);
begin
  CheckItem(CheckListPdv, 1, coUncheck);
  CheckItem(CheckListPdv, 2, coUncheck);
  
   if CheckListPdv.Checked[0] then
    begin
      CheckItem(CheckListPdv, 1, coCheckWithChildren);
      CheckItem(CheckListPdv, 2, coCheckWithChildren);
    end;
end;

procedure CriarBlocoOpcaoFusionPdv(Page : TWizardPage);
var
  Text : TNewStaticText;
begin
  CheckListPdv := TNewCheckListBox.Create(Page);  
  CheckListPdv.Width := Page.SurfaceWidth;
  CheckListPdv.Height := ScaleY(55);
  CheckListPdv.Flat := True;
  CheckListPdv.Parent := Page.Surface;
  CheckListPdv.OnClickCheck := @CheckListPdvCheckHandler;

  if IsPdv = False then
  begin
    CheckListPdv.AddCheckBox('Instalar o PDV', '', 0, False, True, True, True, nil);
    CheckListPdv.AddCheckBox('Instalar o Sincronizador do PDV', 'Obrigatório', 1, False, True, False, True, nil); 

    if PossuiInstaladorSQL() then
      CheckListPdv.AddCheckBox('Instalar o SQL Server', 'Obrigatório', 1, False, True, False, True, nil);

    if PossuiInstaladorSQL() and SQLServerInstalado() then
      CheckListPdv.ItemSubItem[2] := 'Já foi instalado';
  end;

  if IsPdv = True then
  begin;
    CheckListPdv.AddCheckBox('Atualizar o PDV', '', 0, True, False, True, True, nil);
    CheckListPdv.Height := ScaleY(20);
  end;

  SetTop(CheckListPdv);
end;

procedure CheckListNfceCheckHandler(Sender : TObject);
begin
  CheckItem(CheckListNfce, 1, coUncheck);
  CheckItem(CheckListNfce, 2, coUncheck);
  
  if (CheckListNfce.Checked[0]) then
    begin
      CheckItem(CheckListNfce, 1, coCheckWithChildren);
      CheckItem(CheckListNfce, 2, coCheckWithChildren);
    end;
end;

procedure CriarBlocoOpcaoFusionNfce(Page : TWizardPage);
begin  
  CheckListNfce := TNewCheckListBox.Create(Page);  
  CheckListNfce.Width := Page.SurfaceWidth;
  CheckListNfce.Height := ScaleY(55);
  CheckListNfce.Flat := True;
  CheckListNfce.Parent := Page.Surface;
  CheckListNfce.OnClickCheck := @CheckListNfceCheckHandler;

  if IsNfce = False then
  begin
    CheckListNfce.AddCheckBox('Instalar o NFC-E', '', 0, False, True, True, True, nil);
    CheckListNfce.AddCheckBox('Instalar o Sincronizador do NFC-E', 'Obrigatório', 1, False, True, False, True, nil);      

    if PossuiInstaladorSQL() then
      CheckListNfce.AddCheckBox('Instalar o SQL Server', 'Obrigatório', 1, False, True, False, True, nil);

    if PossuiInstaladorSQL() and SQLServerInstalado() then
      CheckListNfce.ItemSubItem[2] := 'Já foi instalado';
  end;

  if IsNfce = True then
  begin
    CheckListNfce.AddCheckBox('Atualizar o NFC-E', '', 0, True, False, True, True, nil);
    CheckListNfce.Height := ScaleY(20);
  end;

  SetTop(CheckListNfce);
end;

procedure CriarPaginaWhiteLabel();
begin
  LastTop := 0;
  Page1 := CreateCustomPage(wpLicense, 
      'Vamos configurar sua instalação White Label.', 
      'Apenas para parceiros habilitados!');

  MontarPaginaWhiteLabel(Page1);
end;

procedure CriarPaginaServidor();
begin
  LastTop := 0;
  Page1_1 := CreateCustomPage(Page1.ID, 
      'Vou ser seu assistente fera de isntalação!', 
      'Vou precisar que selecione algumas possibilidades, ok.');

  CriarOpcaoServidor(Page1_1);
end;

procedure CriarPaginaRecursos();
begin
  LastTop := 0;
  Page2 := CreateCustomPage(Page1_1.ID,
  'Vamos continuar nossa instalação TOP!',
  'Preciso que esoclha quais produtos deseja.');

  CriarBlocoOpcaoFusion(Page2);
  IncrementaTop(10);
  CriarBlocoOpcaoFusionPdv(Page2);
  IncrementaTop(10);
  CriarBlocoOpcaoFusionNfce(Page2);
end;

procedure CriarPaginaAvisoSQL();
  var Text : TNewStaticText;
      VersaoSQL : String;
begin
  VersaoSQL := copy(ObtemVersaoSQLServer, 1, 2);
  if SQLServerInstalado() and (VersaoSQL <> '10') then
  begin
    LastTop := 0;
    Page3 := CreateCustomPage(Page2.ID,
      'IMPORTANTE!!!',
      'AVISO SQL SERVER SUPERIOR AO 2008');

    Text := TNewStaticText.Create(Page3);
    Text.Width := Page3.SurfaceWidth;
    Text.Parent := Page3.Surface;
    Text.Caption := 
      'Foi identificado que o SQL Server da máquina é superior ao SQL Server 2008.' + #10#13 + + #10#13 + 

      'O SQL Server superior ao 2008 possui um recurso de Cache, recurso que pode fazer' + #10#13 + 
      'com que as Sequencias/IDs pulem numeração após o SQL ser encerrado de forma' + #10#13 +

      'forçada (desligar o PC direto pelo estabilizador).' + #10#13 + #10#13
      'Caso considere um problema, recomendamos desabilitar o Cache nos parâmetros' + #10#13 +
      'de inicialização do SQL Server.' + #10#13 + #10#13 +

      'Você pode pedir ajuda para nossa equipe de Suporte.';
    SetTop(Text);

  end;
end;

procedure CarregarVariaveisRegedit();
  var 
    RegValue : String;
begin
  IsUpdate := RegKeyExists(REG_ROOT_KEY, REG_SUB_KEY_IS_UPDATE);

  IsServidor := RegQueryStringValue(REG_ROOT_KEY, REG_SUB_KEY, 'IsServidor', RegValue) and (RegValue = 'yes');
  Log('IsServidor: ' + RegValue);

  IsAdm := RegQueryStringValue(REG_ROOT_KEY, REG_SUB_KEY, 'IsAdm', RegValue) and (RegValue = 'yes');
  Log('IsAdm: ' + RegValue);

  IsPdv := RegQueryStringValue(REG_ROOT_KEY, REG_SUB_KEY, 'IsPdv', RegValue) and (RegValue = 'yes');
  Log('IsPdv: ' + RegValue);

  IsNfce := RegQueryStringValue(REG_ROOT_KEY, REG_SUB_KEY, 'IsNfce', RegValue) and (RegValue = 'yes');
  Log('IsNfce: ' + RegValue);
end;

procedure InitializeWizard();
begin
  if (not InstaladorWhiteLabel) then CnpjParceiro := '21025760000123';
  
  CarregarVariaveisRegedit();
  CriarPaginaWhiteLabel();
  CriarPaginaServidor();
  CriarPaginaRecursos();
  CriarPaginaAvisoSQL();
  
  if (IsUpdate = false) and FileExists('{#MyDefaultDirName}\unins000.exe') then 
  begin
    MsgBox('Opa achei uma instalação incompatível com essa, preciso que desinstale-a para continuar!', mbInformation, MB_OK);
    Abort();
  end;
end;

procedure CurStepChanged(CurStep : TSetupStep);
begin
end;

function DirBackup() : String;
  var 
    Sucesso : Boolean;
begin
  Result := ExpandConstant('{#MyDefaultDirName}\Backup');
  Sucesso := true;

  if (DirExists(Result) = false) then
      Sucesso := CreateDir(Result);

  if (Sucesso = false) then 
    RaiseException('Falhar ao criar diretório de apoio: ' + Result);
end;

procedure FazerBackupSeguranca(NomeBanco : String);
var
  ResultCode: Integer;
  Param, Now, BackupName: String;
begin
  try
    Now := GetDateTimeString('yyyymmdd_hhnnss', #0, #0);
    BackupName := DirBackup() + '\' + NomeBanco + Now + '.bak';
    Param := '-S .\FUSION -U sa -P ' + ObtemSenhaPadraoSQL() + ' -m-1 -Q "BACKUP DATABASE [' + NomeBanco + '] TO DISK = N''' + BackupName + ''' WITH NOFORMAT, NOINIT, SKIP, NOREWIND, NOUNLOAD,  STATS = 10"';

    if (Exec('SQLCMD.EXE', Param, '', SW_HIDE, ewWaitUntilTerminated, ResultCode) = false) or (ResultCode <> 0) then
      begin
        RaiseException('Erro ao executar comando de Backup: SQLCMD.EXE ' + Param);  
      end;

    if FileExists(BackupName) = false then
      begin
        RaiseException('Não consegui confirmar o backup em: ' + BackupName);  
      end;
  except
    RaiseException(GetExceptionMessage());
  end;
end;

procedure InstalarDotNet();
  var
  DotNetInstaller, Param : String;
  Success: Boolean;
  ResultCode: Integer;
begin 
  if PossuiInstaladorDotNet() and DotNetNaoInstalado() then begin
    ExtractTemporaryFile('{#DotNetSetupName}');
    DotNetInstaller := ExpandConstant('{tmp}\{#DotNetSetupName}');
    Param := '/norestart';
        
    Success := Exec(DotNetInstaller, Param, '', SW_SHOW, ewWaitUntilTerminated, ResultCode);

    if (Success <> true) or (ResultCode <> 0) then begin
      RaiseException('Falha ao instalar .NET Framework');
    end;
  end;
end;

procedure InstalarSQLServer();
  var
  SQLInstaller, Param : String;
  Success: Boolean;
  ResultCode: Integer;
begin
  if PossuiInstaladorSQL() and PrecisaInstalarSQL() and (SQLServerInstalado() = false) then begin
    if (VerificaNomeComputadorIgualNomeUsuario()) then
      begin
          RaiseException('Opa, nome do computador não pode ser igual ao nome do usuário');
      end;

      ExtractTemporaryFile(ExpandConstant('{#SQLSetupName}'));

      SQLInstaller := ExpandConstant('{tmp}\{#SQLSetupName}');
      Param := '/QS /IACCEPTSQLSERVERLICENSETERMS /INDICATEPROGRESS /ACTION=Install /INSTANCENAME=FUSION /SAPWD=Fusion@ag4 /ASCOLLATION=Latin1_General_CI_AI /SQLCOLLATION=Latin1_General_CI_AI /SECURITYMODE=SQL /TCPENABLED=1 /BROWSERSVCSTARTUPTYPE=Automatic /FEATURES=SQLEngine';

      Success := Exec(SQLInstaller, Param, '', SW_SHOW, ewWaitUntilTerminated, ResultCode);

      if (ObtemVersaoSQLServer = '') then begin
        MsgBox(
          'Falha ao instalar SQL Server' + #10#13 +
          'As vezes apenas não consigo validar a instalação, confira se o servidor do SQL está ok em services.msc' + #10#13 +
          'Após a conclusão acesse a pasta `.installers` e execute manualmente', 
          mbError, 
          MB_OK
        );
      end;
  end;
end;

procedure DesinstalarVersaoAntiga();
var
  ResultCode: Integer;
  LocalRegistro: String;
  Uninstall: String;
begin
  LocalRegistro := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#SetupSetting("AppId")}_is1');

  if (RegQueryStringValue(HKLM, LocalRegistro, 'UninstallString', Uninstall)) then begin
    MsgBox('Versão antiga será desinstalada!', mbInformation, MB_OK);
    Exec(RemoveQuotes(Uninstall), '/SILENT', '', SW_SHOWNORMAL, ewWaitUntilTerminated, ResultCode);
  end;
end;

procedure InitializeUninstallProgressForm();
  var 
    Success : Boolean;
    ResultCode: Integer;
begin
  Success := Exec('taskkill.exe', '/F /IM acbrmonitor.exe', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  Success := Exec('taskkill.exe', '/F /IM taskreport.exe', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;

procedure CurPageChanged(CurPageID: Integer);
begin
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := False;
  if (PageID = Page1.ID) and (not InstaladorWhiteLabel) then begin
    Result := True;
  end
end;

function NextButtonClick(CurPageID : Integer) : Boolean ;
begin
  if (CurPageID = Page1.ID) then begin
    try
      if (Length(EditCnpj.Text) <> 14) then begin
        Result := False;
        MsgBox('CNPJ Parciero Precisa ter 14 dígitos', mbError, MB_OK);
      end else begin
        CnpjParceiro := EditCnpj.Text;
        Result := True;
      end;
    except
      Result := False;
      MsgBox(GetExceptionMessage(),mbError, MB_OK);
    end
  end else
    Result := True;
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
  var 
    Success : Boolean;
    ResultCode: Integer;
begin
  Result := '';
  Exec('taskkill.exe', '/F /IM taskreport.exe', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);

  try
    if (NenhumRecursoSelecionado() = true) then begin
      RaiseException('Nenhum recurso foi selecionado para instalação');
    end;

    PrepararWhiteLabel();
    DesinstalarVersaoAntiga();
    InstalarDotNet();
    InstalarSQLServer();
  except
    Result := GetExceptionMessage;                                                                              	
  end;
end;

procedure CurInstallProgressChanged(CurProgress, MaxProgress: Integer);
begin
  if (CurProgress = MaxProgress) then begin
    CopiarArquivosWhiteLabel();
  end;
end;
