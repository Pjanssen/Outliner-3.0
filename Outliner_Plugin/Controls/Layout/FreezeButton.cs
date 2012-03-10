using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Layout
{
   public class FreezeButton : ImageButton
   {
      public FreezeButton() : base(OutlinerResources.freeze_button,
                                   OutlinerResources.freeze_button_disabled)
      { }

      public override bool IsEnabled(TreeNode tn)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (node == null)
            return false;

         return node.IsFrozen;
      }

      public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (node != null)
         {
            FreezeCommand cmd = new FreezeCommand(new List<Autodesk.Max.IINode>() { (Autodesk.Max.IINode)node.UnderlyingNode }, !node.IsFrozen);
            cmd.Execute(true);
         }
      }
   }
}
