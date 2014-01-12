using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace PJanssen.Outliner.Controls.Tree.Layout
{
/// <summary>
/// Defines a layout for the TreeView control.
/// </summary>
[Serializable]
public class TreeNodeLayout
{
   private TreeNodeLayoutItemCollection layoutItems;
   private TreeView treeView;
   private Int32 itemHeight;
   private Boolean fullRowSelect;
   private Int32 paddingLeft;
   private Int32 paddingRight;
   private TreeNode prevMouseOverTn;
   private TreeNodeLayoutItem prevMouseOverItem;

   /// <summary>
   /// Initializes a new instance of the TreeNodeLayout class.
   /// </summary>
   public TreeNodeLayout()
   {
      this.LayoutItems = new TreeNodeLayoutItemCollection();
      this.itemHeight = 18;
      this.fullRowSelect = false;
      this.paddingLeft = 2;
      this.paddingRight = 2;
   }

   /// <summary>
   /// Initializes a new instance of the TreeNodeLayout class, as a copy of 
   /// the given TreeNodeLayout.
   /// </summary>
   /// <param name="layout">The TreeNodeLayout to copy.</param>
   public TreeNodeLayout(TreeNodeLayout layout)
   {
      this.LayoutItems = new TreeNodeLayoutItemCollection(layout.layoutItems);
      this.itemHeight = layout.itemHeight;
      this.fullRowSelect = layout.fullRowSelect;
      this.paddingLeft = layout.paddingLeft;
      this.paddingRight = layout.paddingRight;
   }

   /// <summary>
   /// Gets or sets the TreeView control with which this layout is associated.
   /// </summary>
   [XmlIgnore]
   public TreeView TreeView 
   {
      get { return this.treeView; }
      set
      {
         if (value == null)
            throw new ArgumentNullException("value");

         this.treeView = value;
         value.VerticalScroll.SmallChange = this.ItemHeight;
         value.VerticalScroll.LargeChange = this.ItemHeight * 3;
      }
   }

   /// <summary>
   /// Gets or sets the height of one item in the TreeView.
   /// </summary>
   [XmlElement("item_height")]
   [DefaultValue(18)]
   public Int32 ItemHeight
   {
      get { return this.itemHeight; }
      set
      {
         this.itemHeight = value;
         if (this.TreeView != null)
         {
            this.TreeView.VerticalScroll.SmallChange = value;
            this.TreeView.VerticalScroll.LargeChange = value * 3;
            this.TreeView.Invalidate();
         }
      }
   }

   /// <summary>
   /// Gets or sets whether the entire width of the TreeView should be used for the
   /// selection highlighting.
   /// </summary>
   [XmlElement("fullrow_select")]
   [DefaultValue(false)]
   public Boolean FullRowSelect
   {
      get { return this.fullRowSelect; }
      set
      {
         this.fullRowSelect = value;
         if (this.TreeView != null)
            this.TreeView.Invalidate();
      }
   }

   /// <summary>
   /// Gets or sets the padding on the left side of the TreeView control.
   /// </summary>
   [XmlElement("padding_left")]
   [DefaultValue(2)]
   public Int32 PaddingLeft
   {
      get { return this.paddingLeft; }
      set
      {
         this.paddingLeft = value;
         if (this.TreeView != null)
            this.TreeView.Invalidate();
      }
   }

   /// <summary>
   /// Gets or sets the padding on the right side of the TreeView control.
   /// </summary>
   [XmlElement("padding_right")]
   [DefaultValue(2)]
   public Int32 PaddingRight
   {
      get { return this.paddingRight; }
      set
      {
         this.paddingRight = value;
         if (this.TreeView != null)
            this.TreeView.Invalidate();
      }
   }

   /// <summary>
   /// Gets or sets the collection of layout items for this layout.
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
   [XmlArray("layout_items")]
   public TreeNodeLayoutItemCollection LayoutItems
   {
      get { return this.layoutItems; }
      set
      {
         if (value == null)
            throw new ArgumentNullException("value");

         this.layoutItems = value;
         value.Layout = this;
      }
   }

   /// <summary>
   /// Calculates the combined width of all layout items for the given TreeNode.
   /// </summary>
   public Int32 GetTreeNodeWidth(TreeNode tn)
   {
      return this.layoutItems.Sum(i => i.PaddingLeft + i.GetWidth(tn) + i.PaddingRight);
   }

   /// <summary>
   /// Draws the layout items for the given TreeNode.
   /// </summary>
   public void DrawTreeNode(Graphics graphics, TreeNode tn) 
   {
      foreach (TreeNodeLayoutItem item in this.layoutItems)
      {
         if (item.IsVisible(tn))
         {
            item.Draw(graphics, tn);
         }
      }
   }

   /// <summary>
   /// Gets the layout item at the given location.
   /// </summary>
   /// <param name="tn">The TreeNode at the given location.</param>
   /// <param name="location">The location to look for a layout item.</param>
   public TreeNodeLayoutItem GetItemAt(TreeNode tn, Point location)
   {
      return this.GetItemAt(tn, location.X, location.Y);
   }

   /// <summary>
   /// Gets the layout item at the given location.
   /// </summary>
   /// <param name="tn">The TreeNode at the given location.</param>
   /// <param name="x">The x coordinate of the location to look for a layout item at.</param>
   /// <param name="y">The y coordinate of the location to look for a layout item at.</param>
   /// <returns></returns>
   public TreeNodeLayoutItem GetItemAt(TreeNode tn, Int32 x, Int32 y)
   {
      if (tn == null)
         return null;

      foreach (TreeNodeLayoutItem item in this.layoutItems)
      {
         if (item.IsVisible(tn) && item.GetBounds(tn).Contains(x, y))
            return item;
      }

      return null;
   }

   /// <summary>
   /// Handles the TreeView's MouseMove event, passing the event on to the layout items.
   /// </summary>
   public void HandleMouseMove(MouseEventArgs e, TreeNode tn)
   {
      if (e == null)
         return;

      if (this.prevMouseOverItem != null)
      {
         if (tn == this.prevMouseOverTn && this.prevMouseOverItem.GetBounds(tn).Contains(e.Location))
            return;
         else
         {
            prevMouseOverItem.HandleMouseLeave(e, tn);
            this.prevMouseOverItem = null;
            this.prevMouseOverTn = null;
         }
      }

      HandleMouseEvent(e, tn, i => InvokeHandleMouseEnter(e, tn, i));
   }

   private void InvokeHandleMouseEnter(MouseEventArgs e, TreeNode tn, TreeNodeLayoutItem item)
   {
      if (item != prevMouseOverItem || tn != prevMouseOverTn)
      {
         item.HandleMouseEnter(e, tn);
         prevMouseOverItem = item;
         prevMouseOverTn = tn;
      }
   }

   /// <summary>
   /// Handles the TreeView's MouseDown event, passing the event on to the layout items.
   /// </summary>
   public Boolean HandleMouseDown(MouseEventArgs e, TreeNode tn)
   {
      if (e == null || tn == null)
         return false;

      return HandleMouseEvent(e, tn, i => i.HandleMouseDown(e, tn));
   }

   /// <summary>
   /// Handles the TreeView's MouseUp event, passing the event on to the layout items.
   /// </summary>
   public void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      if (e == null || tn == null)
         return;

      HandleMouseEvent(e, tn, i => i.HandleMouseUp(e, tn));
   }

   /// <summary>
   /// Handles the TreeView's MouseDoubleClick event, passing the event on to the layout items.
   /// </summary>
   public void HandleMouseDoubleClick(MouseEventArgs e, TreeNode tn)
   {
      if (e == null || tn == null)
         return;

      HandleMouseEvent(e, tn, i => i.HandleDoubleClick(e, tn));
   }

   private Boolean HandleMouseEvent(MouseEventArgs e, TreeNode tn, Action<TreeNodeLayoutItem> action)
   {
      foreach (TreeNodeLayoutItem i in this.layoutItems)
      {
         if (i.IsVisible(tn) && i.GetBounds(tn).Contains(e.Location))
         {
            action(i);
            return true;
         }
      }
      return false;
   }

   

   /// <summary>
   /// Defines a standard layout with indentation, text and an empty space.
   /// </summary>
   public static TreeNodeLayout SimpleLayout
   {
      get
      {
         TreeNodeLayout layout = new TreeNodeLayout();
         layout.LayoutItems.Add(new TreeNodeIndent());
         layout.LayoutItems.Add(new TreeNodeText());
         layout.LayoutItems.Add(new EmptySpace());
         return layout;
      }
   }
}
}
