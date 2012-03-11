using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.TreeModes;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.NodeSorters
{
   public class AlphabeticalSorter : IComparer<TreeNode>
   {
      public int Compare(TreeNode x, TreeNode y)
      {
         IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
      }
   }
}
