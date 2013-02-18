using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree
{
   /// <summary>
   /// Specifies the action to be executed when a TreeNode is double-clicked.
   /// </summary>
   public enum TreeNodeDoubleClickAction
   {
      /// <summary>
      /// Specifies that the TreeNode should be expanded.
      /// </summary>
      Expand,

      /// <summary>
      /// Specifies that the TreeNode should be renamed.
      /// </summary>
      Rename
   }
}
