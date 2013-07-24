#define AppVersion "3.0 Full"

#include "installer.iss"

[Files]
Source: "{#SourceDir}\plugcfg\*"; DestDir: "{code:GetSelectedPlugCfgDir}"; Flags: recursesubdirs ignoreversion;

; Source: "{#DeploymentDir}\plugcfg\Plugins\*"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;

; Source: "{#DeploymentDir}\plugcfg\ColorSchemes\*"; DestDir: "{code:GetSelectedPlugCfgDir}\ColorSchemes"; Flags: onlyifdoesntexist;
; Source: "{#DeploymentDir}\plugcfg\ContextMenus\*"; DestDir: "{code:GetSelectedPlugCfgDir}\ContextMenus"; Flags: onlyifdoesntexist;
; Source: "{#DeploymentDir}\plugcfg\Filters\*"; DestDir: "{code:GetSelectedPlugCfgDir}\Filters"; Flags: onlyifdoesntexist;
; Source: "{#DeploymentDir}\plugcfg\Layouts\*"; DestDir: "{code:GetSelectedPlugCfgDir}\Layouts"; Flags: onlyifdoesntexist;
; Source: "{#DeploymentDir}\plugcfg\NodeSorters\*"; DestDir: "{code:GetSelectedPlugCfgDir}\NodeSorters"; Flags: onlyifdoesntexist;
; Source: "{#DeploymentDir}\plugcfg\Presets\*"; DestDir: "{code:GetSelectedPlugCfgDir}\Presets"; Flags: onlyifdoesntexist;