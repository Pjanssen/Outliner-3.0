using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.PolyCount
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
