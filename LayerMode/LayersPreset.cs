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

namespace Outliner.Modes.Layer
{
[OutlinerPlugin(OutlinerPluginType.DefaultPreset)]
public class LayersPreset : OutlinerPreset
{
   public LayersPreset()
   {
      this.TextRes          = Resources.Preset_DisplayName;
      this.ResourceTypeName = typeof(Resources).FullName;
      this.Image16Res       = "layer_mode_16";
      this.Image24Res       = "layer_mode_24";

      this.TreeModeTypeName = typeof(LayerMode).FullName;
      this.NodeSorter = new AlphabeticalSorter(false);
      this.Filters = new Filters.MaxNodeFilterCombinator();

      this.TreeNodeLayout = new TreeNodeLayout();
      this.TreeNodeLayout.ItemHeight = 20;
      this.TreeNodeLayout.FullRowSelect = true;
      this.TreeNodeLayout.LayoutItems.Add(new ExpandButton() { UseVisualStyles = false });
      this.TreeNodeLayout.LayoutItems.Add(new NodeIcon(IconSet.Maya, false));
      this.TreeNodeLayout.LayoutItems.Add(new MayaStyleIndent());
      this.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.TreeNodeLayout.LayoutItems.Add(new AddButton());
      this.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.TreeNodeLayout.LayoutItems.Add(new HideButton());
      this.TreeNodeLayout.LayoutItems.Add(new FreezeButton());
   }
}
}