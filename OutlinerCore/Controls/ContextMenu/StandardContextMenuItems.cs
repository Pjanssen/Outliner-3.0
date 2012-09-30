using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Commands;
using System.Drawing;
using Outliner.Scene;
using OutlinerTree = Outliner.Controls.Tree;
using MaxUtils;

namespace Outliner.Controls.ContextMenu
{
public static class StandardContextMenuItems
{
   public static ToolStripItem[] HideFreezeItems(IEnumerable<IMaxNodeWrapper> selectedNodes)
   {
      ToolStripItem[] items = new ToolStripItem[4];

      items[0] = createItem(ContextMenuResources.Str_HideSelection, ContextMenuResources.hide, new HideCommand(selectedNodes, true));
      items[0].Enabled = selectedNodes.Any(n => !n.IsHidden);

      items[1] = createItem(ContextMenuResources.Str_UnhideSelection, null, new HideCommand(selectedNodes, false));
      items[1].Enabled = selectedNodes.Any(n => n.IsHidden);

      items[2] = createItem(ContextMenuResources.Str_FreezeSelection, ContextMenuResources.freeze, new FreezeCommand(selectedNodes, true));
      items[2].Enabled = selectedNodes.Any(n => !n.IsFrozen);

      items[3] = createItem(ContextMenuResources.Str_UnfreezeSelection, null, new FreezeCommand(selectedNodes, false));
      items[3].Enabled = selectedNodes.Any(n => n.IsFrozen);

      return items;
   }


   public static ToolStripItem[] AddSelectionToItems(IEnumerable<IMaxNodeWrapper> selectedNodes)
   {
      ToolStripItem[] items = new ToolStripItem[1];

      ToolStripMenuItem item = createItem(ContextMenuResources.Str_AddSelectionTo, null, null);
      item.DropDownItems.Add(createItem(ContextMenuResources.Str_AddToNewContainer, ContextMenuResources.newcontainer, null));
      item.DropDownItems.Add(createItem(ContextMenuResources.Str_AddToNewGroup, ContextMenuResources.newgroup, null));
      item.DropDownItems.Add(createItem(ContextMenuResources.Str_AddToSelSet, null, null));
      item.DropDownItems.Add(createItem(ContextMenuResources.Str_AddToNewLayer, ContextMenuResources.newlayer, null));
      item.DropDownItems.Add(new ToolStripSeparator());

      Autodesk.Max.IILayerManager layerManager = MaxInterfaces.IILayerManager;
      for (int i = 0; i < layerManager.LayerCount; i++)
      {
         Autodesk.Max.IILayer layer = layerManager.GetLayer(i);
         item.DropDownItems.Add(createItem(layer.Name, ContextMenuResources.layer, null));
      }

      items[0] = item;

      return items;
   }


   public static ToolStripItem[] RenameDeleteItems( IEnumerable<IMaxNodeWrapper> selectedNodes
                                                  , OutlinerTree::TreeNode clickedNode)
   {
      ToolStripItem[] items = new ToolStripItem[2];

      ToolStripItem renameItem = new ToolStripMenuItem(ContextMenuResources.Str_Rename, ContextMenuResources.rename, renameItemClick);
      renameItem.Tag = clickedNode;
      items[0] = renameItem;
      items[1] = createItem(ContextMenuResources.Str_Delete, ContextMenuResources.delete, new DeleteCommand(selectedNodes));

      return items;
   }


   private static ToolStripMenuItem createItem(String text, Image image, Command cmd)
   {
      ToolStripMenuItem item = new ToolStripMenuItem(text, image, StandardContextMenuItemClick);
      item.Tag = cmd;
      return item;
   }

   public static void StandardContextMenuItemClick(object sender, EventArgs e)
   {
      ToolStripItem item = sender as ToolStripItem;
      if (item == null)
         return;

      Command cmd = item.Tag as Command;
      if (cmd == null)
         return;
      
      cmd.Execute(true);
   }

   private static void renameItemClick(object sender, EventArgs e)
   {
      ToolStripItem item = sender as ToolStripItem;
      if (item == null)
         return;

      OutlinerTree::TreeNode treeNode = item.Tag as OutlinerTree::TreeNode;
      if (treeNode == null)
         return;

      treeNode.TreeView.BeginNodeTextEdit(treeNode);
   }


   public static ToolStripItem[] PropertiesItems()
   {
      ToolStripItem[] items = new ToolStripItem[3];

      items[0] = new ToolStripMenuItem("Display Settings");
      items[1] = new ToolStripMenuItem("Render Settings");
      items[2] = new ToolStripMenuItem("Object Properties", ContextMenuResources.properties, objectPropertiesClicked);

      return items;
   }

   private static void objectPropertiesClicked(object sender, EventArgs e)
   {
      if (true) //TODO Switch use properties tool setting
         ManagedServices.MaxscriptSDK.ExecuteMaxscriptCommand("macros.run \"DW Tools\" \"DWObjProps\"");
      else
         ManagedServices.MaxscriptSDK.ExecuteMaxscriptCommand("max properties");
         
   }
}
}
