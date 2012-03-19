using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;

namespace Outliner.Controls.Layout
{
[XmlRoot("TreeNodeLayout")]
public class TreeNodeLayout : ICollection<TreeNodeLayoutItem>
{
   private TreeView treeView;
   [XmlIgnore]
   public TreeView TreeView 
   {
      get { return this.treeView; }
      set
      {
         this.treeView = value;
      }
   }
   [XmlIgnore]
   private List<TreeNodeLayoutItem> items { get; set; }

   public TreeNodeLayout()
   {
      this.items = new List<TreeNodeLayoutItem>();
   }

   public Int32 GetTreeNodeWidth(TreeNode tn)
   {
      return this.items.Sum(i => i.PaddingLeft + i.GetWidth(tn) + i.PaddingRight);
   }

   public void DrawTreeNode(Graphics g, TreeNode tn) 
   {
      foreach (TreeNodeLayoutItem item in this.items)
      {
         if (item.IsVisible(tn))
         {
            item.Draw(g, tn);
         }
      }
   }

   private TreeNode prevMouseOverTn;
   private TreeNodeLayoutItem prevMouseOverItem;

   public void HandleMouseMove(MouseEventArgs e, TreeNode tn)
   {
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
      foreach (TreeNodeLayoutItem item in this.items)
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
      foreach (TreeNodeLayoutItem i in this.items)
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
      foreach (TreeNodeLayoutItem i in this.items)
      {
         if (i.GetBounds(tn).Contains(e.Location))
         {
            i.HandleDoubleClick(e, tn);
            break;
         }
      }
   }


   #region ICollection members
      
   public void Add(TreeNodeLayoutItem item)
   {
      item.Layout = this;
      this.items.Add(item);

      if (this.TreeView != null)
         this.TreeView.Invalidate();
   }

   public void Clear()
   {
      foreach (TreeNodeLayoutItem item in this.items)
         item.Layout = null;

      this.items.Clear();

      if (this.TreeView != null)
         this.TreeView.Invalidate();
   }

   public bool Contains(TreeNodeLayoutItem item)
   {
      return this.items.Contains(item);
   }

   public void CopyTo(TreeNodeLayoutItem[] array, int arrayIndex)
   {
      this.items.CopyTo(array, arrayIndex);
   }

   public int Count
   {
      get { return this.items.Count; }
   }

   public bool IsReadOnly
   {
      get { return false; }
   }

   public bool Remove(TreeNodeLayoutItem item)
   {
      if (this.items.Remove(item))
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
      return this.items.GetEnumerator();
   }

   System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
   {
      return this.items.GetEnumerator();
   }

   #endregion

   public TreeNodeLayoutItem this[int index]
   {
      get { return this.items[index]; }
      set 
      {
         value.Layout = this;
         this.items[index] = value;
         if (this.TreeView != null)
            this.TreeView.Invalidate();
      }
   }

   public Int32 IndexOf(TreeNodeLayoutItem item)
   {
      return this.items.IndexOf(item);
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
         layout.Add(new TreeNodeIndent());
         layout.Add(new TreeNodeIcon(IconSet.Max, false));
         layout.Add(new TreeNodeText());
         layout.Add(new FlexibleSpace());
         layout.Add(new HideButton());
         layout.Add(new FreezeButton());
         return layout;
      }
   }

   public static TreeNodeLayout MayaLayout
   {
      get
      {
         TreeNodeLayout layout = new TreeNodeLayout();
         layout.Add(new ExpandButton() { PaddingRight = 5 });
         layout.Add(new TreeNodeIcon(IconSet.Max, false));
         layout.Add(new MayaStyleIndent());
         layout.Add(new TreeNodeText());
         //layout.Add(new HideButton());
         //layout.Add(new FreezeButton());
         //layout.Add(new BoxModeButton());
         //layout.Add(new FlexibleSpace());
         return layout;
      }
   }

}
}
