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
using MaxUtils;
using Outliner.Filters;
using Outliner.Commands;
using WinForms = System.Windows.Forms;
using Outliner.NodeSorters;
using Outliner.Controls.Tree.Layout;
using Outliner.Controls.Tree;

namespace Outliner.TreeNodeButtons
{
public abstract class AnimatablePropertyButton : ImageButton
{
   protected AnimatablePropertyButton() { }
   protected AnimatablePropertyButton(ButtonImages images)
      : base(images)
   {
      ButtonImages layerImages = NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Layer);
      imageByLayer = layerImages.Regular;
      imageByLayer_Filtered = layerImages.RegularFiltered;
   }

   protected abstract AnimatableProperty Property { get; }
   protected virtual SetNodePropertyCommand<Boolean> CreateCommand(IEnumerable<IMaxNodeWrapper> nodes, Boolean newValue)
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
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      if (node is IILayerWrapper)
      {
         IILayer layer = ((IILayerWrapper)node).IILayer;
         if (!AnimatablePropertyHelpers.IsBooleanProperty(this.Property))
            return false;
         return NestedLayers.IsPropertyInherited(layer, AnimatablePropertyHelpers.ToBooleanProperty(this.Property));
      }
      else if (node is IINodeWrapper)
      {
         IINodeWrapper inode = (IINodeWrapper)node;
         switch (this.Property)
         {
            case AnimatableProperty.BoxMode: return inode.NodeLayerProperties.DisplayByLayer;
            case AnimatableProperty.XRayMtl: return inode.NodeLayerProperties.DisplayByLayer;
            case AnimatableProperty.Renderable: return inode.NodeLayerProperties.RenderByLayer;
            case AnimatableProperty.IsHidden: return inode.IILayer != null && inode.IILayer.IsHidden;
            case AnimatableProperty.IsFrozen: return inode.IILayer != null && inode.IILayer.IsFrozen;
            case AnimatableProperty.WireColor: return inode.NodeLayerProperties.ColorByLayer;
            case AnimatableProperty.Name: return false;
            default: return false;
         }
      }
      else
         return false;
   }

   override public Boolean IsEnabled(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      if (!AnimatablePropertyHelpers.IsBooleanProperty(this.Property))
         return true;

      return (node.GetProperty(AnimatablePropertyHelpers.ToBooleanProperty(this.Property)) != this.InvertBehavior);
   }


   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      if (this.isInheritedFromLayer(tn))
      {
         Image img = (!tn.ShowNode) ? this.ImageByLayer_Filtered : this.ImageByLayer;
         this.DrawImage(graphics, tn, img);
      }
      else
         base.Draw(graphics, tn);
   }



   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return base.GetTooltipText(tn);

      if (this.isInheritedFromLayer(tn))
         return this.Property.ToString() + " " + Resources.Tooltip_ByLayer;
      if (this.IsEnabled(tn))
         return this.ToolTipEnabled;
      else
         return this.ToolTipDisabled;
   }

   protected override bool Clickable(TreeNode tn)
   {
      return !this.isInheritedFromLayer(tn);
   }




   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (!AnimatablePropertyHelpers.IsBooleanProperty(this.Property))
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      TreeView tree = this.Layout.TreeView;
      IEnumerable<TreeNode> nodes = null;
      if (tree.IsSelectedNode(tn) && !HelperMethods.ControlPressed)
         nodes = tree.SelectedNodes;
      else
         nodes = new List<TreeNode>(1) { tn };

      Boolean nodeValue = node.GetProperty(AnimatablePropertyHelpers.ToBooleanProperty(this.Property));
      IEnumerable<IMaxNodeWrapper> maxNodes = HelperMethods.GetMaxNodes(nodes);
      SetNodePropertyCommand<Boolean> cmd = this.CreateCommand(maxNodes, !nodeValue);
      if (cmd != null)
         cmd.Execute(true);

      if (tree.NodeSorter is AnimatablePropertySorter &&
            ((AnimatablePropertySorter)tree.NodeSorter).Property == this.Property)
      {
         tree.StartTimedSort(nodes);
      }
   }
}
}
