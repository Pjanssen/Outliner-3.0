using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Layout
{
   public class BoxModeButton : ImageButton
   {
      public BoxModeButton() : base(OutlinerResources.boxmode_button,
                                   OutlinerResources.boxmode_button_disabled)
      { }

      public override bool IsEnabled(TreeNode tn)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (node == null)
            return false;

         return node.BoxMode;
      }

      public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (node != null)
         {
            SetBoxModeCommand cmd = new SetBoxModeCommand(new List<Autodesk.Max.IINode>() { (Autodesk.Max.IINode)node.UnderlyingNode }, !node.BoxMode);
            cmd.Execute(true);
         }
      }
   }
}
