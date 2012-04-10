using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree
{
public class TreeNodeCollection : ICollection<TreeNode>
{
   private TreeNode owner;
   private List<TreeNode> nodes;

   public TreeNodeCollection(TreeNode owner)
   {
      if (owner == null)
         throw new ArgumentNullException("owner");

      this.owner = owner;
      this.nodes = new List<TreeNode>();
   }

   private void boundsChanged(TreeNode tn)
   {
      TreeView tree = this.owner.TreeView;
      if (tree != null)
         tree.Update(TreeViewUpdateFlags.Scrollbars | TreeViewUpdateFlags.Redraw);
   }

   #region ICollection members
      
   public void Add(TreeNode item)
   {
      if (item == null)
         throw new ArgumentNullException("item");

      //Stop if item is already in the collection.
      if (this.Contains(item))
         return;

      //Remove item from old parent collection.
      item.Remove();

      //Set next/previous node links.
      if (this.nodes.Count > 0)
      {
         TreeNode lastNode = this.nodes[this.nodes.Count - 1];
         lastNode.NextNode = item;
         item.PreviousNode = lastNode;
      }

      //Add item to collection.
      this.nodes.Add(item);
      item.parent = this.owner;
      item.TreeView = this.owner.TreeView;

      //Invalidate tree bounds.
      this.boundsChanged(item);
   }

   public void Clear()
   {
      foreach (TreeNode tn in this.nodes)
      {
         tn.TreeView = null;
         tn.parent = null;
         tn.PreviousNode = null;
         tn.NextNode = null;
      }

      this.nodes.Clear();

      if (this.owner != null && this.owner.TreeView != null)
         this.owner.TreeView.Update(TreeViewUpdateFlags.All);
   }

   public bool Contains(TreeNode item)
   {
      return this.nodes.Contains(item);
   }

   public void CopyTo(TreeNode[] array, int arrayIndex)
   {
      this.nodes.CopyTo(array, arrayIndex);
   }

   public int Count
   {
      get { return this.nodes.Count; }
   }

   public bool IsReadOnly
   {
      get { return false; }
   }

   public bool Remove(TreeNode item)
   {
      if (item == null)
         return false;

      if (!this.Contains(item))
         return false;

      item.InvalidateBounds(item.IsVisible, true);

      item.TreeView = null;
      item.parent = null;
      if (item.PreviousNode != null)
         item.PreviousNode.NextNode = item.NextNode;
      if (item.NextNode != null)
         item.NextNode.PreviousNode = item.PreviousNode;
      item.PreviousNode = null;
      item.NextNode = null;
      
      return this.nodes.Remove(item);
   }

   public IEnumerator<TreeNode> GetEnumerator()
   {
      return this.nodes.GetEnumerator();
   }

   System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
   {
      return this.nodes.GetEnumerator();
   }

   #endregion


   public TreeNode this[Int32 index]
   {
      get 
      {
         if (index < 0 || index > this.nodes.Count - 1)
            return null;
         return this.nodes[index]; 
      }
      set 
      {
         if (value == null)
            throw new ArgumentNullException("value");

         if (index > 0)
         {
            TreeNode prevNode = this.nodes[index - 1];
            prevNode.NextNode = value;
            value.PreviousNode = prevNode;
         }
         if (index < this.nodes.Count - 1)
         {
            TreeNode nextNode = this.nodes[index + 1];
            nextNode.PreviousNode = value;
            value.NextNode = nextNode;
         }
         value.TreeView = this.owner.TreeView;
         value.parent = this.owner;
         this.nodes[index] = value;
      }
   }

   public Int32 IndexOf(TreeNode item)
   {
      return this.nodes.IndexOf(item);
   }

   public void Sort(IComparer<TreeNode> comparer, Boolean sortChildren)
   {
      if (comparer == null)
         return;

      if (this.owner.TreeView != null)
         this.owner.TreeView.BeginUpdate(TreeViewUpdateFlags.Redraw | TreeViewUpdateFlags.Scrollbars);

      this.nodes.Sort(comparer);

      TreeNode prevNode = null;
      foreach (TreeNode tn in this.nodes)
      {
         //Maintain previous-/nextnode links
         if (prevNode != null)
            prevNode.NextNode = tn;
         tn.PreviousNode = prevNode;
         prevNode = tn;
         
         //Invalidate treenode bounds, including children unless children 
         //are also going to be sorted.
         tn.InvalidateBounds(false, !sortChildren);

         //Sort childnodes.
         if (sortChildren && tn.Nodes.Count > 0)
            tn.Nodes.Sort(comparer, sortChildren);
      }
      if (prevNode != null)
         prevNode.NextNode = null;

      if (this.owner.TreeView != null)
         this.owner.TreeView.EndUpdate();
   }
}
}
