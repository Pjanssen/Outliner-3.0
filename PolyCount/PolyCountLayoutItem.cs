using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.MaxUtils;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.PolyCount
{
   [OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
   public class PolyCountLayoutItem : TreeNodeText
   {
      public PolyCountLayoutItem() { }

      protected override string GetText(TreeNode tn)
      {
         IINodeWrapper iinode = HelperMethods.GetMaxNode(tn) as IINodeWrapper;
         if (iinode == null)
            return "0";
         else
            return IINodeHelpers.GetPolyCount(iinode.IINode).ToString();
      }
   }
}
