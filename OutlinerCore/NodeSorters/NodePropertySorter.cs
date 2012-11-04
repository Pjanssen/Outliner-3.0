using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.MaxUtils;
using Outliner.Controls.Tree;
using Outliner.Scene;
using System.Drawing;
using Outliner.Plugins;

namespace Outliner.NodeSorters
{
public abstract class NodePropertySorter : NodeSorter
{
   public NodeProperty Property { get; set; }

   public NodePropertySorter() : base() { }
   public NodePropertySorter(Boolean invert) : base(invert) { }
   public NodePropertySorter(NodeProperty property) : this(property, false) { }
   public NodePropertySorter(NodeProperty property, Boolean invert) : base(invert)
   {
      this.Property = property;
   }


   protected override int InternalCompare(TreeNode x, TreeNode y)
   {
      if (x == y)
         return 0;

      IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
      if (nodeX == null || !nodeX.IsValid) return 0;

      IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
      if (nodeY == null || !nodeY.IsValid) return 0;

      Object propValueX = nodeX.GetProperty(this.Property);
      Object propValueY = nodeY.GetProperty(this.Property);

      if (propValueX.Equals(propValueY))
         return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
      else
      {
         IComparable iCompX = propValueX as IComparable;
         if (iCompX != null)
            return iCompX.CompareTo(propValueY);
         else if (propValueX is Color && propValueY is Color)
            return ColorHelpers.Compare((Color)propValueX, (Color)propValueY);
         else
            return 0;
      }
   }
}
}
