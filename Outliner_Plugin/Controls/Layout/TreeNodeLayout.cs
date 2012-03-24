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
   [XmlIgnore]
   public TreeView TreeView { get; set; }

   [XmlElement("ItemHeight")]
   [DefaultValue(18)]
   public Int32 ItemHeight { get; set; }

   [XmlElement("FullRowSelect")]
   [DefaultValue(false)]
   public Boolean FullRowSelect { get; set; }
   
   private List<TreeNodeLayoutItem> layoutItems;
   [XmlArray("LayoutItems")]
   public List<TreeNodeLayoutItem> LayoutItems
   {
      get { return this.layoutItems; }
   }

   public TreeNodeLayout()
   {
      this.layoutItems = new List<TreeNodeLayoutItem>();
      this.ItemHeight = 18;
      this.FullRowSelect = false;
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


      
   public void AddLayoutItem(TreeNodeLayoutItem item)
   {
      if (item == null)
         throw new ArgumentNullException("item");

      item.Layout = this;
      this.layoutItems.Add(item);

      if (this.TreeView != null)
         this.TreeView.Invalidate();
   }

   public void Clear()
   {
      foreach (TreeNodeLayoutItem item in this.layoutItems)
         item.Layout = null;

      this.layoutItems.Clear();

      if (this.TreeView != null)
         this.TreeView.Invalidate();
   }

   public bool Contains(TreeNodeLayoutItem item)
   {
      return this.layoutItems.Contains(item);
   }

   public void CopyTo(TreeNodeLayoutItem[] array, int arrayIndex)
   {
      this.layoutItems.CopyTo(array, arrayIndex);
   }

   public int Count
   {
      get { return this.layoutItems.Count; }
   }

   public bool IsReadOnly
   {
      get { return false; }
   }

   public bool Remove(TreeNodeLayoutItem item)
   {
      if (item == null)
         return false;

      if (this.layoutItems.Remove(item))
      {
         item.Layout = null;
         if (this.TreeView != null)
            this.TreeView.Invalidate();
         return true;
      }

      return false;
   }

   public IEnumerator<TreeNodeLayoutItem> GetEnumerator()
   {
      return this.layoutItems.GetEnumerator();
   }




   public Int32 IndexOf(TreeNodeLayoutItem item)
   {
      return this.layoutItems.IndexOf(item);
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
      {
         xs.Serialize(st, this);
      }
   }

   public static TreeNodeLayout DefaultLayout
   {
      get
      {
         TreeNodeLayout layout = new TreeNodeLayout();
         layout.AddLayoutItem(new TreeNodeIndent());
         layout.AddLayoutItem(new HideButton());
         layout.AddLayoutItem(new FreezeButton());
         layout.AddLayoutItem(new TreeNodeIcon(IconSet.Max, false));
         layout.AddLayoutItem(new TreeNodeText());
         layout.AddLayoutItem(new FlexibleSpace());
         return layout;
      }
   }

   public static TreeNodeLayout MayaLayout
   {
      get
      {
         TreeNodeLayout layout = new TreeNodeLayout();
         layout.ItemHeight = 20;
         layout.FullRowSelect = true;
         layout.AddLayoutItem(new ExpandButton() { PaddingRight = 5, UseVisualStyles = false });
         layout.AddLayoutItem(new TreeNodeIcon(IconSet.Maya, false));
         layout.AddLayoutItem(new MayaStyleIndent());
         layout.AddLayoutItem(new TreeNodeText());
         layout.AddLayoutItem(new FlexibleSpace());
         layout.AddLayoutItem(new HideButton());
         layout.AddLayoutItem(new FreezeButton());
         return layout;
      }
   }
}
}
