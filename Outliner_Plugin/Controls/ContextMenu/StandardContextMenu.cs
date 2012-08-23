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

namespace Outliner.Controls.ContextMenu
{
internal static class StandardContextMenu
{
   internal static ToolStripDropDown Create(OutlinerSplitContainer container, TreeMode treeMode)
   {
      ToolStripDropDown strip = new ToolStripDropDown();
      strip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
      //FlowLayoutSettings settings = strip.LayoutSettings as FlowLayoutSettings;
      //settings.FlowDirection = FlowDirection.LeftToRight;
      //TableLayoutSettings settings = strip.LayoutSettings as TableLayoutSettings;
      //settings.ColumnCount = 4;
      //settings.RowCount = 2;
      strip.Tag = new Tuple<OutlinerSplitContainer, TreeMode>(container, treeMode);
      strip.Renderer = new OutlinerToolStripRenderer(new OutlinerColorTable());
      strip.Padding = new Padding(3, 2, 1, 1);


      ToolStripDropDownButton mode_btn = new ToolStripDropDownButton(GetModeImage(treeMode));
      mode_btn.Text = "Mode";
      mode_btn.TextImageRelation = TextImageRelation.ImageAboveText;
      mode_btn.ImageScaling = ToolStripItemImageScaling.None;
      mode_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      IEnumerable<OutlinerPluginData> modeTypes = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.TreeMode);
      foreach (OutlinerPluginData mode in modeTypes)
      {
         AddDropDownItem(mode_btn.DropDownItems, mode.DisplayName, null, mode_btn_click, mode.Type);
      }
      strip.Items.Add(mode_btn);


      ToolStripCheckedSplitButton filter_btn = new ToolStripCheckedSplitButton(ContextMenuResources.filter_32);
      filter_btn.Text = "Filters";
      filter_btn.TextImageRelation = TextImageRelation.ImageAboveText;
      filter_btn.ImageScaling = ToolStripItemImageScaling.None;
      filter_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      filter_btn.Checked = treeMode.Filters.Enabled;
      filter_btn.ButtonClick += new EventHandler(filter_btn_ButtonClick);
      filter_btn.DropDownItems.Add("Invert");
      ToolStripItem clearBtn = filter_btn.DropDownItems.Add("Clear", ContextMenuResources.delete);
      clearBtn.Enabled = treeMode.Filters.Count > 0;
      filter_btn.DropDownItems.Add(new ToolStripSeparator());
      int numFilters = AddFilters(filter_btn.DropDownItems, FilterCategories.Classes);
      if (numFilters > 0)
         filter_btn.DropDownItems.Add(new ToolStripSeparator());

      numFilters = AddFilters(filter_btn.DropDownItems, FilterCategories.Properties);
      if (numFilters > 0)
         filter_btn.DropDownItems.Add(new ToolStripSeparator());

      numFilters = AddFilters(filter_btn.DropDownItems, FilterCategories.Custom);
      
      strip.Items.Add(filter_btn);


      ToolStripDropDownButton sort_btn = new ToolStripDropDownButton("Sorting", ContextMenuResources.sort_alphabetical_32);
      sort_btn.ImageScaling = ToolStripItemImageScaling.None;
      sort_btn.TextImageRelation = TextImageRelation.ImageAboveText;
      sort_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      IEnumerable<OutlinerPluginData> sorterTypes = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.NodeSorter);
      foreach (OutlinerPluginData sorter in sorterTypes)
      {
         AddDropDownItem(sort_btn.DropDownItems, sorter.DisplayName, null, sort_itemClick, sorter.Type);
      }
      strip.Items.Add(sort_btn);

      strip.Items.Add(new ToolStripSeparator());


