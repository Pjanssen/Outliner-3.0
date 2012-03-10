using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Layout
{
   public class HideButton : ImageButton
   {
      public HideButton() : base(OutlinerResources.hide_button,
                                 OutlinerResources.hide_button_disabled)
      { }

      public override bool IsEnabled(TreeNode tn)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (node == null)
            return false;

         return node.IsHidden;
      }

      public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (node != null)
         {
            HideCommand cmd = new HideCommand(new List<Autodesk.Max.IINode>() { (Autodesk.Max.IINode)node.WrappedNode }, !node.IsHidden);
            cmd.Execute(true);
         }
      }
   }
}
