#define AppVersion "3.0 Regular"

#include "installer.iss"

[Files]
Source: "Deployment\plugcfg\ContextMenus\*"; DestDir: "{code:GetSelectedPlugCfgDir}\ContextMenus"; Flags: ignoreversion;
Source: "Deployment\plugcfg\Filters\*"; DestDir: "{code:GetSelectedPlugCfgDir}\Filters"; Flags: ignoreversion;
Source: "Deployment\plugcfg\Plugins\Filters.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "Deployment\plugcfg\Plugins\FlatObjectListMode.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "Deployment\plugcfg\Plugins\HierarchyMode.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "Deployment\plugcfg\Plugins\LayerMode.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "Deployment\plugcfg\Plugins\MaterialMode.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "Deployment\plugcfg\Plugins\NodeSorters.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;
Source: "Deployment\plugcfg\Plugins\TreeNodeButtons.dll"; DestDir: "{code:GetSelectedPlugCfgDir}\Plugins"; Flags: ignoreversion;