      ToolStripDropDownButton window_btn = new ToolStripDropDownButton();
      window_btn.ImageScaling = ToolStripItemImageScaling.None;
      if (container.Panel1Collapsed || container.Panel2Collapsed)
         window_btn.Image = ContextMenuResources.window_32;
      else if (container.Orientation == Orientation.Horizontal)
         window_btn.Image = ContextMenuResources.window_hor_32;
      else
         window_btn.Image = ContextMenuResources.window_ver_32;
      window_btn.Text = "Layout";
      window_btn.TextImageRelation = TextImageRelation.ImageAboveText;
      window_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSingle, ContextMenuResources.window, window_click);
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSplitHor, ContextMenuResources.window_split_hor, split_hor_btn_Click);
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSplitVer, ContextMenuResources.window_split_ver, split_ver_btn_Click);
      strip.Items.Add(window_btn);


      ToolStripTextBox textFilter = new ToolStripTextBox();
      NameFilter nameFilter = treeMode.PermanentFilters.Get(typeof(NameFilter)) as NameFilter;
      if (nameFilter != null)
         textFilter.Text = nameFilter.SearchString;
      textFilter.GotFocus += new EventHandler(textFilter_GotFocus);
      textFilter.LostFocus += new EventHandler(textFilter_LostFocus);
      textFilter.TextChanged += new EventHandler(textFilter_TextChanged);
      strip.Items.Add(textFilter);

      return strip;
   }



   private static ToolStripItem AddDropDownItem(ToolStripItemCollection itemCollection, String text, Image img, EventHandler clickHandler, Object tag)
   {
      ToolStripItem item = itemCollection.Add(text, img, clickHandler);
      item.ImageScaling = ToolStripItemImageScaling.None;
      item.Tag = tag;
      return item;
   }

   private static OutlinerSplitContainer GetContainer(object toolstripItem)
   {
      ToolStripItem item = toolstripItem as ToolStripItem;
      Object tag = item.GetCurrentParent().Tag;
      if (tag == null)
         tag = item.OwnerItem.GetCurrentParent().Tag;

      Tuple<OutlinerSplitContainer, TreeMode> data = tag as Tuple<OutlinerSplitContainer, TreeMode>;
      return data.Item1;
   }
   private static TreeMode GetTreeMode(object toolstripItem)
   {
      ToolStripItem item = toolstripItem as ToolStripItem;
      Object tag = item.GetCurrentParent().Tag;
      if (tag == null)
         tag = item.OwnerItem.GetCurrentParent().Tag;

      Tuple<OutlinerSplitContainer, TreeMode> data = tag as Tuple<OutlinerSplitContainer, TreeMode>;
      return data.Item2;
   }


   #region Mode
   
   private static Image GetModeImage(TreeMode mode)
   {
      Type modeType = mode.GetType();
      if (modeType.Equals(typeof(Outliner.Modes.Hierarchy.HierarchyMode)))
         return ContextMenuResources.hierarchy_mode_32;
      if (modeType.Equals(typeof(Outliner.Modes.Layer.LayerMode)))
         return ContextMenuResources.layer_mode_32;

      return ContextMenuResources.hierarchy_mode_32;
   }

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

   private static int AddFilters(ToolStripItemCollection itemCollection, FilterCategories categories)
   {
      IEnumerable<OutlinerPluginData> filterTypes = OutlinerPlugins.GetFilterPlugins(categories);
      foreach (OutlinerPluginData filter in filterTypes)
      {
         ToolStripItem item = AddDropDownItem(itemCollection, filter.DisplayName, filter.DisplayImageSmall, filter_ItemClick, filter.Type);
         //item.Checked = treeMode.Filters.Contains(filterType);
      }
      return filterTypes.Count();
   }

   static void filter_ItemClick(object sender, EventArgs e)
   {
      Type filterType = ((ToolStripItem)sender).Tag as Type;
      TreeMode treeMode = GetTreeMode(sender);
      Filter<Outliner.Scene.IMaxNodeWrapper> filter = treeMode.Filters.Get(filterType);
      if (filter == null)
      {
         filter = (Filter<Outliner.Scene.IMaxNodeWrapper>)Activator.CreateInstance(filterType, false);
         treeMode.Filters.Add(filter);
      }
      else
         treeMode.Filters.Remove(filter);
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

   #endregion


   #region Text Filter
   
   static void textFilter_TextChanged(object sender, EventArgs e)
   {
      TreeMode mode = GetTreeMode(sender);
      ToolStripTextBox textBox = sender as ToolStripTextBox;
      Type filterType = typeof(NameFilter);
      NameFilter filter = mode.PermanentFilters.Get(filterType) as NameFilter;
      if (filter != null)
         filter.SearchString = textBox.Text;
   }

   static void textFilter_LostFocus(object sender, EventArgs e)
   {
      MaxUtils.MaxInterfaces.Global.EnableAccelerators();
   }

   static void textFilter_GotFocus(object sender, EventArgs e)
   {
      TreeMode treeMode = GetTreeMode(sender);
      treeMode.Tree.Invalidate(); //Avoid tree going blank when docked.
      MaxUtils.MaxInterfaces.Global.DisableAccelerators();
   }

   #endregion
}
}
