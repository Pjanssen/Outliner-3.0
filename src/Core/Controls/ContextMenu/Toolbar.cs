using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PJanssen.Outliner.Configuration;
using PJanssen.Outliner.Modes;
using OutlinerTree = PJanssen.Outliner.Controls.Tree;

namespace PJanssen.Outliner.Controls.ContextMenu
{
public class Toolbar
{
   public static ToolStrip Create(ToolStrip strip
                                         , OutlinerSplitContainer container
                                         , OutlinerTree::TreeView tree
                                         , TreeMode treeMode)
   {
      OutlinerGUP outliner = OutlinerGUP.Instance;
      OutlinerColorScheme colorScheme = outliner.ColorScheme;

      strip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
      strip.Tag = new Tuple<OutlinerSplitContainer, OutlinerTree::TreeView, TreeMode>(container, tree, treeMode);
      strip.Padding = new Padding(3, 2, 1, 1);
      strip.Renderer = new OutlinerToolStripRenderer(colorScheme.ContextMenuColorTable);


      ToolStripDropDownButton preset_btn = new ToolStripDropDownButton(ContextMenuResources.Context_Preset);
      OutlinerPreset currentPreset = OutlinerGUP.Instance.GetActivePreset(tree);
      preset_btn.Image = currentPreset.Image24;
      preset_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      IEnumerable<OutlinerPreset> presets = Configurations.GetConfigurations<OutlinerPreset>(OutlinerPaths.PresetsDir);
      foreach (OutlinerPreset preset in presets.Where(p => p.IsDefaultPreset))
      {
         //ToolStripMenuItem item = AddDropDownItem(preset_btn.DropDownItems, preset.Text, preset.Image16, preset_btn_click, preset);
         //item.Checked = preset.Text == currentPreset.Text;
      }
      preset_btn.DropDownItems.Add(new ToolStripSeparator());
      foreach (OutlinerPreset preset in presets.Where(p => !p.IsDefaultPreset))
      {
         //ToolStripMenuItem item = AddDropDownItem(preset_btn.DropDownItems, preset.Text, preset.Image16, preset_btn_click, preset);
         //item.Checked = preset.Text == currentPreset.Text;
      }

      strip.Items.Add(preset_btn);
      strip.Items.Add(new ToolStripSeparator());


      SetDefaultItemProperties(strip);

      return strip;
   }

   private static void SetDefaultItemProperties(ToolStrip strip)
   {
      foreach (ToolStripItem item in strip.Items)
      {
         item.DisplayStyle = ToolStripItemDisplayStyle.Image;
         item.TextImageRelation = TextImageRelation.ImageAboveText;
         item.TextAlign = ContentAlignment.BottomCenter;
         item.ImageScaling = ToolStripItemImageScaling.None;
      }
   }
}
}
