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
using System.Windows.Forms;
using Outliner.NodeSorters;

namespace Outliner.Controls.Tree.Layout
{
public abstract class AnimatablePropertyButton : TreeNodeButton
{
   [XmlAttribute("invert_behavior")]
   [DefaultValue(false)]
   public Boolean InvertBehavior { get; set; }

   protected AnimatablePropertyButton() 
   {
      this.InvertBehavior = false;
   }


   protected abstract AnimatableProperty Property { get; }
   protected virtual SetNodePropertyCommand<Boolean> CreateCommand(IEnumerable<IMaxNodeWrapper> nodes, Boolean newValue)
   {
      return new SetNodePropertyCommand<Boolean>(nodes, this.Property, newValue);
   }
   protected abstract String ToolTipEnabled { get; }
   protected virtual String ToolTipDisabled { get { return this.ToolTipEnabled; } }

   protected abstract Bitmap ImageEnabled { get; }
   private Bitmap imageDisabled;
   protected virtual Bitmap ImageDisabled 
   { 
      get 
      {
         if (this.imageDisabled == null)
         {
            this.imageDisabled = ImageButton.CreateDisabledImage(this.ImageEnabled);
         }
         return this.imageDisabled; 
      } 
   }
   protected virtual Bitmap ImageByLayer { get { return OutlinerResources.button_layer; } }
   private Bitmap imageEnabled_Filtered;
   protected virtual Bitmap ImageEnabled_Filtered 
   { 
      get
      {
         if (this.imageEnabled_Filtered == null)
         {
            this.imageEnabled_Filtered = ImageButton.CreateFilteredImage(this.ImageEnabled);
         }
         return this.imageEnabled_Filtered;
      }
   }
   private Bitmap imageDisabled_Filtered;
   protected virtual Bitmap ImageDisabled_Filtered
   {
      get
      {
         if (this.imageDisabled_Filtered == null)
         {
            this.imageDisabled_Filtered = ImageButton.CreateFilteredImage(this.ImageDisabled);
         }
         return this.imageDisabled_Filtered;
      }
   }
   private Bitmap imageByLayer_Filtered;
   protected virtual Bitmap ImageByLayer_Filtered
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
         return NestedLayers.IsPropertyInherited(layer, this.Property);
      }
      else if (node is IINodeWrapper)
      {
         IINodeWrapper inode = (IINodeWrapper)node;
         switch (this.Property)
         {
            case AnimatableProperty.BoxMode: return inode.NodeLayerProperties.DisplayByLayer;
            case AnimatableProperty.XRayMtl: return inode.NodeLayerProperties.DisplayByLayer;
            case AnimatableProperty.Renderable: return inode.NodeLayerProperties.RenderByLayer;
            case AnimatableProperty.IsHidden: return inode.IILayer.IsHidden;
            case AnimatableProperty.IsFrozen: return inode.IILayer.IsFrozen;
            default: return false;
         }
      }
      else
         return false;
   }

   public virtual Boolean IsEnabled(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      return (node.GetBoolProperty(this.Property) != this.InvertBehavior);
   }



   public override int GetWidth(TreeNode tn)
   {
      return this.ImageEnabled.Width;
   }

   public override int GetHeight(TreeNode tn)
   {
      return this.ImageEnabled.Height;
   }


   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      Boolean isFiltered = !tn.FilterResult.HasFlag(FilterResults.Show);

      Image img = null;
      if (this.isInheritedFromLayer(tn))
         img = (isFiltered) ? this.ImageByLayer_Filtered : this.ImageByLayer;
      else if (this.IsEnabled(tn))
         img = (isFiltered) ? this.ImageEnabled_Filtered : this.ImageEnabled;
      else
         img = (isFiltered) ? this.ImageDisabled_Filtered : this.ImageDisabled;

      Rectangle bounds = this.GetBounds(tn);
      bounds.X += (bounds.Width - img.Width) / 2;
      bounds.Size = img.Size;
      graphics.DrawImage(img, bounds);
   }

      

   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return base.GetTooltipText(tn);

      if (this.isInheritedFromLayer(tn))
         return this.Property.ToString() + " " + OutlinerResources.Tooltip_ByLayer;
      if (this.IsEnabled(tn))
         return this.ToolTipEnabled;
      else
         return this.ToolTipDisabled;
   }

   protected override bool Clickable(TreeNode tn)
   {
      return !this.isInheritedFromLayer(tn);
   }

  
   

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
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

      Boolean nodeValue = node.GetBoolProperty(this.Property);
      IEnumerable<IMaxNodeWrapper> maxNodes = HelperMethods.GetMaxNodes(nodes);
      SetNodePropertyCommand<Boolean> cmd = this.CreateCommand(maxNodes, !nodeValue);
      if (cmd != null)
         cmd.Execute(true);

      if (tree.NodeSorter is AnimatablePropertySorter && 
          ((AnimatablePropertySorter)tree.NodeSorter).Property == this.Property)
      {
         tree.AddToSortQueue(nodes);
         tree.StartTimedSort(true);
      }
   }
}
}
