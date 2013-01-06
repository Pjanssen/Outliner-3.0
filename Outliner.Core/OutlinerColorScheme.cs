using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Outliner.Controls.ContextMenu;
using Outliner.Controls.Tree;

namespace Outliner
{
   public class OutlinerColorScheme
   {
      [XmlElement("treeview")]
      public TreeViewColorScheme TreeViewColorScheme { get; set; }

      [XmlElement("contextmenu")]
      public ContextMenuColorTable ContextMenuColorTable { get; set; }

      public OutlinerColorScheme()
      {
         this.TreeViewColorScheme = new TreeViewColorScheme();
         this.ContextMenuColorTable = new ContextMenuColorTable();
      }

      public static OutlinerColorScheme Default
      {
         get
         {
            OutlinerColorScheme scheme = new OutlinerColorScheme();
            scheme.TreeViewColorScheme = TreeViewColorScheme.MayaColors;
            scheme.ContextMenuColorTable = new ContextMenuColorTable();
            return scheme;
         }
      }
   }
}
