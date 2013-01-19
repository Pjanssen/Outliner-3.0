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

namespace Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
public class RemoveButton : ImageButton
{
   public RemoveButton()
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Remove)) { }

   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeType.SelectionSet)]
   public override MaxNodeType VisibleTypes
   {
      get { return base.VisibleTypes & (MaxNodeType.SelectionSet); }
      set { base.VisibleTypes = value; }
   }

   public override bool IsEnabled(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return false;

      SelectionSetWrapper node = HelperMethods.GetMaxNode(tn) as SelectionSetWrapper;
      IEnumerable<TreeNode> selTreeNodes = this.Layout.TreeView.SelectedNodes;
      if (node == null || selTreeNodes.Count() == 0)
         return false;

      return node.CanRemoveChildNodes(HelperMethods.GetMaxNodes(selTreeNodes));
   }

   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (!this.IsEnabled(tn))
         return;

      SelectionSetWrapper selSet = HelperMethods.GetMaxNode(tn) as SelectionSetWrapper;
      if (selSet == null)
         return;

      IEnumerable<IMaxNode> selNodes = HelperMethods.GetMaxNodes(this.Layout.TreeView.SelectedNodes);
      IEnumerable<IMaxNode> newNodes = selSet.ChildNodes.Except(selNodes);
      ModifySelectionSetCommand cmd = new ModifySelectionSetCommand(newNodes, selSet);
      cmd.Execute(true);
   }


   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null || !this.IsEnabled(tn))
         return null;

      return Resources.Tooltip_Remove_SelSet;
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
