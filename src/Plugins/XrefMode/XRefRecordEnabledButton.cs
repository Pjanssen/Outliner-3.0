using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PJanssen.Outliner.Controls;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.TreeNodeButtons;

namespace PJanssen.Outliner.Modes.XRef
{
   [OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
   public class XRefRecordEnabledButton : ImageButton
   {
      protected static ButtonImages images;
      protected static ButtonImages GetButtonImages(Image baseImage)
      {
         if (images.Regular == null)
         {
            images = NodeButtonImages.CreateImages(baseImage);
         }

         return images;
      }

      public XRefRecordEnabledButton() : base(GetButtonImages(Resources.check)) { }

      public override TreeNodeLayoutItem Copy()
      {
         return new XRefRecordEnabledButton();
      }

      public override bool IsEnabled(Outliner.Controls.Tree.TreeNode tn)
      {
         IXRefRecord xrefRecord = TreeMode.GetMaxNode(tn) as IXRefRecord;
         if (xrefRecord == null)
            return false;
         else
            return xrefRecord.Enabled;
      }

      protected override bool Clickable(TreeNode tn)
      {
         IXRefRecord xrefRecord = TreeMode.GetMaxNode(tn) as IXRefRecord;
         return xrefRecord != null;
      }

      protected override string GetTooltipText(TreeNode tn)
      {
         IXRefRecord xrefRecord = TreeMode.GetMaxNode(tn) as IXRefRecord;
         if (xrefRecord == null)
            return null;

         if (xrefRecord.Enabled)
            return Resources.Tooltip_Disable;
         else
            return Resources.Tooltip_Enable;
      }

      public override void HandleMouseDown(System.Windows.Forms.MouseEventArgs e, TreeNode tn)
      {
         base.HandleMouseDown(e, tn);

         IXRefRecord xrefRecord = TreeMode.GetMaxNode(tn) as IXRefRecord;
         if (xrefRecord == null)
            return;

         Boolean newValue = !xrefRecord.Enabled;

         IEnumerable<IMaxNode> nodes = TreeMode.GetMaxNodes(this.GetContextNodes(tn))
                                               .ToList();

         foreach (IMaxNode maxNode in nodes)
         {
            IXRefRecord record = maxNode as IXRefRecord;
            if (record != null)
               record.Enabled = newValue;
         }
      }
   }
}
