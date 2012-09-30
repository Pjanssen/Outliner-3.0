using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree
{
   public class SelectionChangedEventArgs : EventArgs
   {
      public IEnumerable<TreeNode> Nodes { get; set; }

      public SelectionChangedEventArgs(IEnumerable<TreeNode> nodes)
      {
         this.Nodes = nodes;
      }
   }
}
