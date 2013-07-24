#define AppName "Outliner"
#define AppPublisher "Pier Janssen"
#define AppURL "http://outliner.pjanssen.nl"
#define Min3dsMaxVersion 15
#define SourceDir "..\Build\Sources"

#include "functions.iss"

[Setup]
AppId=OutlinerMax{code:GetSelectedMaxVersion}
;{2002D6FD-0D9C-4456-801F-979406054CFB}
UsePreviousLanguage=no
SetupLogging=yes
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppContact=pier@pjanssen.nl
AppCopyright=Copyright (C) Pier Janssen 2013
CreateAppDir=no
OutputBaseFilename=OutlinerSetup
Compression=lzma
SolidCompression=yes
AllowCancelDuringInstall=no
AppMutex=3dsmax,Global\3dsmax
UninstallDisplayName={#AppName} for 3dsMax {code:GetSelectedMaxVersion}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Messages]
SetupAppRunningError=Setup has detected that 3dsMax is currently running.%n%nPlease close all instances of it now, then click OK to continue, or Cancel to exit.

[CustomMessages]
NoCompatibleMaxVersionsError=No compatible 3dsMax version found!%nThe {#AppName} {#AppVersion} requires 3dsMax %1 or above.
SelectMaxVersionTitle=Select 3dsMax version
SelectMaxVersionDescription=Select the 3dsMax version you want to install the {#AppName} for, then click Next.

[Files]
Source: "{#SourceDir}\assemblies\*"; DestDir: "{code:GetSelectedAssemblyDir}"; Flags: ignoreversion;

[Code]
var
  MaxVersions: Array of MaxVersionData;
  MaxVersionPage: TInputOptionWizardPage;

//=============================================================================

procedure CheckMaxVersion;
begin
  if GetArrayLength(MaxVersions) = 0 then
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

//=============================================================================

function GetSelectedVersion(): MaxVersionData;
var
  param: String;
  version: Integer;
  i: Integer;
begin
  param := GetCommandLineParam('/TARGETMAXVERSION');
  if not (param = '') then
  begin
    version := StrToInt(param);
    for i := 0 to GetArrayLength(MaxVersions) - 1 do
    begin
      if MaxVersions[i].Version = version then
      begin
        Result := MaxVersions[i];
        exit;
      end;
    end;
  end else if not (MaxVersionPage = nil) then
  begin
    Result := MaxVersions[MaxVersionPage.SelectedValueIndex];
  end;
end;

function GetSelectedMaxVersion(input: String): String;
var
  selVersion: MaxVersionData;
begin
  selVersion := GetSelectedVersion();
  if not (selVersion.Version = 0) then
    Result := GetMaxVersionString(selVersion.Version);
end;

function GetSelectedAssemblyDir(Input: String): String;
begin
  Result := GetAssembliesDir(GetSelectedVersion());
end;

function GetSelectedPlugCfgDir(Input: String): String;
begin
  Result := GetPlugCfgDir(GetSelectedVersion());
end;

//=============================================================================

procedure LogMaxVersionData(Version: MaxVersionData; Prefix: String);
begin
  Log(Prefix + 'Version:       ' + IntToStr(Version.Version));
  Log(Prefix + 'IsDesign:      ' + BoolToStr(Version.IsDesign));
  Log(Prefix + 'RegKey:        ' + Version.RegKey);
  Log(Prefix + 'RegSubKey:     ' + Version.RegSubKey);
  Log(Prefix + 'Name:          ' + Version.ProductName);
  Log(Prefix + 'Language:      ' + Version.Language);
  Log(Prefix + 'InstallDir:    ' + Version.Installdir);
  Log(Prefix + 'AssembliesDir: ' + GetAssembliesDir(Version));
  Log(Prefix + 'PlugCfgDir:    ' + GetPlugCfgDir(Version));
end;

procedure LogInstalledMaxVersions;
var
  i: Integer;
  v: MaxVersionData;
begin
  Log('--- Installed 3dsMax Versions ---');
  for i := 0 to GetArrayLength(MaxVersions) - 1 do
  begin
    v := MaxVersions[i];
    Log('[' + IntToStr(i) + ']');
    LogMaxVersionData(v, '    ');
  end;
  Log('');
end;

//=============================================================================

procedure InitializeWizard;
begin
  MaxVersions := GetInstalledMaxVersions(StrToInt(ExpandConstant('{#Min3dsMaxVersion}')));
  Log('');
  LogInstalledMaxVersions();
  CheckMaxVersion();
  CreateMaxVersionPage();
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  Log('--- Target 3dsMax Version ---');
  LogMaxVersionData(GetSelectedVersion(), '');
  Log('');
end;