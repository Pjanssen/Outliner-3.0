using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Outliner.Filters;
using System.Resources;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Tree.Layout
{
public class TreeNodeIcon : TreeNodeButton
{
   private Dictionary<String, Bitmap> icons;
   private Size iconSize;
   private IconSet iconSet;
   private Boolean invert;

   [XmlAttribute("iconSet")]
   public IconSet IconSet
   {
      get { return this.iconSet; }
      set
      {
         this.iconSet = value;
         this.icons = IconHelperMethods.CreateIconSetBitmaps(value, this.invert);
         this.iconSize = (this.icons.Count == 0) ? Size.Empty : this.icons.First().Value.Size;
      }
   }

   [XmlAttribute("invert")]
   [DefaultValue(true)]
   public Boolean Invert
   {
      get { return this.invert; }
      set
      {
         this.invert = value;
         this.icons = IconHelperMethods.CreateIconSetBitmaps(this.iconSet, value);
      }
   }

   public TreeNodeIcon() { }

   public TreeNodeIcon(Dictionary<String, Bitmap> icons) 
   {
      if (icons == null)
         throw new ArgumentNullException("icons");

      this.icons = icons;
      this.iconSize = (this.icons.Count == 0) ? Size.Empty : this.icons.First().Value.Size;
   }

   public TreeNodeIcon(ResourceSet resSet, Boolean invert) 
   {
      this.icons = IconHelperMethods.CreateIconSetBitmaps(resSet, invert);
      this.iconSize = (this.icons.Count == 0) ? Size.Empty : this.icons.First().Value.Size;
      this.invert = invert;
   }

   public TreeNodeIcon(IconSet iconSet, Boolean invert)
   {
      this.iconSet = iconSet;
      this.icons = IconHelperMethods.CreateIconSetBitmaps(iconSet, invert);
      this.iconSize = (this.icons.Count == 0) ? Size.Empty : this.icons.First().Value.Size;
      this.invert = invert;
   }

   public override int GetWidth(TreeNode tn)
   {
      return this.iconSize.Width;
   }

   public override int GetHeight(TreeNode tn)
   {
      return this.iconSize.Height;
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      if (this.Layout == null || this.icons == null)
         return;

      Bitmap icon = null;
      String iconKey = tn.ImageKey;

      if (iconKey == null)
         iconKey = IconHelperMethods.IMGKEY_UNKNOWN;

      if (tn.FilterResult == FilterResults.ShowChildren)
         iconKey += "_filtered";

      if (!this.icons.TryGetValue(iconKey, out icon))
      {
         if (!this.icons.TryGetValue(IconHelperMethods.IMGKEY_UNKNOWN, out icon))
            return;
      }

      graphics.DrawImage(icon, this.GetBounds(tn));
   }

   public override void HandleClick(MouseEventArgs e, TreeNode tn) 
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      if (node is IILayerWrapper)
      {
         IILayerWrapper layer = (IILayerWrapper)node;
         if (!layer.IsCurrent)
         {
            SetActiveLayerCommand cmd = new SetActiveLayerCommand(layer);
            cmd.Execute(false);
         }
      }
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Light)
      {
         Autodesk.Max.IINode inode = node.WrappedNode as Autodesk.Max.IINode;
         if (inode == null)
            return;
         Autodesk.Max.ILightObject light = inode.ObjectRef as Autodesk.Max.ILightObject;
         if (light == null)
            return;
         ToggleLightCommand cmd = new ToggleLightCommand(new List<IMaxNodeWrapper>(1) { node }, !light.UseLight);
         cmd.Execute(true);
      }
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Camera)
      {
         Autodesk.Max.IInterface ip = Autodesk.Max.GlobalInterface.Instance.COREInterface;
         Autodesk.Max.IViewExp vpt = ip.ActiveViewExp;
         SetViewCameraCommand cmd = new SetViewCameraCommand(node, vpt);
         cmd.Execute(true);
      }
   }

   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return null;

      if (node is IILayerWrapper)
      {
         if (!((IILayerWrapper)node).IsCurrent)
            return OutlinerResources.Tooltip_SetCurrentLayer;
      }
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Light)
         return OutlinerResources.Tooltip_ToggleLight;
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Camera)
         return OutlinerResources.Tooltip_SetCamera;
      
      return null;
   }
}
}
