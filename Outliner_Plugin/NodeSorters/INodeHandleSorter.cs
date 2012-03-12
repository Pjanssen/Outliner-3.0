using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Autodesk.Max;

namespace Outliner.NodeSorters
{
   public class INodeHandleSorter : IComparer<TreeNode>
   {
      public int Compare(TreeNode x, TreeNode y)
      {
         IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         Boolean xIsIINodeWrapper = nodeX is IINodeWrapper;
         Boolean yIsIINodeWrapper = nodeY is IINodeWrapper;

         if (!xIsIINodeWrapper && !yIsIINodeWrapper)
            return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
         else if (!xIsIINodeWrapper)
            return -1;
         else if (!yIsIINodeWrapper)
            return 1;
         else
            return (int)(((IINode)nodeX.WrappedNode).Handle - ((IINode)nodeY.WrappedNode).Handle);
      }
   }
}
