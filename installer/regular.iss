#define AppVersion "3.0 Regular"

#include "installer.iss"

[Files]
Source: "{#SourceDir}\plugcfg\ColorSchemes\*"; DestDir: "{code:GetSelectedPlugCfgDir}\ColorSchemes"; Flags: onlyifdoesntexist;
Source: "{#SourceDir}\plugcfg\ContextMenus\*"; DestDir: "{code:GetSelectedPlugCfgDir}\ContextMenus"; Flags: onlyifdoesntexist;
Source: "{#SourceDir}\plugcfg\Filters\*"; DestDir: "{code:GetSelectedPlugCfgDir}\Filters"; Flags: onlyifdoesntexist;
Source: "{#SourceDir}\plugcfg\Layouts\*"; DestDir: "{code:GetSelectedPlugCfgDir}\Layouts"; Flags: onlyifdoesntexist;
Source: "{#SourceDir}\plugcfg\NodeSorters\*"; DestDir: "{code:GetSelectedPlugCfgDir}\NodeSorters"; Flags: onlyifdoesntexist;
Source: "{#SourceDir}\plugcfg\Presets\*"; DestDir: "{code:GetSelectedPlugCfgDir}\Presets"; Flags: onlyifdoesntexist;

Source: "{#SourceDir}\plugcfg\Plugins\Filters.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "{#SourceDir}\plugcfg\Plugins\FlatObjectListMode.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "{#SourceDir}\plugcfg\Plugins\HierarchyMode.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "{#SourceDir}\plugcfg\Plugins\LayerMode.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "{#SourceDir}\plugcfg\Plugins\MaterialMode.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "{#SourceDir}\plugcfg\Plugins\NodeSorters.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "{#SourceDir}\plugcfg\Plugins\TreeNodeButtons.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;