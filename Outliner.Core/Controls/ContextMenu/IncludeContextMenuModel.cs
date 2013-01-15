using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using Outliner.Scene;
using Outliner.Plugins;

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

      protected override void OnClick( Outliner.Controls.Tree.TreeView treeView
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
                  ContextMenuModel contextMenu = XmlSerializationHelpers.Deserialize<ContextMenuModel>(file);
                  return contextMenu.Items;
               }
               catch (InvalidOperationException e) { }
            }
            return new List<MenuItemModel>();
         }
         set { }
      }
   }
}
