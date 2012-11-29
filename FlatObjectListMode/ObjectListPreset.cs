using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Outliner.Presets;
using System.Drawing;
using Outliner.NodeSorters;
using Outliner.Controls.Tree.Layout;
using Outliner.TreeNodeButtons;

namespace Outliner.Modes.FlatObjectList
{
[OutlinerPlugin(OutlinerPluginType.DefaultPreset)]
public class ObjectListPreset : OutlinerPreset
{
   public ObjectListPreset()
   {
      this.TextRes = Resources.Preset_DisplayName;
      this.ResourceTypeName = typeof(Resources).FullName;
      this.Image16Res = "flatobjectlist_mode_16_dark";
      this.Image24Res = "flatobjectlist_mode_24_dark";

      this.TreeModeTypeName = typeof(FlatObjectListMode).FullName;
      this.NodeSorter = new AlphabeticalSorter(false);
      this.Filters = new Filters.MaxNodeFilterCombinator();

      this.TreeNodeLayout = new TreeNodeLayout();
      this.TreeNodeLayout.ItemHeight = 20;
      this.TreeNodeLayout.FullRowSelect = true;
      this.TreeNodeLayout.LayoutItems.Add(new NodeIcon(IconSet.Maya, false));
      this.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.TreeNodeLayout.LayoutItems.Add(new HideButton());
      this.TreeNodeLayout.LayoutItems.Add(new FreezeButton());
   }
}
}
