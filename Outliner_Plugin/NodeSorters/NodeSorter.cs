using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;

namespace Outliner.NodeSorters
{
   public abstract class NodeSorter : IComparer<TreeNode>
   {
      protected NodeSorter() : this(false) { }
      protected NodeSorter(Boolean invert)
      {
         this.invert = invert;
      }

      private Boolean invert;
      public Boolean Ascending { get { return !this.invert; } }
      public Boolean Descending { get { return this.invert; } }

      public int Compare(TreeNode x, TreeNode y)
      {
         ExceptionHelper.ThrowIfArgumentIsNull(x, "x");
         ExceptionHelper.ThrowIfArgumentIsNull(y, "y");

         if (invert)
            return this.InternalCompare(y, x);
         else
            return this.InternalCompare(x, y);
      }

      protected abstract int InternalCompare(TreeNode x, TreeNode y);
   }
}
