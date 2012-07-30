using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxUtils;
using Outliner.Controls.Tree;
using Outliner.Scene;

namespace Outliner.NodeSorters
{
public class AnimatablePropertySorter : NodeSorter
{
   public AnimatableProperty Property { get; private set; }

   public AnimatablePropertySorter() : base() { }
   public AnimatablePropertySorter(Boolean invert) : base(invert) { }
   public AnimatablePropertySorter(AnimatableProperty property) : this(property, false) { } 
   public AnimatablePropertySorter(AnimatableProperty property, Boolean invert) : base(invert)
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

      if (propValueX == propValueY)
         return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
      else if (propValueX is IComparable)
         return ((IComparable)propValueX).CompareTo(propValueY);
      else
         return 0;
   }
}
}
