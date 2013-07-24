using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
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

      [XmlElement("image_suffix")]
      public String ImageResourceSuffix { get; set; }

      public OutlinerColorScheme()
      {
         this.TreeViewColorScheme = new TreeViewColorScheme();
         this.ContextMenuColorTable = new ContextMenuColorTable();
         this.ImageResourceSuffix = "";
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

      public Image GetImageFromResource(ResourceManager resMan, String resName)
      {
         object res = resMan.GetObject(resName + ImageResourceSuffix, CultureInfo.InvariantCulture);
         if (res == null)
            res = resMan.GetObject(resName, CultureInfo.InvariantCulture);
         if (res == null)
            throw new ArgumentException(resName + " could not be found in resMan");

         Image img = res as Image;
         if (img == null)
            throw new ArgumentException(resName + " is not an Image");

         return img;
      }
   }
}
