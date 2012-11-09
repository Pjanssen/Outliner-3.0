using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree
{
public class TreeNodeCollection : ICollection<TreeNode>
{
   private TreeNode owner;
   private List<TreeNode> unfilteredNodes;
   private List<TreeNode> filteredNodes;

   public TreeNodeCollection(TreeNode owner)
   {
      if (owner == null)
         throw new ArgumentNullException("owner");

      this.owner = owner;
      this.unfilteredNodes = new List<TreeNode>();
      this.filteredNodes = new List<TreeNode>();
   }

   private void boundsChanged(TreeNode tn)
   {
      tn.InvalidateBounds(tn.IsVisible, tn.IsExpanded);
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

      this.unfilteredNodes.Add(item);

      item.parent = this.owner;
      item.TreeView = this.owner.TreeView;

      if (item.ShowNode)
         this.addFiltered(item);
   }

   public void Clear()
   {
      foreach (TreeNode tn in this.unfilteredNodes)
      {
         tn.TreeView = null;
         tn.parent = null;
         tn.PreviousNode = null;
         tn.NextNode = null;
      }

      this.unfilteredNodes.Clear();
      this.filteredNodes.Clear();

      if (this.owner != null && this.owner.TreeView != null)
         this.owner.TreeView.Update(TreeViewUpdateFlags.All);
   }

   public bool Contains(TreeNode item)
   {
      return this.unfilteredNodes.Contains(item);
   }

   public void CopyTo(TreeNode[] array, int arrayIndex)
   {
      this.unfilteredNodes.CopyTo(array, arrayIndex);
   }

   public int Count
   {
      get { return this.filteredNodes.Count; }
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

      this.removeFiltered(item);

      TreeView tree = item.TreeView;
      tree.SelectNode(item, false);

      item.TreeView = null;
      item.parent = null;

      Boolean result = this.unfilteredNodes.Remove(item);

      tree.Update(TreeViewUpdateFlags.TreeNodeBounds | TreeViewUpdateFlags.Redraw);

      return result;
   }

   public IEnumerator<TreeNode> GetEnumerator()
   {
      return this.filteredNodes.GetEnumerator();
   }

   System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
   {
      return this.filteredNodes.GetEnumerator();
   }

   #endregion




   private void addFiltered(TreeNode tn)
   {
      //Set next/previous node links.
      if (this.filteredNodes.Count > 0)
      {
         TreeNode lastNode = this.filteredNodes[this.filteredNodes.Count - 1];
         lastNode.NextNode = tn;
         tn.PreviousNode = lastNode;
      }

      //Add item to collection.
      this.filteredNodes.Add(tn);

      //Invalidate tree bounds.
      this.boundsChanged(tn);

      if (tn.Parent != null && !tn.Parent.ShowNode)
         tn.Parent.parent.Nodes.addFiltered(tn.Parent);
   }

   private void removeFiltered(TreeNode tn) 
   {
      tn.InvalidateBounds(true, true);

      if (tn.PreviousNode != null)
         tn.PreviousNode.NextNode = tn.NextNode;
      if (tn.NextNode != null)
         tn.NextNode.PreviousNode = tn.PreviousNode;
      tn.PreviousNode = null;
      tn.NextNode = null;

      if (tn.Parent != null && !tn.Parent.ShowNode && !tn.Parent.HasUnfilteredChildren)
         tn.Parent.parent.Nodes.removeFiltered(tn.Parent);

      this.filteredNodes.Remove(tn);
   }

   internal void updateFilter(TreeNode tn)
   {
      if (!tn.ShowNode && !tn.HasUnfilteredChildren)
         this.removeFiltered(tn);
      else if (!this.filteredNodes.Contains(tn))
         this.addFiltered(tn);
   }




   public TreeNode this[Int32 index]
   {
      get 
      {
         if (index < 0 || index > this.filteredNodes.Count - 1)
            return null;
         return this.filteredNodes[index]; 
      }
      set 
      {
         if (value == null)
            throw new ArgumentNullException("value");

         if (index > 0)
         {
            TreeNode prevNode = this.filteredNodes[index - 1];
            prevNode.NextNode = value;
            value.PreviousNode = prevNode;
         }
         if (index < this.filteredNodes.Count - 1)
         {
            TreeNode nextNode = this.filteredNodes[index + 1];
            nextNode.PreviousNode = value;
            value.NextNode = nextNode;
         }
         value.TreeView = this.owner.TreeView;
         value.parent = this.owner;
         this.filteredNodes[index] = value;
      }
   }

   public Int32 IndexOf(TreeNode item)
   {
      return this.filteredNodes.IndexOf(item);
   }

   public void Sort(IComparer<TreeNode> comparer, Boolean sortChildren)
   {
      if (comparer == null)
         return;

      if (this.owner.TreeView != null)
         this.owner.TreeView.BeginUpdate(TreeViewUpdateFlags.Redraw | TreeViewUpdateFlags.Scrollbars);

      this.filteredNodes.Sort(comparer);

      TreeNode prevNode = null;
      foreach (TreeNode tn in this.filteredNodes)
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
