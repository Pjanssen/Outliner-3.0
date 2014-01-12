using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.TreeNodeButtons
{
   [OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
   [LocalizedDisplayName(typeof(Resources), "Str_LockTransformsButton")]
   public class LockTransformsButton : ImageButton
   {
      public LockTransformsButton()
         : base(CreateButtonImages())
      {

      }

      private static ButtonImages CreateButtonImages()
      {
         return NodeButtonImages.GetButtonImages(NodeButtonImages.Images.LockTransform);
      }

      public override TreeNodeLayoutItem Copy()
      {
         LockTransformsButton newItem = new LockTransformsButton();

         newItem.PaddingLeft = this.PaddingLeft;
         newItem.PaddingRight = this.PaddingRight;
         newItem.VisibleTypes = this.VisibleTypes;
         newItem.InvertBehavior = this.InvertBehavior;
         newItem.imageDisabled = this.imageDisabled;
         newItem.imageDisabled_Filtered = this.imageDisabled_Filtered;
         newItem.imageEnabled = this.imageEnabled;
         newItem.imageEnabled_Filtered = this.imageEnabled_Filtered;

         return newItem;
      }

      public override bool IsEnabled(TreeNode tn)
      {
         INodeWrapper node = TreeMode.GetMaxNode(tn) as INodeWrapper;
         if (node == null)
            return false;

         return node.AllTransformsLocked;
      }

      protected override bool Clickable(TreeNode tn)
      {
         return true;
      }

      protected override string GetTooltipText(TreeNode tn)
      {
         if (IsEnabled(tn))
            return Resources.Tooltip_UnlockTransforms;
         else
            return Resources.Tooltip_LockTransforms;
      }

      public override void HandleMouseUp(System.Windows.Forms.MouseEventArgs e, TreeNode tn)
      {
         IMaxNode node = TreeMode.GetMaxNode(tn);
         if (node == null)
            return;

         LockTransformsCommand cmd = new LockTransformsCommand(node.ToIEnumerable(), !IsEnabled(tn));
         cmd.Execute();

         //Todo: check system node events?
         tn.Invalidate();
      }
   }
}
