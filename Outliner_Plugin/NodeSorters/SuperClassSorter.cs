using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Autodesk.Max;

namespace Outliner.NodeSorters
{
   public class SuperClassSorter : IComparer<TreeNode>
   {
      public int Compare(TreeNode x, TreeNode y)
      {
         IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         SClass_ID sClassIDX = nodeX.SuperClassID;
         SClass_ID sClassIDY = nodeY.SuperClassID;

         if (sClassIDX == sClassIDY)
            return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
         else
            return (int)(sClassIDX - sClassIDY);
      }
   }
}
