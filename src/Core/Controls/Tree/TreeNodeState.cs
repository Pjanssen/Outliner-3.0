using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree
{
   /// <summary>
   /// Specifies the state of a TreeNode.
   /// </summary>
   [Flags]
   public enum TreeNodeStates : int
   {
      /// <summary>
      /// The default, unhighlighted state.
      /// </summary>
      None             = 0x00,

      /// <summary>
      /// Specifies that the TreeNode is selected.
      /// </summary>
      Selected         = 0x01,

      /// <summary>
      /// Specifies that the TreeNode is a parent of a selected node.
      /// </summary>
      ParentOfSelected = 0x02,

      /// <summary>
      /// Specifies that the TreeNode is currently a drop target in a drag &amp; drop target.
      /// </summary>
      DropTarget       = 0x04
   }
}
