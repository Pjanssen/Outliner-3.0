using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree
{
   public class TreeNodeTextSorter : IComparer<TreeNode>
   {
      public int Compare(TreeNode x, TreeNode y)
      {
         if (x == null || y == null)
            return 0;

         return NativeMethods.StrCmpLogicalW(x.Text, y.Text);
      }
   }
}
