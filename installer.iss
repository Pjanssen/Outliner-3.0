#define AppName "Outliner"
#define AppVersion "3.0"
#define AppPublisher "Pier Janssen"
#define AppURL "http://outliner.pjanssen.nl"
#define Min3dsMaxVersion 15

#include "functions.iss"

[Setup]
AppId={{2002D6FD-0D9C-4456-801F-979406054CFB}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
AppContact=pier@pjanssen.nl
AppCopyright=Copyright (C) Pier Janssen 2012
CreateAppDir=no
OutputBaseFilename=OutlinerSetup
Compression=lzma
SolidCompression=yes
AllowCancelDuringInstall=no
AppMutex=3dsmax,Global\3dsmax

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Messages]
SetupAppRunningError=Setup has detected that 3dsMax is currently running.%n%nPlease close all instances of it now, then click OK to continue, or Cancel to exit.

[CustomMessages]
SelectMaxVersionTitle=

[Files]
;Source: "C:\Code\OutlinerCore\Outliner.Core\bin\Release\Outliner.Core.dll"; DestDir: "{#MaxDir}\bin\assemblies\"; Flags: ignoreversion;
;Source: "C:\Code\OutlinerCore\Outliner.Core\bin\Release\Outliner.LayerTools.dll"; DestDir: "{#MaxDir}\bin\assemblies\"; Flags: ignoreversion;
;Source: "C:\Code\OutlinerCore\Outliner.Core\bin\Release\Outliner.MaxUtils.dll"; DestDir: "{#MaxDir}\bin\assemblies\"; Flags: ignoreversion;
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Code]
var
  MaxVersions: TArrayOfInteger;
  MaxVersionPage: TInputOptionWizardPage;
  AssemblyPath : String;
  OutlinerPlugCfgDir: String;


procedure CheckMaxVersion;
begin
  if not CompatibleVersionPresent(MaxVersions, {#Min3dsMaxVersion}) then
  begin
    MsgBox('No compatible 3dsMax version found!' + #13#10 
           + 'The {#AppName} {#AppVersion} requires 3dsMax ' 
           + GetMaxVersionString({#Min3dsMaxVersion}) 
           + ' or above.', mbError, MB_OK);
    Abort;
  end;
end;


procedure CreateMaxVersionPage;
var
  i: Integer;
begin
  MaxVersionPage := CreateInputOptionPage(wpWelcome,
    'Select 3dsMax version', '',
    'Select the 3dsMax version you want to install the {#AppName} for, then click Next.',
    True, False);
  for i := 0 to GetArrayLength(MaxVersions) - 1 do
  begin
    MaxVersionPage.Add(GetMaxProductName(MaxVersions[i]));
  end;
  MaxVersionPage.Values[0] := True;
end;


procedure InitializeWizard;
begin
  MaxVersions := GetInstalledMaxVersions();
  CheckMaxVersion();
  CreateMaxVersionPage();

  AssemblyPath := GetAssembliesDir(15);
  OutlinerPlugCfgDir := GetPlugCfgDir(15);
end;