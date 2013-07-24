using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using Outliner.Scene;
using Outliner.Plugins;
using System.ComponentModel;

namespace Outliner.Controls.ContextMenu
{
   [OutlinerPlugin(OutlinerPluginType.ContextMenuItemModel)]
   [LocalizedDisplayName(typeof(ContextMenuResources), "Str_IncludeContextMenuModel")]
   public class IncludeContextMenuModel : MenuItemModel
   {
      public IncludeContextMenuModel() : base() 
      {
         this.File = String.Empty;
      }

      [XmlElement("file")]
      public String File { get; set; }

      [XmlElement("keep_open")]
      [DefaultValue(false)]
      [DisplayName("Keep open")]
      public Boolean KeepOpen { get; set; }

      protected override void OnClick( ToolStripMenuItem clickedItem
                                     , Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
      {
         //IncludeContextMenuData does not execute anything.
      }

      public override List<MenuItemModel> SubItems
      {
         get
         {
            String file = this.File;
            if (!Path.IsPathRooted(file))
               file = Path.Combine(OutlinerPaths.ContextMenusDir, file);

            if (System.IO.File.Exists(file))
            {
               try
               {
                  ContextMenuModel contextMenu = XmlSerialization.Deserialize<ContextMenuModel>(file);
                  return contextMenu.Items;
               }
               catch (InvalidOperationException) { }
            }
            return new List<MenuItemModel>();
         }
         set { }
      }

      public override ToolStripItem[] ToToolStripMenuItems(Tree.TreeView treeView, Tree.TreeNode clickedTn)
      {
         ToolStripItem[] items = base.ToToolStripMenuItems(treeView, clickedTn);

         if (this.KeepOpen)
         {
            foreach (ToolStripItem item in items)
            {
               ToolStripMenuItem menuItem = item as ToolStripMenuItem;
               if (menuItem != null)
                  menuItem.DropDown.Closing += DropDown_Closing;
            }
         }

         return items;
      }

      void DropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
      {
         if ((e.CloseReason & ToolStripDropDownCloseReason.ItemClicked) == ToolStripDropDownCloseReason.ItemClicked)
            e.Cancel = true;
      }
   }
}
