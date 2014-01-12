using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Controls.Tree
{
   /// <summary>
   /// Provides data for the SelectionChanged event.
   /// </summary>
   public class SelectionChangedEventArgs : EventArgs
   {
      /// <summary>
      /// The new TreeNode selection.
      /// </summary>
      public IEnumerable<TreeNode> Nodes { get; set; }

      internal SelectionChangedEventArgs(IEnumerable<TreeNode> nodes)
      {
         this.Nodes = nodes;
      }
   }
}
