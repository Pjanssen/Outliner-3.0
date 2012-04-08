using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Scene;

namespace Outliner.NodeSorters
{
   public class HiddenSorter : NodeSorter
   {
      public HiddenSorter() : base() { }
      public HiddenSorter(Boolean invert) : base(invert) { }

      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         if (x == y)
            return 0;

         IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         if (nodeX.IsHidden == nodeY.IsHidden)
            return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
         else if (nodeX.IsHidden)
            return 1;
         else
            return -1;
      }
   }
}
