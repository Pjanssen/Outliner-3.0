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
NoCompatibleMaxVersionsError=No compatible 3dsMax version found!%nThe {#AppName} {#AppVersion} requires 3dsMax %1 or above.
SelectMaxVersionTitle=Select 3dsMax version
SelectMaxVersionDescription=Select the 3dsMax version you want to install the {#AppName} for, then click Next.

[Files]
Source: "Deployment\assemblies\*"; DestDir: "{code:GetSelectedAssemblyDir}"; Flags: ignoreversion;
Source: "Deployment\plugcfg\*"; DestDir: "{code:GetSelectedPlugCfgDir}"; Flags: recursesubdirs ignoreversion;

[Code]
var
  MaxVersions: Array of MaxVersionData;
  MaxVersionPage: TInputOptionWizardPage;

     
procedure CheckMaxVersion;
begin
  if not CompatibleVersionPresent(MaxVersions, {#Min3dsMaxVersion}) then
  begin
    MsgBox( FmtMessage( ExpandConstant('{cm:NoCompatibleMaxVersionsError}')
                      , [GetMaxVersionString({#Min3dsMaxVersion})])
          , mbError, MB_OK);
    Abort;
  end;
end;
    
 
procedure CreateMaxVersionPage;
var
  i: Integer;
begin
  MaxVersionPage := CreateInputOptionPage(wpWelcome,
    ExpandConstant('{cm:SelectMaxVersionTitle}'), '',
    ExpandConstant('{cm:SelectMaxVersionDescription}'),
    True, False);
  for i := 0 to GetArrayLength(MaxVersions) - 1 do
  begin
    if (MaxVersions[i].Version >= {#Min3dsMaxVersion}) then
      MaxVersionPage.Add(MaxVersions[i].ProductName);
  end;
  MaxVersionPage.Values[0] := True;
end;


function GetSelectedVersion(): MaxVersionData;
var
  i: Integer;
begin
  for i := 0 to GetArrayLength(MaxVersions) - 1 do
  begin
    if MaxVersionPage.Values[i] then
    begin
      Result := MaxVersions[i];
      exit;
    end;
  end;
end;

function GetSelectedAssemblyDir(Input: String): String;
begin
  Result := GetAssembliesDir(GetSelectedVersion());
end;

function GetSelectedPlugCfgDir(Input: String): String;
begin
  Result := GetPlugCfgDir(GetSelectedVersion());
end;

procedure InitializeWizard;
begin
  MaxVersions := GetInstalledMaxVersions();
  CheckMaxVersion();
  CreateMaxVersionPage();
end;