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
using Autodesk.Max;
using Outliner.Modes;

namespace Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(Resources), "Str_NodeIcon")]
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


   protected override int GetAutoWidth(TreeNode tn)
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

      Bitmap icon = this.GetIcon(tn);
      
      graphics.DrawImage(icon, this.GetBounds(tn));
   }

   private Bitmap GetIcon(TreeNode tn)
   {
      Bitmap icon;
      String imageKey = tn.ImageKey;
      String iconSuffix = OutlinerGUP.Instance.ColorScheme.ImageResourceSuffix;
      String iconKey = GetIconKey(tn, imageKey + "_" + iconSuffix);

      if (!this.icons.TryGetValue(iconKey, out icon))
      {
         iconKey = GetIconKey(tn, imageKey);
         if (!this.icons.TryGetValue(iconKey, out icon))
         {
            iconKey = GetIconKey(tn, "unknown_" + iconSuffix);
            if (!this.icons.TryGetValue(iconKey, out icon))
            {
               iconKey = GetIconKey(tn, "unknown");
               if (!this.icons.TryGetValue(iconKey, out icon))
               {
                  return null;
               }
            }
         }
      }

      return icon;
   }

   private static String GetIconKey(TreeNode tn, String baseName)
   {
      String iconKey = baseName;
      if (!tn.ShowNode)
         iconKey += "_filtered";
      return iconKey;
   }

   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      if (tn == null)
         throw new ArgumentNullException("tn");

      IMaxNode node = TreeMode.GetMaxNode(tn);
      if (node == null)
         return;

      OutlinerAction action = null;

      if (node.BaseObject is IILayer)
         action = OutlinerActions.GetAction("SetCurrentLayer");
      else if (node.SuperClassID == SClass_ID.Light)
         action = Actions.ToggleLight;
      else if (node.SuperClassID == SClass_ID.Camera)
         action = Actions.SetActiveViewCamera;

      if (action != null)
         action(tn, TreeMode.GetMaxNodes(this.Layout.TreeView.SelectedNodes));
   }

   protected override bool Clickable(TreeNode tn)
   {
      IMaxNode node = TreeMode.GetMaxNode(tn);
      if (node == null)
         return false;

      OutlinerPredicate predicate = null;

      if (node.BaseObject is IILayer)
         predicate = OutlinerActions.GetPredicate("IsNotCurrentLayer");
      else if (node.SuperClassID == SClass_ID.Light)
         predicate = Actions.AlwaysTrue;
      else if (node.SuperClassID == SClass_ID.Camera)
         predicate = Actions.AlwaysTrue;

      if (predicate != null)
         return predicate(tn, TreeMode.GetMaxNodes(this.Layout.TreeView.SelectedNodes));
      else
         return false;
   }

   protected override string GetTooltipText(TreeNode tn)
   {
      if (!this.Clickable(tn))
         return null;

      IMaxNode node = TreeMode.GetMaxNode(tn);
      if (node == null)
         return null;

      if (node.BaseObject is IILayer)
         return Resources.Tooltip_SetCurrentLayer;
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Light)
         return Resources.Tooltip_ToggleLight;
      else if (node.SuperClassID == Autodesk.Max.SClass_ID.Camera)
         return Resources.Tooltip_SetCamera;

      return null;
   }
}
}
