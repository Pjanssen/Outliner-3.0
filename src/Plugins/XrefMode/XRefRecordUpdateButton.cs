using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.TreeNodeButtons;
using WinForms = System.Windows.Forms;

namespace PJanssen.Outliner.Modes.XRef
{
   [OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
   public class XRefRecordUpdateButton : ImageButton
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

      public XRefRecordUpdateButton() : base(GetButtonImages(Resources.reload)) { }

      public override TreeNodeLayoutItem Copy()
      {
         return new XRefRecordUpdateButton();
      }

      public override bool IsEnabled(TreeNode tn)
      {
         return true;
      }

      protected override string GetTooltipText(TreeNode tn)
      {
         return Resources.Tooltip_Update;
      }

      public override void HandleMouseDown(WinForms.MouseEventArgs e, TreeNode tn)
      {
         IEnumerable<IXRefRecord> records = TreeMode.GetMaxNodes(this.GetContextNodes(tn))
                                                    .OfType<IXRefRecord>()
                                                    .ToList();

         foreach (IXRefRecord record in records)
         {
            record.Update();
         }
      }
   }
}
