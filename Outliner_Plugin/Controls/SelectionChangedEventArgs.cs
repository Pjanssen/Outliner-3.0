using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls
{
   public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
   public class SelectionChangedEventArgs : EventArgs
   {
      public ICollection<TreeNode> Nodes { get; set; }

      public SelectionChangedEventArgs(ICollection<TreeNode> nodes)
      {
         this.Nodes = nodes;
      }
   }
}
