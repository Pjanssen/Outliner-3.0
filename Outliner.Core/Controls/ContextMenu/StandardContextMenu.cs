using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Modes;
using System.Drawing;
using Outliner.Filters;
using System.Reflection;
using Outliner.Scene;
using Outliner.Plugins;
using OutlinerTree = Outliner.Controls.Tree;
using Outliner.Presets;
using Outliner.Controls.Options;
using Outliner.MaxUtils;

namespace Outliner.Controls.ContextMenu
{
internal static class StandardContextMenu
{
   internal static ToolStripDropDown Create(ContextMenuStrip menu, OutlinerSplitContainer container, OutlinerTree::TreeView tree, TreeMode treeMode)
   {
      ToolStripDropDown strip = new OutlinerContextMenu(menu);
      strip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
      //FlowLayoutSettings settings = strip.LayoutSettings as FlowLayoutSettings;
      //settings.FlowDirection = FlowDirection.LeftToRight;
      //TableLayoutSettings settings = strip.LayoutSettings as TableLayoutSettings;
      //settings.ColumnCount = 4;
      //settings.RowCount = 2;
      strip.Tag = new Tuple<OutlinerSplitContainer, OutlinerTree::TreeView, TreeMode>(container, tree, treeMode);
      strip.Renderer = new OutlinerToolStripRenderer(new OutlinerColorTable());
      strip.Padding = new Padding(3, 2, 1, 1);

      
      ToolStripDropDownButton preset_btn = new ToolStripDropDownButton("Preset");
      OutlinerPreset currentPreset = OutlinerGUP.Instance.GetActivePreset(tree);
      preset_btn.Image = currentPreset.Image24;
      preset_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      foreach (OutlinerPreset preset in OutlinerPresets.Presets.Where(p => p.IsDefaultPreset))
      {
         ToolStripMenuItem item = AddDropDownItem(preset_btn.DropDownItems, preset.Name, preset.Image16, preset_btn_click, preset);
         item.Checked = preset == currentPreset;
      }
      preset_btn.DropDownItems.Add(new ToolStripSeparator());
      foreach (OutlinerPreset preset in OutlinerPresets.Presets.Where(p => !p.IsDefaultPreset))
      {
         ToolStripMenuItem item = AddDropDownItem(preset_btn.DropDownItems, preset.Name, preset.Image16, preset_btn_click, preset);
         item.Checked = preset == currentPreset;
      }
      preset_btn.DropDownItems.Add(new ToolStripSeparator());
      AddDropDownItem(preset_btn.DropDownItems, ContextMenuResources.Str_EditPresets, null, EditPresets_Click, null);

      strip.Items.Add(preset_btn);

      strip.Items.Add(new ToolStripSeparator());


      //ToolStripDropDownButton mode_btn = new ToolStripDropDownButton("Mode");
      //mode_btn.TextImageRelation = TextImageRelation.ImageAboveText;
      //mode_btn.TextAlign = ContentAlignment.BottomCenter;
      //mode_btn.ImageScaling = ToolStripItemImageScaling.None;
      //mode_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      //IEnumerable<OutlinerPluginData> modeTypes = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.TreeMode);
      //Type currentModeType = treeMode.GetType();
      //foreach (OutlinerPluginData mode in modeTypes)
      //{
      //   AddDropDownItem(mode_btn.DropDownItems, mode.DisplayName, mode.DisplayImageSmall, mode_btn_click, mode.Type);
      //   if (currentModeType.Equals(mode.Type))
      //      mode_btn.Image = mode.DisplayImageLarge;
      //}
      //strip.Items.Add(mode_btn);



      ToolStripCheckedSplitButton filter_btn = new ToolStripCheckedSplitButton("Filters");
      filter_btn.Image = ContextMenuResources.filter_24;
      filter_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      filter_btn.Checked = treeMode.Filters.Enabled;
      filter_btn.ButtonClick += new EventHandler(filter_btn_ButtonClick);
      filter_btn.DropDown.Closing += new ToolStripDropDownClosingEventHandler(DropDown_Closing);
      
      ToolStripMenuItem invertBtn = filter_btn.DropDownItems.Add(ContextMenuResources.Str_InvertFilter) as ToolStripMenuItem;
      invertBtn.Checked = treeMode.Filters.Invert;
      invertBtn.Tag = treeMode.Filters;
      invertBtn.Click += new EventHandler(invertBtn_Click);

      ToolStripItem clearBtn = filter_btn.DropDownItems.Add(ContextMenuResources.Str_ClearFilter, ContextMenuResources.delete);
      clearBtn.Name = "Clear";
      clearBtn.Enabled = treeMode.Filters.Filters.Count > 0;
      clearBtn.Tag = treeMode.Filters;
      clearBtn.Click += new EventHandler(clearBtn_Click);

      filter_btn.DropDownItems.Add(new ToolStripSeparator());
      if (AddFilters(filter_btn.DropDownItems, FilterCategories.Classes, treeMode) > 0)
         filter_btn.DropDownItems.Add(new ToolStripSeparator());

      if (AddFilters(filter_btn.DropDownItems, FilterCategories.Properties, treeMode) > 0)
         filter_btn.DropDownItems.Add(new ToolStripSeparator());

      if (AddFilters(filter_btn.DropDownItems, FilterCategories.Custom, treeMode) > 0)
         filter_btn.DropDownItems.Add(new ToolStripSeparator());

      filter_btn.DropDownItems.Add("Advanced Filter...", null, advancedFilterClick);

      strip.Items.Add(filter_btn);


      ToolStripDropDownButton sort_btn = new ToolStripDropDownButton("Sorting");
      sort_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      Type currentSorterType = (treeMode.Tree.NodeSorter != null) ? treeMode.Tree.NodeSorter.GetType() : null;
      IEnumerable<OutlinerPluginData> sorterTypes = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.NodeSorter);
      foreach (OutlinerPluginData sorter in sorterTypes)
      {
         ToolStripMenuItem item = AddDropDownItem(sort_btn.DropDownItems, sorter.DisplayName, sorter.DisplayImageSmall, sort_itemClick, sorter.Type);
         if (sorter.Type.Equals(currentSorterType))
         {
            sort_btn.Image = sorter.DisplayImageLarge;
            item.Checked = true;
         }
      }
      strip.Items.Add(sort_btn);

