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
using Outliner.Controls.Options;
using Outliner.MaxUtils;
using Outliner.Configuration;
using Outliner.Controls.Tree.Layout;
using Outliner.NodeSorters;

namespace Outliner.Controls.ContextMenu
{
internal static class StandardContextMenu
{
   public static ToolStripDropDown Create(ContextMenuStrip menu, OutlinerSplitContainer container, OutlinerTree::TreeView tree, TreeMode treeMode)
   {
      ToolStripDropDown strip = new OutlinerContextMenu(menu);
      strip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
      strip.Tag = new Tuple<OutlinerSplitContainer, OutlinerTree::TreeView, TreeMode>(container, tree, treeMode);
      strip.Padding = new Padding(3, 2, 1, 1);
      strip.Renderer = new OutlinerToolStripRenderer(OutlinerGUP.Instance.ColorScheme.ContextMenuColorTable);

      
      ToolStripDropDownButton preset_btn = new ToolStripDropDownButton(ContextMenuResources.Context_Preset);
      OutlinerPreset currentPreset = OutlinerGUP.Instance.GetActivePreset(tree);
      preset_btn.Image = currentPreset.Image24;
      preset_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      IEnumerable<OutlinerPreset> presets = ConfigurationHelpers.GetConfigurations<OutlinerPreset>(OutlinerPaths.PresetsDir);
      foreach (OutlinerPreset preset in presets.Where(p => p.IsDefaultPreset))
      {
         ToolStripMenuItem item = AddDropDownItem(preset_btn.DropDownItems, preset.Text, preset.Image16, preset_btn_click, preset);
         item.Checked = preset == currentPreset;
      }
      preset_btn.DropDownItems.Add(new ToolStripSeparator());
      foreach (OutlinerPreset preset in presets.Where(p => !p.IsDefaultPreset))
      {
         ToolStripMenuItem item = AddDropDownItem(preset_btn.DropDownItems, preset.Text, preset.Image16, preset_btn_click, preset);
         item.Checked = preset == currentPreset;
      }

      strip.Items.Add(preset_btn);

      strip.Items.Add(new ToolStripSeparator());



      ToolStripCheckedSplitButton filter_btn = new ToolStripCheckedSplitButton(ContextMenuResources.Context_Filters);
      filter_btn.Image = ContextMenuResources.filter_24;
      filter_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      filter_btn.Checked = treeMode.Filters.Enabled;
      filter_btn.ButtonClick += new EventHandler(filter_btn_ButtonClick);
      filter_btn.DropDown.Closing += new ToolStripDropDownClosingEventHandler(DropDown_Closing);
      
      ToolStripMenuItem invertBtn = filter_btn.DropDownItems.Add(ContextMenuResources.Context_InvertFilter) as ToolStripMenuItem;
      invertBtn.Checked = treeMode.Filters.Invert;
      invertBtn.Tag = treeMode.Filters;
      invertBtn.Click += new EventHandler(invertBtn_Click);

      ToolStripItem clearBtn = filter_btn.DropDownItems.Add(ContextMenuResources.Context_ClearFilter, ContextMenuResources.delete);
      clearBtn.Enabled = treeMode.Filters.Filters.Count > 0;
      clearBtn.Tag = treeMode.Filters;
      clearBtn.Click += new EventHandler(clearBtn_Click);

      filter_btn.DropDownItems.Add(new ToolStripSeparator());
      
      IEnumerable<FilterConfiguration> filters = ConfigurationHelpers.GetConfigurations<FilterConfiguration>(OutlinerPaths.FiltersDir);
      IEnumerable<FilterConfiguration> classesFilters = filters.Where(f => f.Category == FilterCategory.Classes);
      IEnumerable<FilterConfiguration> propertiesFilters = filters.Where(f => f.Category == FilterCategory.Properties);
      IEnumerable<FilterConfiguration> customFilters = filters.Where(f => f.Category == FilterCategory.Custom);

      AddUserFileItems(filter_btn.DropDownItems, treeMode, classesFilters, filter_ItemClick);
      
      if (propertiesFilters.Count() > 0)
         filter_btn.DropDownItems.Add(new ToolStripSeparator());
      AddUserFileItems(filter_btn.DropDownItems, treeMode, propertiesFilters, filter_ItemClick);
      
      if (customFilters.Count() > 0)
         filter_btn.DropDownItems.Add(new ToolStripSeparator());
      AddUserFileItems(filter_btn.DropDownItems, treeMode, customFilters, filter_ItemClick);

      strip.Items.Add(filter_btn);


      ToolStripDropDownButton sort_btn = new ToolStripDropDownButton(ContextMenuResources.Context_Sorting);
      sort_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      NodeSorters.NodeSorter currentSorter = treeMode.Tree.NodeSorter as NodeSorters.NodeSorter;
      IEnumerable<SorterConfiguration> sorters = ConfigurationHelpers.GetConfigurations<SorterConfiguration>(OutlinerPaths.SortersDir)
                                                                     .Where(s => s.Sorter != null)
                                                                     .OrderBy(s => s.Text);
      foreach (SorterConfiguration sorterConfig in sorters)
      {
         ToolStripMenuItem item = AddDropDownItem(sort_btn.DropDownItems, sorterConfig.Text, sorterConfig.Image16, sort_itemClick, sorterConfig);
         if (sorterConfig.Sorter.Equals(currentSorter))
         {
            sort_btn.Image = sorterConfig.Image24;
            item.Checked = true;
         }
      }

      strip.Items.Add(sort_btn);

      strip.Items.Add(new ToolStripSeparator());


      ToolStripDropDownButton window_btn = new ToolStripDropDownButton(ContextMenuResources.Context_Layout);
      if (container.Panel1Collapsed || container.Panel2Collapsed)
         window_btn.Image = ContextMenuResources.window_24;
      else if (container.Orientation == Orientation.Horizontal)
         window_btn.Image = ContextMenuResources.window_hor_24;
      else
         window_btn.Image = ContextMenuResources.window_ver_24;
      window_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      window_btn.DropDownItems.Add(ContextMenuResources.Context_WindowSingle, ContextMenuResources.window, window_click);
      window_btn.DropDownItems.Add(ContextMenuResources.Context_WindowSplitHor, ContextMenuResources.window_split_hor, split_hor_btn_Click);
      window_btn.DropDownItems.Add(ContextMenuResources.Context_WindowSplitVer, ContextMenuResources.window_split_ver, split_ver_btn_Click);
      window_btn.DropDownItems.Add(ContextMenuResources.Context_WindowNew, ContextMenuResources.window_new_16, window_new_click);
      strip.Items.Add(window_btn);

      ToolStripDropDownButton options_btn = new ToolStripDropDownButton(ContextMenuResources.Context_Options);
      options_btn.Image = ContextMenuResources.options_24;
      options_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      strip.Items.Add(options_btn);

      options_btn.DropDownItems.Add(ContextMenuResources.Context_EditContextMenus, null, editContextMenusClick);
      options_btn.DropDownItems.Add(ContextMenuResources.Context_EditFilters, null, editFiltersClick);
      options_btn.DropDownItems.Add(ContextMenuResources.Context_EditLayouts, null, editLayoutsClick);
      options_btn.DropDownItems.Add(ContextMenuResources.Context_EditPresets, null, editPresetsClick);
      options_btn.DropDownItems.Add(ContextMenuResources.Context_EditSorters, null, editSortersClick);
      options_btn.DropDownItems.Add(new ToolStripSeparator());
      options_btn.DropDownItems.Add("About", null, aboutClick);

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

   private static ToolStripMenuItem AddUserFilesItem(ToolStripItemCollection itemCollection, ConfigurationFile item, EventHandler clickHandler)
   {
      return AddDropDownItem(itemCollection, item.Text, item.Image16, clickHandler, item);
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


   static void filter_ItemClick(object sender, EventArgs e)
   {
      ToolStripMenuItem item = sender as ToolStripMenuItem;
      FilterConfiguration config = item.Tag as FilterConfiguration;
      if (config.Filter == null)
         return;

      TreeMode treeMode = GetTreeMode(sender);

      Filter<Outliner.Scene.IMaxNodeWrapper> filter = treeMode.Filters.Filters.Get(config.Filter.GetType());
      if (filter == null)
         treeMode.Filters.Filters.Add(config.Filter);
      else
         treeMode.Filters.Filters.Remove(filter);

      treeMode.Filters.Enabled = (treeMode.Filters.Filters.Count > 0);
      CheckFilterItem(item, treeMode);
   }

   private static void CheckFilterItem(ToolStripMenuItem item, TreeMode treeMode)
   {
      Type filterType = null;
      FilterConfiguration config = item.Tag as FilterConfiguration;
      if (config != null && config.Filter != null)
         filterType = config.Filter.GetType();
      item.Checked = treeMode.Filters.Filters.Get(filterType) != null;
   }

   #endregion


   #region Sort
   static void sort_itemClick(object sender, EventArgs e)
   {
      TreeMode treeMode = GetTreeMode(sender);
      ToolStripItem item = sender as ToolStripItem;
      SorterConfiguration sorterConfig = item.Tag as SorterConfiguration;
      if (sorterConfig != null && sorterConfig.Sorter != null)
         treeMode.Tree.NodeSorter = sorterConfig.Sorter;
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
      MainWindow f = new MainWindow();

      OutlinerGUP outlinerInstance = OutlinerGUP.Instance;
      OutlinerState outlinerState = outlinerInstance.State;

      f.treeView1.Colors = OutlinerGUP.Instance.ColorScheme.TreeViewColorScheme;
      f.treeView2.Colors = OutlinerGUP.Instance.ColorScheme.TreeViewColorScheme;

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

   private static Int32 AddUserFileItems(ToolStripItemCollection itemCollection, TreeMode treeMode, IEnumerable<ConfigurationFile> items, EventHandler clickHandler)
   {
      foreach (ConfigurationFile item in items)
      {
         ToolStripMenuItem menuItem = AddUserFilesItem(itemCollection, item, clickHandler);
         IContextMenuExtendable contextMenuExt = item as IContextMenuExtendable;
         if (contextMenuExt != null)
         {
            menuItem.DropDownItems.AddRange(contextMenuExt.ContextMenuItem.ToToolStripMenuItems(null, null));
         }
         CheckFilterItem(menuItem, treeMode);
      }
      return items.Count();
   }

   #region Options
   
   private static void openEditor<T>(string directory, Type editorType, string title, Boolean showUIProperties) where T: class, new()
   {
      ConfigFilesEditor<T> editor = new ConfigFilesEditor<T>(directory, editorType);
      editor.Text = title;
      editor.ShowUIProperties = showUIProperties;
      editor.ShowDialog(MaxUtils.MaxInterfaces.MaxHwnd);
   }

   private static void editContextMenusClick(object sender, EventArgs e)
   {
      openEditor<ContextMenuModel>( OutlinerPaths.ContextMenusDir
                                  , typeof(ContextMenuModelEditor)
                                  , "Edit context-menus"
                                  , false);
   }

   private static void editFiltersClick(object sender, EventArgs e)
   {
      openEditor<FilterConfiguration>( OutlinerPaths.FiltersDir
                                     , typeof(FilterCollectionEditor)
                                     , "Edit filters"
                                     , true);
   }

   private static void editLayoutsClick(object sender, EventArgs e)
   {
      openEditor<TreeNodeLayout>( OutlinerPaths.LayoutsDir
                                , typeof(TreeNodeLayoutEditor)
                                , "Edit layouts"
                                , false);
   }

   private static void editPresetsClick(object sender, EventArgs e)
   {
      PresetsEditor editor = new PresetsEditor(OutlinerPaths.PresetsDir);
      editor.Text = "Edit presets";
      editor.ShowUIProperties = true;
      editor.ShowDialog(MaxUtils.MaxInterfaces.MaxHwnd);
   }

   private static void editSortersClick(object sender, EventArgs e)
   {
      openEditor<SorterConfiguration>( OutlinerPaths.SortersDir
                                     , typeof(SorterConfigurationEditor)
                                     , "Edit sorters"
                                     , true);
   }

   private static void aboutClick(object sender, EventArgs e)
   {
      AboutBox aboutBox = new AboutBox();
      aboutBox.ShowDialog(MaxInterfaces.MaxHwnd);
   }

   #endregion
}
}
