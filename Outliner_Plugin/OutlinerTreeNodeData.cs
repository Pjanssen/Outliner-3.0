using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner
{
   public class OutlinerTreeNodeData : TreeNodeData
   {
      public IMaxNodeWrapper Node { get; private set; }

      public OutlinerTreeNodeData(TreeNode tn, IMaxNodeWrapper node) : base(tn)
      {
         this.Node = node;
      }
   }
}
