using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Outliner.Controls;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.MaxUtils;
using Outliner.Modes;
using Outliner.Modes.XRefMode.Commands;
using Outliner.Plugins;
using Outliner.Scene;
using Outliner.TreeNodeButtons;

namespace Outliner.Modes.XRefMode
{
   [OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
   public class XRefRecordAutoUpdateButton : ImageButton
   {
      private static ButtonImages images;
      private static ButtonImages GetButtonImages()
      {
         if (images.Regular == null)
         {
            images = NodeButtonImages.CreateImages(Resources.reload);
         }

         return images;
      }

      public XRefRecordAutoUpdateButton() : base(GetButtonImages()) { }

      public override TreeNodeLayoutItem Copy()
      {
         return new XRefRecordAutoUpdateButton();
      }

      [XmlAttribute("flag")]
      public XRefSceneFlags Flags { get; set; }

      public override bool IsEnabled(Outliner.Controls.Tree.TreeNode tn)
      {
         XRefSceneRecord xrefScene = TreeMode.GetMaxNode(tn) as XRefSceneRecord;
         if (xrefScene == null)
            return false;
         else
            return xrefScene.HasFlags(this.Flags);
      }

      protected override bool Clickable(TreeNode tn)
      {
         XRefSceneRecord xrefScene = TreeMode.GetMaxNode(tn) as XRefSceneRecord;
         return xrefScene != null;
      }

      protected override string GetTooltipText(TreeNode tn)
      {
         XRefSceneRecord xrefScene = TreeMode.GetMaxNode(tn) as XRefSceneRecord;
         if (xrefScene == null)
            return null;

         return Resources.Tooltip_AutoUpdate;
      }

      public override void HandleMouseDown(System.Windows.Forms.MouseEventArgs e, TreeNode tn)
      {
         base.HandleMouseDown(e, tn);

         XRefSceneRecord xrefScene = TreeMode.GetMaxNode(tn) as XRefSceneRecord;
         if (xrefScene == null)
            return;

         Boolean newValue = !xrefScene.HasFlags(this.Flags);

         IEnumerable<IMaxNode> nodes = TreeMode.GetMaxNodes(this.GetContextNodes(tn));

         SetXRefSceneFlagsCommand cmd = new SetXRefSceneFlagsCommand(nodes, this.Flags, newValue);
         cmd.Execute();
         Viewports.Redraw();
      }
   }
}
