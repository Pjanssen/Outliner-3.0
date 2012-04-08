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
   [XmlInclude(typeof(AddButton))]
   [XmlInclude(typeof(BoxModeButton))]
   [XmlInclude(typeof(ExpandButton))]
   [XmlInclude(typeof(FlexibleSpace))]
   [XmlInclude(typeof(FreezeButton))]
   [XmlInclude(typeof(HideButton))]
   [XmlInclude(typeof(MayaStyleIndent))]
   [XmlInclude(typeof(RenderableButton))]
   [XmlInclude(typeof(TreeNodeIcon))]
   [XmlInclude(typeof(TreeNodeIndent))]
   [XmlInclude(typeof(TreeNodeText))]
   [XmlInclude(typeof(WireColorButton))]
   public abstract class TreeNodeLayoutItem
   {
      public TreeNodeLayoutItem()
      {
         this.VisibleTypes = MaxNodeTypes.All;
      }

      [XmlIgnore]
      public TreeNodeLayout Layout { get; internal set; }

      /// <summary>
      /// The number of blank pixels to the left of the layout item.
      /// </summary>
      [XmlAttribute("padding_left")]
      [System.ComponentModel.DefaultValue(0)]
      public virtual Int32 PaddingLeft { get; set; }

      /// <summary>
      /// The number of blank pixels to the right of the layout item.
      /// </summary>
      [XmlAttribute("padding_right")]
      [System.ComponentModel.DefaultValue(0)]
      public virtual Int32 PaddingRight { get; set; }

      /// <summary>
      /// Determines for which MaxNodeTypes this item will be visible.
      /// </summary>
      [XmlAttribute("visible_types")]
      [DefaultValue(MaxNodeTypes.All)]
      public virtual MaxNodeTypes VisibleTypes { get; set; }

      /// <summary>
      /// The item will only be shown if this method returns true.
      /// </summary>
      public virtual Boolean IsVisible(TreeNode tn)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (node == null)
            return true;
         else
            return node.IsNodeType(this.VisibleTypes);
      }

      /// <summary>
      /// If true, the GetBounds method will return an item bounds rectangle
      /// which is centered relative to the tree.ItemHeight.
      /// </summary>
      public virtual Boolean CenterVertically { get { return true; } }

      /// <summary>
      /// Returns the position of the item.
      /// </summary>
      public virtual Point GetPos(TreeNode tn)
      {
         if (tn == null)
            return Point.Empty;
         if (this.Layout == null || this.Layout.TreeView == null)
            return Point.Empty;

         Point pt = new Point(0, tn.Bounds.Y);
         pt.X = 0 - this.Layout.TreeView.HorizontalScroll.Value;
         
         foreach (TreeNodeLayoutItem item in this.Layout.LayoutItems)
         {
            if (item.IsVisible(tn))
            {
               pt.X += item.PaddingLeft;
               if (item == this)
                  break;
               pt.X += item.GetWidth(tn) + item.PaddingRight;
            }
         }

         return pt;
      }

      /// <summary>
      /// Returns the width of the item.
      /// </summary>
      public abstract Int32 GetWidth(TreeNode tn);

      /// <summary>
      /// Returns the height of the item.
      /// </summary>
      public abstract Int32 GetHeight(TreeNode tn);

      /// <summary>
      /// Returns the size of the item.
      /// </summary>
      public virtual Size GetSize(TreeNode tn)
      {
         return new Size(this.GetWidth(tn), this.GetHeight(tn));
      }

      /// <summary>
      /// The bounds of the item.
      /// </summary>
      public virtual Rectangle GetBounds(TreeNode tn)
      {
         if (this.Layout == null)
            return Rectangle.Empty;

         Rectangle b = new Rectangle(this.GetPos(tn), this.GetSize(tn));
         if (this.CenterVertically && this.Layout != null)
            b.Y += (this.Layout.ItemHeight - b.Height) / 2;

         return b;
      }

      /// <summary>
      /// Draws the item for the given TreeNode at the given position.
      /// </summary>
      public abstract void Draw(Graphics graphics, TreeNode tn);

      /// <summary>
      /// This method is called when the mouse pointer is moved over the item for the first time.
      /// </summary>
      public virtual void HandleMouseEnter(MouseEventArgs e, TreeNode tn) { }

      /// <summary>
      /// This method is called when the mouse pointer is moved from within to outside the bounds of the item.
      /// </summary>
      /// <param name="e"></param>
      /// <param name="tn"></param>
      public virtual void HandleMouseLeave(MouseEventArgs e, TreeNode tn) { }

      /// <summary>
      /// This method is called when a mouse button is held down over a TreeNode.
      /// </summary>
      /// <param name="e"></param>
      /// <param name="tn"></param>
      public virtual void HandleMouseDown(MouseEventArgs e, TreeNode tn) { }

      /// <summary>
      /// This method is called when a mouse button is released over a TreeNode.
      /// </summary>
      public virtual void HandleMouseUp(MouseEventArgs e, TreeNode tn) { }

      /// <summary>
      /// This method is called when a TreeNode is double-clicked.
      /// </summary>
      public virtual void HandleDoubleClick(MouseEventArgs e, TreeNode tn) { }
   }

}
