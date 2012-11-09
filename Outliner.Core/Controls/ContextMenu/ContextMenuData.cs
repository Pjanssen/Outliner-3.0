using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
   [XmlRoot("ContextMenu")]
   public class ContextMenuData
   {
      public ContextMenuData()
      {
         this.Items = new List<MenuItemData>();
      }

      [XmlArray("Items")]
      [XmlArrayItem("MenuItem")]
      public List<MenuItemData> Items { get; set; }


      public ContextMenuStrip ToContextMenuStrip( Outliner.Controls.Tree.TreeNode clickedTn
                                                , IEnumerable<IMaxNodeWrapper> context)
      {
         ContextMenuStrip strip = new ContextMenuStrip();

         foreach (MenuItemData item in this.Items)
         {
            strip.Items.Add(item.ToToolStripMenuItem(clickedTn, context));
         }

         return strip;
      }
   }
}
