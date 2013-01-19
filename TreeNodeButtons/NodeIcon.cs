using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WinForms = System.Windows.Forms;
using Outliner.Filters;
using System.Resources;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.MaxUtils;
using Outliner.Controls.Tree.Layout;
using Outliner.Plugins;
using Outliner.Controls.Tree;

namespace Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
public class NodeIcon : TreeNodeButton
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

   public NodeIcon() { }

   public NodeIcon(Dictionary<String, Bitmap> icons)
   {
      if (icons == null)
         throw new ArgumentNullException("icons");

      this.icons = icons;
      this.iconSize = (this.icons.Count == 0) ? Size.Empty : this.icons.First().Value.Size;
   }

   public NodeIcon(ResourceSet resSet, Boolean invert)
   {
      this.icons = IconHelperMethods.CreateIconSetBitmaps(resSet, invert);
      this.iconSize = (this.icons.Count == 0) ? Size.Empty : this.icons.First().Value.Size;
      this.invert = invert;
   }

   public NodeIcon(IconSet iconSet, Boolean invert)
   {
      this.iconSet = iconSet;
      this.icons = IconHelperMethods.CreateIconSetBitmaps(iconSet, invert);
      this.iconSize = (this.icons.Count == 0) ? Size.Empty : this.icons.First().Value.Size;
      this.invert = invert;
   }


   public override TreeNodeLayoutItem Copy()
   {
      NodeIcon newItem = new NodeIcon();

      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;
      newItem.VisibleTypes = this.VisibleTypes;
      newItem.invert = this.invert;
      newItem.IconSet = this.IconSet;

      return newItem;
   }


   public override int GetAutoWidth(TreeNode tn)
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
         iconKey = "unknown";

      if (!tn.ShowNode)
         iconKey += "_filtered";

      if (!this.icons.TryGetValue(iconKey, out icon))
      {
         if (!this.icons.TryGetValue("unknown", out icon))
            return;
      }

      graphics.DrawImage(icon, this.GetBounds(tn));
   }

   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      if (tn == null)
         throw new ArgumentNullException("tn");

      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      if (node is ILayerWrapper)
      {
         ILayerWrapper layer = (ILayerWrapper)node;
         if (!layer.IsCurrent)
         {
            SetCurrentLayerCommand cmd = new SetCurrentLayerCommand(layer);
            cmd.Execute(false);

            tn.TreeView.Invalidate();
         }
      }
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Light)
      {
         Autodesk.Max.IINode inode = node.BaseObject as Autodesk.Max.IINode;
         if (inode == null)
            return;
         Autodesk.Max.ILightObject light = inode.ObjectRef as Autodesk.Max.ILightObject;
         if (light == null)
            return;
         ToggleLightCommand cmd = new ToggleLightCommand(node.ToEnumerable(), !light.UseLight);
         cmd.Execute(true);
      }
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Camera)
      {
         Autodesk.Max.IInterface ip = MaxInterfaces.Global.COREInterface;
         Autodesk.Max.IViewExp vpt = ip.ActiveViewExp;
         SetViewCameraCommand cmd = new SetViewCameraCommand(node, vpt);
         cmd.Execute(true);
      }
   }

   protected override bool Clickable(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      if (node is ILayerWrapper)
      {
         if (!((ILayerWrapper)node).IsCurrent)
            return true;
      }
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Light)
         return true;
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Camera)
         return true;

      return false;
   }

   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNode node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return null;

      if (node is ILayerWrapper)
      {
         if (!((ILayerWrapper)node).IsCurrent)
            return Resources.Tooltip_SetCurrentLayer;
      }
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Light)
         return Resources.Tooltip_ToggleLight;
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Camera)
         return Resources.Tooltip_SetCamera;

      return null;
   }
}
}
