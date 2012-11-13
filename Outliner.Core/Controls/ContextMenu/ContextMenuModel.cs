using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
   /// <summary>
   /// An Xml-serializable model for a ContextMenu.
   /// </summary>
   [XmlRoot("ContextMenu")]
   public class ContextMenuModel
   {
      public ContextMenuModel()
      {
         this.Items = new List<MenuItemModel>();
      }

      [XmlArray("Items")]
      [XmlArrayItem("MenuItem")]
      public List<MenuItemModel> Items { get; set; }


      public ContextMenuStrip ToContextMenuStrip( Outliner.Controls.Tree.TreeNode clickedTn
                                                , IEnumerable<IMaxNodeWrapper> context)
      {
         ContextMenuStrip strip = new ContextMenuStrip();

         foreach (MenuItemModel item in this.Items)
         {
            strip.Items.Add(item.ToToolStripMenuItem(clickedTn, context));
         }

         return strip;
      }
   }
}
