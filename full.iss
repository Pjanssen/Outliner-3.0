#define AppVersion "3.0 Full"

#include "installer.iss"

[Files]
Source: "Deployment\plugcfg\*"; DestDir: "{code:GetSelectedPlugCfgDir}"; Flags: recursesubdirs ignoreversion;