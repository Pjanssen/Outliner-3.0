using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.Plugins;
using Outliner.Scene;
using Outliner.TreeNodeButtons;
using WinForms = System.Windows.Forms;

namespace Outliner.Modes.XRefMode
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