      strip.Items.Add(new ToolStripSeparator());


      ToolStripDropDownButton window_btn = new ToolStripDropDownButton("Layout");
      if (container.Panel1Collapsed || container.Panel2Collapsed)
         window_btn.Image = ContextMenuResources.window_24;
      else if (container.Orientation == Orientation.Horizontal)
         window_btn.Image = ContextMenuResources.window_hor_24;
      else
         window_btn.Image = ContextMenuResources.window_ver_24;
      window_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSingle, ContextMenuResources.window, window_click);
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSplitHor, ContextMenuResources.window_split_hor, split_hor_btn_Click);
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSplitVer, ContextMenuResources.window_split_ver, split_ver_btn_Click);
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowNew, ContextMenuResources.window_new_16, window_new_click);
      strip.Items.Add(window_btn);

      ToolStripButton options_btn = new ToolStripButton("Options");
      options_btn.Image = ContextMenuResources.options_24;
      strip.Items.Add(options_btn);

      //ToolStripTextBox textFilter = new ToolStripTextBox();
      //NameFilter nameFilter = treeMode.PermanentFilters.Get(typeof(NameFilter)) as NameFilter;
      //if (nameFilter != null)
      //   textFilter.Text = nameFilter.SearchString;
      //textFilter.GotFocus += new EventHandler(textFilter_GotFocus);
      //textFilter.LostFocus += new EventHandler(textFilter_LostFocus);
      //textFilter.TextChanged += new EventHandler(textFilter_TextChanged);
      //strip.Items.Add(textFilter);

      foreach (ToolStripItem item in strip.Items)
      {
         item.DisplayStyle = ToolStripItemDisplayStyle.Image;
         item.TextImageRelation = TextImageRelation.ImageAboveText;
         item.TextAlign = ContentAlignment.BottomCenter;
         item.ImageScaling = ToolStripItemImageScaling.None;
      }

      return strip;
   }

   

   

   private static ToolStripMenuItem AddDropDownItem(ToolStripItemCollection itemCollection, String text, Image img, EventHandler clickHandler, Object tag)
   {
      ToolStripMenuItem item = new ToolStripMenuItem(text, img, clickHandler);
      item.Tag = tag;
      itemCollection.Add(item);
      return item;
   }

   private static Tuple<OutlinerSplitContainer, OutlinerTree::TreeView, TreeMode> GetStripTag(object toolstripItem)
   {
      ToolStripItem item = toolstripItem as ToolStripItem;
      Object tag = item.GetCurrentParent().Tag;
      if (tag == null)
         tag = item.OwnerItem.GetCurrentParent().Tag;

      return tag as Tuple<OutlinerSplitContainer, OutlinerTree::TreeView, TreeMode>;
   }
   
   private static OutlinerSplitContainer GetContainer(object toolstripItem)
   {
      Tuple<OutlinerSplitContainer, OutlinerTree::TreeView, TreeMode> data = GetStripTag(toolstripItem);
      return data.Item1;
   }
   private static OutlinerTree::TreeView GetTreeView(object toolstripItem)
   {
      Tuple<OutlinerSplitContainer, OutlinerTree::TreeView, TreeMode> data = GetStripTag(toolstripItem);
      return data.Item2;
   }
   private static TreeMode GetTreeMode(object toolstripItem)
   {
      Tuple<OutlinerSplitContainer, OutlinerTree::TreeView, TreeMode> data = GetStripTag(toolstripItem);
      return data.Item3;
   }


   #region Presets
   
   private static void preset_btn_click(object sender, EventArgs e)
   {
      OutlinerPreset preset = ((ToolStripItem)sender).Tag as OutlinerPreset;
      OutlinerTree::TreeView tree = GetTreeView(sender);
      OutlinerGUP.Instance.SwitchPreset(tree, preset, true);
   }

   private static void EditPresets_Click(object sender, EventArgs e)
   {
      PresetEditor editor = new PresetEditor(GetTreeView(sender));
      editor.ShowDialog(MaxInterfaces.MaxHwnd);
   }

   #endregion


   #region Mode

   private static void mode_btn_click(object sender, EventArgs e)
   {
      Type newModeType = ((ToolStripItem)sender).Tag as Type;
      TreeMode oldMode = GetTreeMode(sender);
      oldMode.Stop();
      TreeMode newMode = Activator.CreateInstance(newModeType, new object[] { oldMode.Tree }) as TreeMode;
      newMode.Start();
   }

   #endregion


   #region Filters

   static void filter_btn_ButtonClick(object sender, EventArgs e)
   {
      TreeMode treeMode = GetTreeMode(sender);
      treeMode.Filters.Enabled = !treeMode.Filters.Enabled;
   }

   static void invertBtn_Click(object sender, EventArgs e)
   {
      ToolStripMenuItem item = sender as ToolStripMenuItem;
      Filter<IMaxNodeWrapper> filter = item.Tag as Filter<IMaxNodeWrapper>;
      item.Checked = !item.Checked;
      filter.Invert = item.Checked;
   }

   static void clearBtn_Click(object sender, EventArgs e)
   {
      ToolStripMenuItem item = sender as ToolStripMenuItem;
      FilterCombinator<IMaxNodeWrapper> filter = item.Tag as FilterCombinator<IMaxNodeWrapper>;
      if (filter != null)
      {
         filter.Filters.Clear();
         filter.Invert = false;
         filter.Enabled = false;
      }
   }


   static void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
   {
      if ((e.CloseReason & ToolStripDropDownCloseReason.ItemClicked) == ToolStripDropDownCloseReason.ItemClicked)
         e.Cancel = true;
   }

   private static int AddFilters(ToolStripItemCollection itemCollection, FilterCategories categories, TreeMode treeMode)
   {
      IEnumerable<OutlinerPluginData> filterTypes = OutlinerPlugins.GetFilterPlugins(categories);
      foreach (OutlinerPluginData filter in filterTypes)
      {
         ToolStripMenuItem item = AddDropDownItem(itemCollection, filter.DisplayName, filter.DisplayImageSmall, filter_ItemClick, filter.Type);
         CheckFilterItem(item, treeMode);
      }
      return filterTypes.Count();
   }

   static void filter_ItemClick(object sender, EventArgs e)
   {
      ToolStripMenuItem item = sender as ToolStripMenuItem;
      Type filterType = item.Tag as Type;
      TreeMode treeMode = GetTreeMode(sender);
      Filter<Outliner.Scene.IMaxNodeWrapper> filter = treeMode.Filters.Filters.Get(filterType);
      if (filter == null)
      {
         filter = (Filter<Outliner.Scene.IMaxNodeWrapper>)Activator.CreateInstance(filterType, false);
         treeMode.Filters.Filters.Add(filter);
      }
      else
      {
         treeMode.Filters.Filters.Remove(filter);
      }

      treeMode.Filters.Enabled = (treeMode.Filters.Filters.Count > 0);
      CheckFilterItem(item, treeMode);
   }

   private static void CheckFilterItem(ToolStripMenuItem item, TreeMode treeMode)
   {
      Type filterType = item.Tag as Type;
      item.Checked = treeMode.Filters.Filters.Get(filterType) != null;
   }

   private static void advancedFilterClick(Object sender, EventArgs e)
   {
      TreeMode treeMode = GetTreeMode(sender);
      AdvancedFilterEditor editor = new AdvancedFilterEditor(treeMode.Filters);
      editor.Show(MaxInterfaces.MaxHwnd);
   }

   #endregion


   #region Sort
   static void sort_itemClick(object sender, EventArgs e)
   {
      TreeMode treeMode = GetTreeMode(sender);
      ToolStripItem item = sender as ToolStripItem;
      treeMode.Tree.NodeSorter = Activator.CreateInstance((Type)item.Tag) as NodeSorters.NodeSorter;
   }
   #endregion


   #region Layout

   static void window_click(object sender, EventArgs e)
   {
      OutlinerSplitContainer container = GetContainer(sender);
      container.ToSinglePanel(container.Panel1);
   }

   static void split_hor_btn_Click(object sender, EventArgs e)
   {
      OutlinerSplitContainer container = GetContainer(sender);
      container.ToSplitPanels();
      container.Orientation = Orientation.Horizontal;
   }

   static void split_ver_btn_Click(object sender, EventArgs e)
   {
      OutlinerSplitContainer container = GetContainer(sender);
      container.ToSplitPanels();
      container.Orientation = Orientation.Vertical;
   }

   static void window_new_click(object sender, EventArgs e)
   {
      TestForm f = new TestForm();

      OutlinerGUP outlinerInstance = OutlinerGUP.Instance;
      OutlinerState outlinerState = outlinerInstance.State;

      f.treeView1.Colors = OutlinerGUP.Instance.ColorScheme;
      f.treeView2.Colors = OutlinerGUP.Instance.ColorScheme;

      f.outlinerSplitContainer1.Orientation = outlinerState.SplitterOrientation;
      f.outlinerSplitContainer1.SplitterDistance = outlinerState.SplitterDistance;
      f.outlinerSplitContainer1.Panel1Collapsed = outlinerState.Panel1Collapsed;
      f.outlinerSplitContainer1.Panel2Collapsed = outlinerState.Panel2Collapsed;

      if (!outlinerState.Panel1Collapsed)
         outlinerInstance.SwitchPreset(f.treeView1, outlinerState.Tree1Preset, true);
      if (!outlinerState.Panel2Collapsed)
         outlinerInstance.SwitchPreset(f.treeView2, outlinerState.Tree2Preset, true);

      f.Show(new WindowWrapper(MaxInterfaces.COREInterface.MAXHWnd));
   }

   #endregion


   #region Text Filter
   
   static void textFilter_TextChanged(object sender, EventArgs e)
   {
      throw new NotImplementedException();
      //TreeMode mode = GetTreeMode(sender);
      //ToolStripTextBox textBox = sender as ToolStripTextBox;
      //Type filterType = typeof(NameFilter);
      //NameFilter filter = mode.PermanentFilters.Get(filterType) as NameFilter;
      //if (filter != null)
      //   filter.SearchString = textBox.Text;
   }

   static void textFilter_LostFocus(object sender, EventArgs e)
   {
      MaxInterfaces.Global.EnableAccelerators();
   }

   static void textFilter_GotFocus(object sender, EventArgs e)
   {
      TreeMode treeMode = GetTreeMode(sender);
      treeMode.Tree.Invalidate(); //Avoid tree going blank when docked.
      MaxInterfaces.Global.DisableAccelerators();
   }

   #endregion


}
}
