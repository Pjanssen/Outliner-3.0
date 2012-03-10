using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using System.Windows.Forms;
using Outliner.Controls.FiltersBase;

namespace Outliner.Controls
{
   public class TreeNodeData
   {
      public TreeNode TreeNode { get; private set; }

      public FilterResult FilterResult { get; set; }


      public TreeNodeData(TreeNode tn) 
      {
         this.TreeNode = tn;
         this.FilterResult = FiltersBase.FilterResult.Show;
      }
   }
}
