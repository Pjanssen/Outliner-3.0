using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Outliner.Controls.Layout
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
      [XmlIgnore]
      public TreeNodeLayout Layout { get; set; }

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
      /// The item will only be shown if this method returns true.
      /// </summary>
      public virtual Boolean IsVisible(TreeNode tn)
      {
         return true;
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
         if (this.Layout == null || this.Layout.TreeView == null)
            return Point.Empty;

         Point pt = new Point(0, tn.Bounds.Y);
         pt.X = 0 - this.Layout.TreeView.HorizontalScroll.Value;
         
         foreach (TreeNodeLayoutItem item in this.Layout)
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
         Rectangle b = new Rectangle(this.GetPos(tn), this.GetSize(tn));
         if (this.CenterVertically && this.Layout != null)
            b.Y += (this.Layout.TreeView.ItemHeight - b.Height) / 2;

         return b;
      }

      /// <summary>
      /// Draws the item for the given TreeNode at the given position.
      /// </summary>
      public abstract void Draw(Graphics g, TreeNode tn);

      /// <summary>
      /// This method is called when a TreeNode is clicked.
      /// </summary>
      public virtual void HandleClick(MouseEventArgs e, TreeNode tn) { }

      /// <summary>
      /// This method is called when a TreeNode is double-clicked.
      /// </summary>
      public virtual void HandleDoubleClick(MouseEventArgs e, TreeNode tn) { }
   }

}
