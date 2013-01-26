using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;
using Autodesk.Max;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Controls.Tree.Layout;
using Outliner.Controls.Tree;
using Outliner.Plugins;
using System.Globalization;

namespace Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(Resources), "Str_RemoveButton")]
public class RemoveButton : ImageButton
{
   public RemoveButton()
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Remove)) { }

   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeType.SelectionSet | MaxNodeType.Material)]
   public override MaxNodeType VisibleTypes
   {
      get { return base.VisibleTypes & (MaxNodeType.SelectionSet | MaxNodeType.Material); }
      set { base.VisibleTypes = value; }
   }

   public override bool ShowForNonMaxNodes
   {
      get { return false; }
   }

   public override Boolean IsVisible(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;
      else
         return node.IsNodeType(this.VisibleTypes);
   }

   public override bool IsEnabled(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return false;

      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      IEnumerable<TreeNode> selTreeNodes = this.Layout.TreeView.SelectedNodes;
      return node.CanRemoveChildNodes(HelperMethods.GetMaxNodes(selTreeNodes));
   }

   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (!this.IsEnabled(tn))
         return;

      IMaxNode target = HelperMethods.GetMaxNode(tn);
      if (target == null)
         return;

      IEnumerable<IMaxNode> nodes = HelperMethods.GetMaxNodes(this.Layout.TreeView.SelectedNodes);
      String description = Resources.Command_RemoveFrom + target.NodeTypeDisplayName;

      RemoveNodesCommand cmd = new RemoveNodesCommand(target, nodes, description);
      cmd.Execute(true);
   }


   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null || !this.IsEnabled(tn))
         return null;

      return Resources.Tooltip_RemoveFrom + node.NodeTypeDisplayName.ToLower(CultureInfo.InvariantCulture);
   }


   public override TreeNodeLayoutItem Copy()
   {
      RemoveButton newItem = new RemoveButton();

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
}
}
