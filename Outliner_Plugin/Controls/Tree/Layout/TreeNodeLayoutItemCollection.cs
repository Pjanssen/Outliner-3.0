using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree.Layout
{
public class TreeNodeLayoutItemCollection : ICollection<TreeNodeLayoutItem>
{
   private List<TreeNodeLayoutItem> items;
   private TreeNodeLayout layout;
   internal TreeNodeLayout Layout
   {
      get { return this.layout; }
      set 
      {
         this.layout = value;
         foreach (TreeNodeLayoutItem item in this.items)
            item.Layout = value;
      }
   }

   public TreeNodeLayoutItemCollection()
   {
      this.items = new List<TreeNodeLayoutItem>();
   }

   public void Add(TreeNodeLayoutItem item)
   {
      if (item == null)
         throw new ArgumentNullException("item");

      item.Layout = this.layout;
      this.items.Add(item);
   }

   public void Clear()
   {
      foreach (TreeNodeLayoutItem item in this.items)
         item.Layout = null;

      this.items.Clear();
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
      return this.items.Remove(item);
   }

   public IEnumerator<TreeNodeLayoutItem> GetEnumerator()
   {
      return this.items.GetEnumerator();
   }

   System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
   {
      return this.items.GetEnumerator();
   }
}
}
