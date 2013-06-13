using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outliner.Controls;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.Plugins;
using Outliner.Scene;
using Outliner.TreeNodeButtons;

namespace Outliner.Modes.XRefMode
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
