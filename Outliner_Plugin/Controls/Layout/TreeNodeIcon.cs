using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Outliner.Controls.FiltersBase;
using System.Resources;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner.Controls.Layout
{
public class TreeNodeIcon : TreeNodeLayoutItem
{
   private Dictionary<String, Bitmap> icons;
   private IconSet iconSet;
   private Boolean invert;

   [XmlAttribute("iconSet")]
   public IconSet IconSet
   {
      get { return this.iconSet; }
      set
      {
         this.iconSet = value;
         this.icons = IconHelperMethods.CreateIconSetBitmaps(value, this.invert);
      }
   }

   [XmlAttribute("invert")]
   [DefaultValue(true)]
   public Boolean Invert
   {
      get { return this.invert; }
      set
      {
         this.invert = value;
         this.icons = IconHelperMethods.CreateIconSetBitmaps(this.iconSet, value);
      }
   }

   public TreeNodeIcon() { }

   public TreeNodeIcon(Dictionary<String, Bitmap> icons) 
   {
      this.icons = icons;
   }

   public TreeNodeIcon(ResourceSet resSet, Boolean invert) 
   {
      this.icons = IconHelperMethods.CreateIconSetBitmaps(resSet, invert);
      this.invert = invert;
   }

   public TreeNodeIcon(IconSet iconSet, Boolean invert)
   {
      this.icons = IconHelperMethods.CreateIconSetBitmaps(iconSet, invert);
      this.invert = invert;
   }


   public override Size GetSize(TreeNode tn)
   {
      if (this.Layout == null || this.icons == null || this.icons.Count == 0)
         return Size.Empty;

      return this.icons.First().Value.Size;
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      if (this.Layout == null || this.icons == null)
         return;

      Bitmap icon = null;
      String iconKey = tn.ImageKey;

      TreeNodeData data = tn.Tag as TreeNodeData;
      if (data != null && data.FilterResult == FilterResult.ShowChildren)
         iconKey += "_filtered";

      if (!this.icons.TryGetValue(iconKey, out icon))
      {
         if (!this.icons.TryGetValue("unknown", out icon))
            return;
      }

      g.DrawImage(icon, this.GetBounds(tn));
   }

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn) { }
}
}
