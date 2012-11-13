using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
   public class IncludeContextMenuModel : MenuItemModel
   {
      public IncludeContextMenuModel() : base() 
      {
         this.File = String.Empty;
      }

      [XmlAttribute("file")]
      public String File { get; set; }

      public override void OnClick( Outliner.Controls.Tree.TreeNode clickedTn
                                  , IEnumerable<IMaxNodeWrapper> context)
      {
         //IncludeContextMenuData does not execute anything.
      }

      public override List<MenuItemModel> SubItems
      {
         get
         {
            String file = this.File;
            if (!Path.IsPathRooted(file))
               file = Path.Combine(OutlinerPaths.ContextMenuDir, file);

            if (System.IO.File.Exists(file))
            {
               try
               {
                  ContextMenuModel contextMenu = XmlSerializationHelpers<ContextMenuModel>.FromXml(file);
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
