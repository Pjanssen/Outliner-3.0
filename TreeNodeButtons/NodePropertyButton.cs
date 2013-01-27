using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Outliner.Scene;
using Outliner.LayerTools;
using System.Xml.Serialization;
using System.ComponentModel;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Filters;
using Outliner.Commands;
using WinForms = System.Windows.Forms;
using Outliner.NodeSorters;
using Outliner.Controls.Tree.Layout;
using Outliner.Controls.Tree;

namespace Outliner.TreeNodeButtons
{
public abstract class NodePropertyButton : ImageButton
{
   protected NodePropertyButton() 
   {
      ButtonImages layerImages = NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Layer);
      imageByLayer = layerImages.Regular;
      imageByLayer_Filtered = layerImages.RegularFiltered;
   }
   protected NodePropertyButton(ButtonImages images)
      : base(images)
   {
      ButtonImages layerImages = NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Layer);
      imageByLayer = layerImages.Regular;
      imageByLayer_Filtered = layerImages.RegularFiltered;
   }

   protected abstract NodeProperty Property { get; }
   protected virtual SetNodePropertyCommand<Boolean> CreateCommand(IEnumerable<IMaxNode> nodes, Boolean newValue)
   {
      return new SetNodePropertyCommand<Boolean>(nodes, this.Property, newValue);
   }
   protected abstract String ToolTipEnabled { get; }
   protected virtual String ToolTipDisabled { get { return this.ToolTipEnabled; } }

   protected Image imageByLayer;
   protected virtual Image ImageByLayer { get { return imageByLayer; } }
   protected Image imageByLayer_Filtered;
   protected virtual Image ImageByLayer_Filtered
   {
      get
      {
         if (this.imageByLayer_Filtered == null)
         {
            this.imageByLayer_Filtered = ImageButton.CreateFilteredImage(this.ImageByLayer);
         }
         return this.imageByLayer_Filtered;
      }
   }


   protected Boolean isInheritedFromLayer(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      NodeProperty property = this.Property;
      if (node.BaseObject is IILayer)
      {
         IILayer layer = (IILayer)node.BaseObject;
         if (!NodePropertyHelpers.IsBooleanProperty(property))
            return false;
         return NestedLayers.IsPropertyInherited(layer, NodePropertyHelpers.ToBooleanProperty(property));
      }
      else if (node is INodeWrapper)
      {
         INodeWrapper inode = (INodeWrapper)node;
         if (property == NodeProperty.IsHidden)
            return inode.ILayer != null && inode.ILayer.IsHidden;
         else if (property == NodeProperty.IsFrozen)
            return inode.ILayer != null && inode.ILayer.IsFrozen;
         else if (property == NodeProperty.WireColor)
            return inode.NodeLayerProperties.ColorByLayer;
         else if (NodePropertyHelpers.IsDisplayProperty(property))
            return inode.NodeLayerProperties.DisplayByLayer;
         else if (NodePropertyHelpers.IsRenderProperty(property))
            return inode.NodeLayerProperties.RenderByLayer;
         else
            return false;
      }
      else
         return false;
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.isInheritedFromLayer(tn))
         return this.imageByLayer.Height;
      else
         return base.GetHeight(tn);
   }

   public override bool ShowForNonMaxNodes
   {
      get { return false; }
   }

   override public Boolean IsEnabled(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      if (!NodePropertyHelpers.IsBooleanProperty(this.Property))
         return true;

      return !node.GetNodeProperty(this.Property).Equals(this.InvertBehavior);
   }


   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node != null && node.IsNodePropertyInherited(this.Property))
      {
         Image img = (!tn.ShowNode) ? this.ImageByLayer_Filtered : this.ImageByLayer;
         this.DrawImage(graphics, tn, img);
      }
      else
         base.Draw(graphics, tn);
   }


   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return base.GetTooltipText(tn);

      if (node.IsNodePropertyInherited(this.Property))
         return this.Property.ToString() + " " + Resources.Tooltip_ByLayer;
      if (this.IsEnabled(tn))
         return this.ToolTipEnabled;
      else
         return this.ToolTipDisabled;
   }


   protected override bool Clickable(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      return node != null && !node.IsNodePropertyInherited(this.Property);
   }


   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (!NodePropertyHelpers.IsBooleanProperty(this.Property))
         return;

      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      TreeView tree = this.Layout.TreeView;
      IEnumerable<TreeNode> nodes = null;
      if (tn.IsSelected && !HelperMethods.ControlPressed)
         nodes = tree.SelectedNodes;
      else
         nodes = new List<TreeNode>(1) { tn };

      Boolean nodeValue = node.GetNodeProperty(NodePropertyHelpers.ToBooleanProperty(this.Property));
      IEnumerable<IMaxNode> maxNodes = HelperMethods.GetMaxNodes(nodes);
      SetNodePropertyCommand<Boolean> cmd = this.CreateCommand(maxNodes, !nodeValue);
      if (cmd != null)
         cmd.Execute(true);
   }
}
}

