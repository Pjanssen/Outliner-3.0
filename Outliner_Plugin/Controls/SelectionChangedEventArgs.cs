using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Outliner.Controls
{
   public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
   public class SelectionChangedEventArgs : EventArgs
   {
      public IEnumerable<TreeNode> Nodes { get; set; }

      public SelectionChangedEventArgs(IEnumerable<TreeNode> nodes)
      {
         this.Nodes = nodes;
      }
   }
}
