using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace Outliner.Controls.Layout
{
[XmlRoot("TreeNodeLayout")]
public class TreeNodeLayout
{
   private TreeNodeLayoutItemCollection layoutItems;
   private TreeView treeView;
   private Int32 itemHeight;
   private Boolean fullRowSelect;
   private Boolean alternateBackground;

   [XmlIgnore]
   public TreeView TreeView 
   {
      get { return this.treeView; }
      set
      {
         this.treeView = value;
         value.VerticalScroll.SmallChange = this.ItemHeight;
         value.VerticalScroll.LargeChange = this.ItemHeight * 3;
      }
   }

   [XmlElement("ItemHeight")]
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

   [XmlElement("FullRowSelect")]
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

   [XmlElement("AlternateBackcolor")]
   [DefaultValue(false)]
   public Boolean AlternateBackground
   {
      get { return this.alternateBackground; }
      set
      {
         this.alternateBackground = value;
         if (this.TreeView != null)
            this.TreeView.Invalidate();
      }
   }

   [XmlArray("LayoutItems")]
   public TreeNodeLayoutItemCollection LayoutItems
   {
      get { return this.layoutItems; }
      set
      {
         this.layoutItems = value;
         value.Layout = this;
      }
   }

   public TreeNodeLayout()
   {
      this.LayoutItems = new TreeNodeLayoutItemCollection();
      this.itemHeight = 18;
      this.fullRowSelect = false;
      this.alternateBackground = false;
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

   public void HandleClick(MouseEventArgs e, TreeNode tn)
   {
      if (e == null)
         return;

      foreach (TreeNodeLayoutItem i in this.layoutItems)
      {
         if (i.GetBounds(tn).Contains(e.Location))
         {
            i.HandleClick(e, tn);
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




   /// <summary>
   /// Opens and parses an Xml file with a TreeNodeLayout.
   /// </summary>
   public static TreeNodeLayout FromXml(String file)
   {
      XmlSerializer xs = new XmlSerializer(typeof(TreeNodeLayout));
      using (FileStream st = new FileStream(file, FileMode.Open))
      {
         return (TreeNodeLayout)xs.Deserialize(st);
      }
   }

   /// <summary>
   /// Writes the TreeNodeLayout to an Xml file.
   /// </summary>
   public void ToXml(String file)
   {
      XmlSerializer xs = new XmlSerializer(typeof(TreeNodeLayout));
      using (FileStream st = new FileStream(file, FileMode.Create))
      using (StreamWriter stWr = new StreamWriter(st, Encoding.Unicode))
      {
         xs.Serialize(stWr, this);
      }
   }

   /// <summary>
   /// The standard 3dsMax-style layout.
   /// Itemheight = 18, FullRowSelect = false.
   /// </summary>
   public static TreeNodeLayout DefaultLayout
   {
      get
      {
         TreeNodeLayout layout = new TreeNodeLayout();
         layout.LayoutItems.Add(new TreeNodeIndent());
         layout.LayoutItems.Add(new HideButton());
         layout.LayoutItems.Add(new FreezeButton());
         layout.LayoutItems.Add(new TreeNodeIcon(IconSet.Max, false));
         layout.LayoutItems.Add(new TreeNodeText());
         layout.LayoutItems.Add(new FlexibleSpace());
         return layout;
      }
   }

   /// <summary>
   /// Maya-style layout.
   /// Itemheight = 20, FullRowSelect = true.
   /// </summary>
   public static TreeNodeLayout MayaLayout
   {
      get
      {
         TreeNodeLayout layout = new TreeNodeLayout();
         layout.ItemHeight = 20;
         layout.FullRowSelect = true;
         layout.LayoutItems.Add(new ExpandButton() { PaddingRight = 5, UseVisualStyles = false });
         layout.LayoutItems.Add(new TreeNodeIcon(IconSet.Maya, false));
         layout.LayoutItems.Add(new MayaStyleIndent());
         layout.LayoutItems.Add(new TreeNodeText());
         layout.LayoutItems.Add(new FlexibleSpace());
         layout.LayoutItems.Add(new HideButton());
         layout.LayoutItems.Add(new FreezeButton());
         return layout;
      }
   }
}
}
