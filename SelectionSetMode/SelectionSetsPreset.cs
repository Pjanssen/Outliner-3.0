using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Presets;
using System.Drawing;
using Outliner.NodeSorters;
using Outliner.Controls.Tree.Layout;
using Outliner.TreeNodeButtons;
using Outliner.Plugins;
using System.Xml.Serialization;

namespace Outliner.Modes.SelectionSet
{
[OutlinerPlugin(OutlinerPluginType.DefaultPreset)]
public class SelectionSetsPreset : OutlinerPreset
{
   public SelectionSetsPreset()
   {
      this.TextRes          = Resources.Preset_DisplayName;
      this.ResourceTypeName = typeof(Resources).FullName;
      this.Image16Res       = "selectionset_mode_16_dark";
      this.Image24Res       = "selectionset_mode_24_dark";

      this.TreeModeTypeName    = typeof(SelectionSetMode).FullName;
      this.NodeSorter          = new AlphabeticalSorter();
      this.Filters             = new Filters.MaxNodeFilterCombinator();

      this.TreeNodeLayout = new TreeNodeLayout();
      this.TreeNodeLayout.ItemHeight = 20;
      this.TreeNodeLayout.FullRowSelect = true;
      this.TreeNodeLayout.LayoutItems.Add(new ExpandButton() { UseVisualStyles = false });
      this.TreeNodeLayout.LayoutItems.Add(new NodeIcon(IconSet.Maya, false));
      this.TreeNodeLayout.LayoutItems.Add(new MayaStyleIndent());
      this.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.TreeNodeLayout.LayoutItems.Add(new AddButton());
      this.TreeNodeLayout.LayoutItems.Add(new RemoveButton());
      this.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.TreeNodeLayout.LayoutItems.Add(new HideButton());
      this.TreeNodeLayout.LayoutItems.Add(new FreezeButton());
   }
}
}
