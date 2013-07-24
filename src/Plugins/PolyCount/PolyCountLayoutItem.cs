using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.MaxUtils;
using Outliner.Modes;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.PolyCount
{
   [OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
   [LocalizedDisplayName(typeof(Resources), "Button_PolyCount")]
   public class PolyCountLayoutItem : TreeNodeText
   {
      public PolyCountLayoutItem() { }

      protected override string GetText(TreeNode tn)
      {
         INodeWrapper iinode = TreeMode.GetMaxNode(tn) as INodeWrapper;
         if (iinode == null)
            return "0";
         else
            return IINodes.GetPolyCount(iinode.INode).ToString();
      }
   }
}
