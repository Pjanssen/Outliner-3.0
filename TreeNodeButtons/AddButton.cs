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
public class AddButton : ImageButton
{
   public AddButton()
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Add)) { }


   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeTypes.All)]
   public override MaxNodeTypes VisibleTypes
   {
      get { return base.VisibleTypes & (MaxNodeTypes.SelectionSet | MaxNodeTypes.Layer); }
      set { base.VisibleTypes = value; }
   }


   public override bool IsEnabled(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return false;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      IEnumerable<TreeNode> selTreeNodes = this.Layout.TreeView.SelectedNodes;
      if (node == null || selTreeNodes.Count() == 0)
         return false;

      return node.CanAddChildNodes(HelperMethods.GetMaxNodes(selTreeNodes));
   }

   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (!this.IsEnabled(tn))
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      IEnumerable<IMaxNodeWrapper> nodes = HelperMethods.GetMaxNodes(this.Layout.TreeView.SelectedNodes);
      Command cmd;
      if (node is SelectionSetWrapper)
      {
         SelectionSetWrapper selSet = (SelectionSetWrapper)node;
         IEnumerable<IMaxNodeWrapper> newNodes = selSet.WrappedChildNodes.Union(nodes);
         cmd = new ModifySelectionSetCommand(newNodes.ToList(), selSet);
      }
      else
      {
         cmd = new MoveMaxNodeCommand(nodes, node, Resources.Command_AddToLayer, Resources.Command_UnlinkLayer);
      }

      cmd.Execute(true);
   }


   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null || !this.IsEnabled(tn))
         return null;

      if (node is IILayerWrapper)
         return Resources.Tooltip_Add_Layer;
      else if (node is SelectionSetWrapper)
         return Resources.Tooltip_Add_SelSet;
      else
         return null;
   }


   public override TreeNodeLayoutItem Copy()
   {
      AddButton newItem = new AddButton();

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
