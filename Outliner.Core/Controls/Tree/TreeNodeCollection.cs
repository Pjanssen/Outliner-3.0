using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree
{
/// <summary>
/// Represents a collection of TreeNode objects.
/// </summary>
public class TreeNodeCollection : ICollection<TreeNode>
{
   private TreeNode owner;
   private List<TreeNode> unfilteredNodes;
   private List<TreeNode> filteredNodes;

   internal TreeNodeCollection(TreeNode owner)
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
     
   /// <summary>
   /// Adds a TreeNode to this collection.
   /// </summary>
   /// <remarks>The given TreeNode will be removed from its current parent before it's
   /// added to this collection.</remarks>
   /// <param name="item">The TreeNode to add.</param>
   public void Add(TreeNode item)
   {
      if (item == null)
         throw new ArgumentNullException("item");

      //Stop if item is already in the collection.
      if (this.Contains(item))
         return;

      if (this.dependencyLoopTest(item))
         throw new InvalidOperationException(String.Format("Parenting '{0}' to '{1}' would create a dependency loop.", 
                                                           item.ToString(),
                                                           this.owner.ToString()));

      //Remove item from old parent collection.
      item.Remove();

      this.unfilteredNodes.Add(item);

      item.parent = this.owner;
      item.TreeView = this.owner.TreeView;

      if (item.ShowNode)
         this.addFiltered(item);
   }

   private Boolean dependencyLoopTest(TreeNode newItem)
   {
      return this.owner.Equals(newItem) || this.owner.IsChildOf(newItem);
   }

   /// <summary>
   /// Removes all TreeNodes from this collection.
   /// </summary>
   /// <remarks>The TreeNodes in the collection will be removed from the TreeView 
   /// and will not be added to the TreeView's rootnode.</remarks>
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

   /// <summary>
   /// Tests if the collection contains the given TreeNode.
   /// </summary>
   public bool Contains(TreeNode item)
   {
      return this.unfilteredNodes.Contains(item);
   }

   /// <summary>
   /// Tests if the collection contains the given TreeNode.
   /// </summary>
   /// <param name="item">The TreeNode to find in the collection.</param>
   /// <param name="recursive">Specifies whether the child collections should be searched too.</param>
   public bool Contains(TreeNode item, Boolean recursive)
   {
      return this.Contains(item) || this.Any(tn => tn.Nodes.Contains(item, true));
   }

   /// <summary>
   /// Copies the collection to an array.
   /// </summary>
   /// <param name="array">The array to copy the collection to.</param>
   /// <param name="arrayIndex">The starting index for the copy operation.</param>
   public void CopyTo(TreeNode[] array, int arrayIndex)
   {
      this.unfilteredNodes.CopyTo(array, arrayIndex);
   }

   /// <summary>
   /// Gets the number of TreeNodes in this collection.
   /// </summary>
   public int Count
   {
      get { return this.filteredNodes.Count; }
   }

   /// <summary>
   /// Gets a value indicating whether the collection is read-only.
   /// </summary>
   public bool IsReadOnly
   {
      get { return false; }
   }

   /// <summary>
   /// Removes the given TreeNode from the collection.
   /// </summary>
   public bool Remove(TreeNode item)
   {
      if (item == null)
         return false;

      if (!this.Contains(item))
         return false;

      this.removeFiltered(item);

      TreeView tree = item.TreeView;
      if (tree != null)
         tree.SelectNode(item, false);

      item.TreeView = null;
      item.parent = null;

      Boolean result = this.unfilteredNodes.Remove(item);

      if (tree != null)
         tree.Update(TreeViewUpdateFlags.TreeNodeBounds | TreeViewUpdateFlags.Redraw);

      return result;
   }

   /// <summary>
   /// Gets the enumerator for this collection.
   /// </summary>
   public IEnumerator<TreeNode> GetEnumerator()
   {
      return this.filteredNodes.GetEnumerator();
   }

   System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
   {
      return this.filteredNodes.GetEnumerator();
   }

   #endregion


   #region Filter
   
   private void addFiltered(TreeNode tn)
   {
      if (this.filteredNodes.Contains(tn))
         return;

      //Set next/previous node links.
      if (this.filteredNodes.Count > 0)
      {
         TreeNode lastNode = this.filteredNodes[this.filteredNodes.Count - 1];
         lastNode.NextNode = tn;
         tn.PreviousNode = lastNode;
      }

      //Add item to collection.
      this.filteredNodes.Add(tn);

      if (tn.Parent != null && !tn.Parent.ShowNode)
         tn.Parent.parent.Nodes.addFiltered(tn.Parent);

      tn.InvalidateBounds(true, true);

      if (tn.TreeView != null)
         tn.TreeView.Update(TreeViewUpdateFlags.Scrollbars | TreeViewUpdateFlags.Redraw);
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

   #endregion


   /// <summary>
   /// Gets or sets the TreeNode at the given index from the collection.
   /// </summary>
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

   /// <summary>
   /// Gets the index of the given TreeNode.
   /// </summary>
   public Int32 IndexOf(TreeNode item)
   {
      return this.filteredNodes.IndexOf(item);
   }

   /// <summary>
   /// Sorts the TreeNodes in the collection.
   /// </summary>
   /// <param name="comparer">The comparer to use for the sorting.</param>
   /// <param name="sortChildren">If true, the childnodes of the TreeNodes in the collection will be sorted recursively.</param>
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
