using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Scene;

namespace Outliner.Controls.Tree.Layout
{
public class EmptySpace : TreeNodeLayoutItem
{
   [XmlAttribute("padding_left")]
   [DefaultValue(2)]
   public override int PaddingLeft
   {
      get { return 2; }
      set { }
   }

   [XmlAttribute("padding_right")]
   [DefaultValue(2)]
   public override int PaddingRight
   {
      get { return 2; }
      set { }
   }

   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeTypes.All)]
   public override Scene.MaxNodeTypes VisibleTypes
   {
      get { return MaxNodeTypes.All; }
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

   public override int GetWidth(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return 0;

      TreeView tree = this.Layout.TreeView;
      Int32 w = tree.Width;
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
      Boolean isSelected = tree.IsSelectedNode(tn);
      if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed)
      {
         if (!isSelected && this.Layout.FullRowSelect)
         {
            tree.SelectAllNodes(false);
            tree.SelectNode(tn, true);
            tree.OnSelectionChanged();
         }
      }
   }

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      Boolean isSelected = tree.IsSelectedNode(tn);

      if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed)
      {
         tree.SelectAllNodes(false);
      }

      if (this.Layout.FullRowSelect)
      {
         if (HelperMethods.ShiftPressed && tree.LastSelectedNode != null)
            tree.SelectNodesInsideRange(tree.LastSelectedNode, tn);
         else if (HelperMethods.ControlPressed)
            tree.SelectNode(tn, !isSelected);
         else
            tree.SelectNode(tn, true);
      }

      tree.OnSelectionChanged();
   }
}
}
