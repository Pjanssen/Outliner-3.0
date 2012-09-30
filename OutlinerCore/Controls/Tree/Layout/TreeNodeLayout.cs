using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace Outliner.Controls.Tree.Layout
{
[Serializable]
public class TreeNodeLayout
{
   private TreeNodeLayoutItemCollection layoutItems;
   private TreeView treeView;
   private Int32 itemHeight;
   private Boolean fullRowSelect;
   private Int32 paddingLeft;
   private Int32 paddingRight;

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

   public TreeNodeLayout()
   {
      this.LayoutItems = new TreeNodeLayoutItemCollection();
      this.itemHeight = 18;
      this.fullRowSelect = false;
      this.paddingLeft = 2;
      this.paddingRight = 2;
   }

   /// <summary>
   /// Creates a new <see cref="TreeNodeLayout"/>, as a copy of the passed <see cref="TreeNodeLayout"/>.
   /// </summary>
   /// <param name="layout">The <see cref="TreeNodeLayout"/> to copy.</param>
   public TreeNodeLayout(TreeNodeLayout layout)
   {
      this.LayoutItems = new TreeNodeLayoutItemCollection(layout.layoutItems);
      this.itemHeight = layout.itemHeight;
      this.fullRowSelect = layout.fullRowSelect;
      this.paddingLeft = layout.paddingLeft;
      this.paddingRight = layout.paddingRight;
   }


   public Int32 GetTreeNodeWidth(TreeNode tn)
   {
      return this.layoutItems.Sum(i => i.PaddingLeft + i.GetWidth(tn) + i.PaddingRight);
   }

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

   public TreeNodeLayoutItem GetItemAt(TreeNode tn, Point location)
   {
      return this.GetItemAt(tn, location.X, location.Y);
   }
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

   private TreeNode prevMouseOverTn;
   private TreeNodeLayoutItem prevMouseOverItem;

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
      foreach (TreeNodeLayoutItem item in this.layoutItems)
      {
         if (item.IsVisible(tn) && item.GetBounds(tn).Contains(e.Location))
         {
            if (item != prevMouseOverItem || tn != prevMouseOverTn)
            {
               item.HandleMouseEnter(e, tn);
               prevMouseOverItem = item;
               prevMouseOverTn = tn;
               break;
            }
         }
      }
   }

   public void HandleMouseDown(MouseEventArgs e, TreeNode tn)
   {
      if (e == null)
         return;

      foreach (TreeNodeLayoutItem i in this.layoutItems)
      {
         if (i.GetBounds(tn).Contains(e.Location))
         {
            i.HandleMouseDown(e, tn);
            break;
         }
      }
   }

   public void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      if (e == null)
         return;

      foreach (TreeNodeLayoutItem i in this.layoutItems)
      {
         if (i.GetBounds(tn).Contains(e.Location))
         {
            i.HandleMouseUp(e, tn);
            break;
         }
      }
   }

   public void HandleMouseDoubleClick(MouseEventArgs e, TreeNode tn)
   {
      if (e == null)
         return;

      foreach (TreeNodeLayoutItem i in this.layoutItems)
      {
         if (i.GetBounds(tn).Contains(e.Location))
         {
            i.HandleDoubleClick(e, tn);
            break;
         }
      }
   }




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
