using System;
using System.Collections.Generic;
using System.Drawing;
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
   public class XRefSceneFlagsButton : ImageButton
   {
      public XRefSceneFlagsButton() : base(GetButtonImages(Resources.check)) { }

      protected static ButtonImages images;
      protected static ButtonImages GetButtonImages(Image baseImage)
      {
         if (images.Regular == null)
         {
            images = NodeButtonImages.CreateImages(baseImage);
         }

         return images;
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

      public override TreeNodeLayoutItem Copy()
      {
         return new XRefSceneFlagsButton();
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

         if (xrefScene.Enabled)
            return Resources.Tooltip_Disable;
         else
            return Resources.Tooltip_Enable;
      }

      public override void HandleMouseDown(System.Windows.Forms.MouseEventArgs e, TreeNode tn)
      {
         base.HandleMouseDown(e, tn);

         XRefSceneRecord xrefScene = TreeMode.GetMaxNode(tn) as XRefSceneRecord;
         if (xrefScene == null)
            return;

         Boolean newValue = !xrefScene.HasFlags(this.Flags);

         TreeView tree = this.Layout.TreeView;
         IEnumerable<IMaxNode> nodes = null;
         if (tn.IsSelected && !ControlHelpers.ControlPressed)
            nodes = TreeMode.GetMaxNodes(tree.SelectedNodes);
         else
            nodes = xrefScene.ToIEnumerable<IMaxNode>();

         SetXRefSceneFlagsCommand cmd = new SetXRefSceneFlagsCommand(nodes, this.Flags, newValue);
         cmd.Execute();
         Viewports.Redraw();
      }
   }
}
