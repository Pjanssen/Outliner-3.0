using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Controls.Tree.Layout
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(OutlinerResources), "Button_EmptySpace")]
public class EmptySpace : TreeNodeLayoutItem
{
   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeType.All)]
   public override Scene.MaxNodeType VisibleTypes
   {
      get { return MaxNodeType.All; }
      set { }
   }

   public EmptySpace() { }

   public override TreeNodeLayoutItem Copy()
   {
      return new EmptySpace();
   }

   public override bool IsVisible(TreeNode tn)
   {
      return true;
   }

   public override int GetAutoWidth(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return 0;

      TreeView tree = this.Layout.TreeView;
      Int32 w = tree.Width - this.Layout.PaddingLeft - this.Layout.PaddingRight;
      foreach (TreeNodeLayoutItem item in this.Layout.LayoutItems)
      {
         if (item.IsVisible(tn))
         {
            w -= item.PaddingLeft + item.PaddingRight;
            if (item != this)
               w -= item.GetWidth(tn);
         }
      }

      w -= 2; //A few pixels for the borders.

      if (tree.VerticalScroll.Visible)
         w -= SystemInformation.VerticalScrollBarWidth;

      return Math.Max(w, 0);
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.Layout == null)
         return 0;

      return this.Layout.ItemHeight;
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      //EmptySpace does not draw anything.
   }

   public override void HandleMouseDown(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;

      if (!this.Layout.FullRowSelect)
      {
         tree.SelectAllNodes(false);
      }
      else if (ControlHelpers.ControlPressed)
      {
         tree.SelectNode(tn, !tn.IsSelected);
      }
      else if (ControlHelpers.ShiftPressed)
      {
         tree.SelectNodesInsideRange(tree.LastSelectedNode, tn);
      }
      else if ((e.Button & MouseButtons.Right) != MouseButtons.Right || !tn.IsSelected)
      {
         tree.SelectAllNodes(false);
         tree.SelectNode(tn, true);
      }

      tree.OnSelectionChanged();
   }
}
}
