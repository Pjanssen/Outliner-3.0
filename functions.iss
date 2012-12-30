#define MaxRegKey "Software\Autodesk\3dsMax"
#define MaxDesignRegKey "Software\Autodesk\3dsMaxDesign"

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


//=============================================================================


type 
MaxVersionData = record
  Version: Integer;
  IsDesign: Boolean;
  ProductName: String;
  Language: String;
  Installdir: String;
  RegKey: String;
  RegSubKey: String;
end;

function GetMaxVersionKey(Version: MaxVersionData): String;
begin
  Result := Version.RegKey + '\' + Version.RegSubKey;
end;

//Turns a 3dsmax version integer into a string. E.g.: 15 -> '2013'.
function GetMaxVersionString(Version: Integer): String;
begin
  Result := IntToStr(1998 + Version);
end;

//Converts a string representation of a max version to an integer.
//E.g. '15.0' -> 15
function GetVersionFromString(Version: String): Integer;
begin
  Delete(Version, Pos('.', Version), 100);
  Result := StrToIntDef(Version, 0);
end;



//=============================================================================




//Collects all data about a max version from te registry.
function GetMaxVersionData(Key: String; VersionKey: String): MaxVersionData;
var
  MaxVersion: MaxVersionData;
  VersionString: String;
begin
  MaxVersion.RegKey := Key;
  MaxVersion.RegSubKey := VersionKey;
  MaxVersion.Version := GetVersionFromString(VersionKey);
  MaxVersion.ProductName := QueryRegStringValue(GetLMRootKey(), GetMaxVersionKey(MaxVersion), 'ProductName');
  MaxVersion.Language := QueryRegStringValue(HKCU, GetMaxVersionKey(MaxVersion), 'CurrentLanguage');
  MaxVersion.InstallDir := QueryRegStringValue(GetLMRootKey(), GetMaxVersionKey(MaxVersion), 'InstallDir');

  Result := MaxVersion;
end;

//Retrieves the max versions from a registry key.
function GetVersionsFromRegistry(Key: String): Array of MaxVersionData;
var
  Names: TArrayOfString;
  i: Integer;
  Version: Integer;
  Versions: Array of MaxVersionData;
begin
  if RegGetSubkeyNames(GetLMRootKey(), Key, Names) then
  begin
    for i := 0 to GetArrayLength(Names) - 1 do
    begin
      Version := GetVersionFromString(Names[i]);
      if (Version < 100) then
      begin
        SetArrayLength(Versions, GetArrayLength(Versions) + 1);
        Versions[i] := GetMaxVersionData(Key, Names[i]);
      end;
    end;
  end;
  Result := Versions;
end;

//Retrieves the installed 3dsMax versions from the registry.
//Returns an array with MaxVersionData records.
function GetInstalledMaxVersions: Array of MaxVersionData;
var
  MaxVersions: Array of MaxVersionData;
  MaxDesignVersions: Array of MaxVersionData;
  Versions: Array of MaxVersionData;
  maxLen: Integer;
  maxDesignLen: Integer;
  i: Integer;
begin
  MaxVersions := GetVersionsFromRegistry('{#MaxRegKey}');
  MaxDesignVersions := GetVersionsFromRegistry('{#MaxDesignRegKey}');
  maxLen := GetArrayLength(MaxVersions);
  maxDesignLen := GetArrayLength(MaxDesignVersions);
  SetArrayLength(Versions, maxLen + maxDesignLen);
  for i := 0 to maxLen - 1 do
  begin
    Versions[i] := MaxVersions[i];
    Versions[i].IsDesign := False;
  end;
  for i := 0 to maxDesignLen - 1 do
  begin
    Versions[maxLen + i] := MaxDesignVersions[i];
    Versions[maxLen + i].IsDesign := True;
  end;
  Result := Versions;
end;


//=============================================================================


//Tests if the array of versions contains a version that is larger than or 
//equal to the given minimal version.
function CompatibleVersionPresent(Versions: Array of MaxVersionData; 
                                  MinVersion: Integer): Boolean;
var
  i: Integer;
begin
  Result := False;
  for i := 0 to GetArrayLength(Versions) - 1 do
  begin
    if (Versions[i].Version >= MinVersion) then
    begin
      Result := True;
      exit;
    end;
  end;
end;


//=============================================================================


function GetAssembliesDir(Version: MaxVersionData): String;
begin
  Result := Version.InstallDir + '\bin\assemblies';
end;

function GetMaxLangDir(Version: MaxVersionData): String;
var
  LangKey: String;
begin
  LangKey := GetMaxVersionKey(Version) + '\LanguagesInstalled\' + Version.Language;
  Result := QueryRegStringValue(GetLMRootKey(), LangKey, 'LangSubDir');
end;

function GetPlugCfgDir(Version: MaxVersionData): String;
var
  Path: String;
begin
  Path := ExpandConstant('{localappdata}') + '\Autodesk\3dsMax';

  if Version.IsDesign then
  begin
    Path := Path + 'Design\';
  end else
  begin
    Path := Path + '\';
  end;

  Path := Path + GetMaxVersionString(Version.Version);

  if IsWin64 then
  begin
    Path := Path + ' - 64bit\';
  end
  else
  begin
    Path := Path + ' - 32bit\';
  end;

  Path := Path + Version.Language
               + '\'
               + GetMaxLangDir(Version)
               + '\plugcfg\Outliner';

  Result := Path;
end;

