using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Controls.Tree;

namespace Outliner.NodeSorters
{
   public class FrozenSorter : IComparer<TreeNode>
   {
      public int Compare(TreeNode x, TreeNode y)
      {
         IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         if (nodeX.IsFrozen == nodeY.IsFrozen)
            return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
         else if (nodeX.IsFrozen)
            return 1;
         else
            return -1;
      }
   }
}
