#define MaxRegKey "Software\Autodesk\3dsMax"

[Code]
//Retrieves the LocalMachine root registry key for the current platform (x86/x64).
function GetLMRootKey: Integer;
begin
  if IsWin64 then
  begin
    Result := HKLM64;
  end
  else
  begin
    Result := HKEY_LOCAL_MACHINE;
  end;
end;


function QueryRegStringValue(Root: Integer; Key: String; ValueName: String): String;
var
  Value: String;
begin
    if (RegQueryStringValue(Root, Key, ValueName, Value)) then
    begin
      Result := Value;
    end;
end;


//Retrieves the installed 3dsMax versions from the registry.
//Returns an array with version numbers, e.g.: [13, 14, 15];
function GetInstalledMaxVersions: TArrayOfInteger;
var
  Names: TArrayOfString;
  i: Integer;
  VStr: String;
  Version: Integer;
  Versions: Array of Integer;
begin
  if RegGetSubkeyNames(GetLMRootKey(), '{#MaxRegKey}', Names) then
  begin
    for i := 0 to GetArrayLength(Names) - 1 do
    begin
      VStr := Names[i];
      Delete(VStr, Pos('.', VStr), 100);
      Version := StrToIntDef(VStr, 0);
      if (Version < 100) then
      begin
        SetArrayLength(Versions, GetArrayLength(Versions) + 1);
        Versions[i] := Version;
      end;
    end;
  end;
  Result := Versions;
end;


function GetMaxVersionKey(Version : Integer) : String;
begin
  Result := '{#MaxRegKey}' + '\' + IntToStr(Version) + '.0';
end;

function GetMaxInstallDir(Version: Integer) : String;
begin
  Result := QueryRegStringValue( GetLMRootKey(), GetMaxVersionKey(Version), 'InstallDir');
end;

function GetMaxProductName(Version : Integer) : String;
begin
  Result := QueryRegStringValue( GetLMRootKey(), GetMaxVersionKey(Version), 'ProductName');
end;


//Turns a 3dsmax version integer into a string. E.g.: 15 -> '2013'.
function GetMaxVersionString(Version: Integer): String;
begin
  Result := IntToStr(1998 + Version);
end;

function GetMaxLanguage(Version: Integer): String;
begin
  Result := QueryRegStringValue(HKCU, GetMaxVersionKey(Version), 'CurrentLanguage');
end;

function GetMaxLangDir(Version: Integer): String;
var
  LangKey: String;
begin
  LangKey := GetMaxVersionKey(Version) + '\LanguagesInstalled\' + GetMaxLanguage(Version);
  Result := QueryRegStringValue( GetLMRootKey(), LangKey, 'LangSubDir');
end;


//Tests if the array of versions contains a version that is larger than or 
//equal to the given minimal version.
function CompatibleVersionPresent(Versions: TArrayOfInteger; 
                                  MinVersion: Integer): Boolean;
var
  i: Integer;
begin
  Result := False;
  for i := 0 to GetArrayLength(Versions) - 1 do
  begin
    if (Versions[i] >= MinVersion) then
    begin
      Result := True;
      exit;
    end;
  end;
end;


function GetPlugCfgDir(Version: Integer): String;
var
  Path: String;
begin
  Path := ExpandConstant('{localappdata}')
          + '\Autodesk\3dsMax\'
          + GetMaxVersionString(Version);

  if IsWin64 then
  begin
    Path := Path + ' - 64bit\';
  end
  else
  begin
    Path := Path + ' - 32bit\';
  end;

  Path := Path + GetMaxLanguage(Version)
               + '\'
               + GetMaxLangDir(Version)
               + '\plugcfg\Outliner';

  Result := Path;
end;


function GetAssembliesDir(Version: Integer): String;
begin
  Result := GetMaxInstallDir(Version) + '\bin\assemblies';
end;