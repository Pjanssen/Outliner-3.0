using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;
using Autodesk.Max;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner.Controls.Tree.Layout
{
public class AddButton : ImageButton
{
   public AddButton() 
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Add)) { }

   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeTypes.SelectionSet | MaxNodeTypes.Layer)]
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
      ICollection<TreeNode> selTreeNodes = this.Layout.TreeView.SelectedNodes;
      if (node == null || selTreeNodes.Count == 0)
         return false;

      return node.CanAddChildNodes(HelperMethods.GetMaxNodes(selTreeNodes));
   }

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
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
         IEnumerable<IMaxNodeWrapper> newNodes = node.WrappedChildNodes.Union(nodes);
         cmd = new ModifySelectionSetCommand(selSet, newNodes.ToList());
      }
      else
      {
         cmd = new MoveMaxNodeCommand(nodes, node, OutlinerResources.Command_AddToLayer, OutlinerResources.Command_UnlinkLayer);
      }

      cmd.Execute(true);
   }


   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null || !this.IsEnabled(tn))
         return null;

      if (node is IILayerWrapper)
         return OutlinerResources.Tooltip_Add_Layer;
      else if (node is SelectionSetWrapper)
         return OutlinerResources.Tooltip_Add_SelSet;
      else
         return null;
   }
}
}
