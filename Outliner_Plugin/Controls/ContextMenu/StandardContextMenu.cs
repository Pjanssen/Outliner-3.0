using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Modes;
using System.Drawing;

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
      mode_btn.ImageScaling = ToolStripItemImageScaling.None;
      mode_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      AddDropDownItem(mode_btn.DropDownItems, ContextMenuResources.Str_ModeHierarchy, ContextMenuResources.hierarchy_mode, mode_btn_click, typeof(Outliner.Modes.Hierarchy.HierarchyMode));
      AddDropDownItem(mode_btn.DropDownItems, ContextMenuResources.Str_ModeFlat, ContextMenuResources.flat_object_list_mode_dark, mode_btn_click, typeof(Outliner.Modes.FlatList.FlatObjectListMode));
      AddDropDownItem(mode_btn.DropDownItems, ContextMenuResources.Str_ModeLayer, ContextMenuResources.layer_mode, mode_btn_click, typeof(Outliner.Modes.Layer.LayerMode));
      AddDropDownItem(mode_btn.DropDownItems, ContextMenuResources.Str_ModeSelSet, ContextMenuResources.selection_set_mode_dark, mode_btn_click, typeof(Outliner.Modes.SelectionSet.SelectionSetMode));
      strip.Items.Add(mode_btn);

      ToolStripCheckedSplitButton filter_btn = new ToolStripCheckedSplitButton(ContextMenuResources.filter_32);
      filter_btn.ImageScaling = ToolStripItemImageScaling.None;
      filter_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      filter_btn.Checked = treeMode.Filters.Enabled;
      filter_btn.ButtonClick += new EventHandler(filter_btn_ButtonClick);
      filter_btn.DropDownItems.Add("Bones");
      filter_btn.DropDownItems.Add("Geometry");
      strip.Items.Add(filter_btn);

      //ToolStripButton sort_btn = new ToolStripButton();
      //sort_btn.Checked = true;
      //strip.Items.Add(sort_btn);

      strip.Items.Add(new ToolStripSeparator());

      ToolStripDropDownButton window_btn = new ToolStripDropDownButton();
      window_btn.Image = ContextMenuResources.window;
      window_btn.DropDownDirection = ToolStripDropDownDirection.BelowRight;
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSingle, ContextMenuResources.window, window_click);
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSplitHor, ContextMenuResources.window_split_hor, split_hor_btn_Click);
      window_btn.DropDownItems.Add(ContextMenuResources.Str_WindowSplitVer, ContextMenuResources.window_split_ver, split_ver_btn_Click);
      strip.Items.Add(window_btn);

      //ToolStripTextBox textFilter = new ToolStripTextBox();
      //textFilter.GotFocus += new EventHandler(textFilter_GotFocus);
      //textFilter.LostFocus += new EventHandler(textFilter_LostFocus);
      //textFilter.TextChanged += new EventHandler(textFilter_TextChanged);
      //strip.Items.Add(textFilter);

      return strip;
   }



   private static ToolStripItem AddDropDownItem(ToolStripItemCollection itemCollection, String text, Image img, EventHandler clickHandler, Object tag)
   {
      ToolStripItem item = itemCollection.Add(text, img, clickHandler);
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


   static void filter_btn_ButtonClick(object sender, EventArgs e)
   {
      TreeMode treeMode = GetTreeMode(sender);
      treeMode.Filters.Enabled = !treeMode.Filters.Enabled;
   }


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



   static void textFilter_TextChanged(object sender, EventArgs e)
   {
      TreeMode mode = GetTreeMode(sender);
      ToolStripTextBox textBox = sender as ToolStripTextBox;
      Type filterType = typeof(Outliner.Filters.NameFilter);
      if (textBox.TextLength == 0)
         mode.Filters.Remove(filterType);
      else
      {
         Outliner.Filters.NameFilter filter = mode.Filters.Get(filterType) as Outliner.Filters.NameFilter;
         if (filter == null)
         {
            filter = new Filters.NameFilter();
            mode.Filters.Add(filter);
         }
         filter.SearchString = textBox.Text;
      }
   }

   static void textFilter_LostFocus(object sender, EventArgs e)
   {
      MaxUtils.MaxInterfaces.Global.EnableAccelerators();
   }

   static void textFilter_GotFocus(object sender, EventArgs e)
   {
      MaxUtils.MaxInterfaces.Global.DisableAccelerators();
   }
}
}
