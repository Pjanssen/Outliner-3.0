using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Outliner.Filters;

namespace Outliner.Controls.Tree
{
public class TreeNode
{
   internal TreeNode parent;
   private TreeView treeview;
   private TreeNodeStates state;
   private Boolean isExpanded;
   private String text;
   private Boolean boundsValid;
   private Rectangle bounds;
   private Color backColor;
   private Color foreColor;
   private FontStyle fontStyle;
   private String imageKey;
   private FilterResults filterResult;

   public TreeNodeCollection Nodes { get; private set; }
   public DragDropHandler DragDropHandler { get; set; }
   public Object Tag { get; set; }

   public TreeNode() : this("") { }
   internal TreeNode(TreeView tree, String text) : this(text)
   {
      this.TreeView = tree;
   }
   public TreeNode(String text) 
      : this (text, Color.Empty, Color.Empty, FontStyle.Regular, String.Empty, FilterResults.Show)
   { }
   public TreeNode(String text, Color backColor, Color foreColor,
                   FontStyle fontStyle, String imageKey, 
                   FilterResults filterResult)
   {
      this.text = text;
      this.backColor = backColor;
      this.foreColor = foreColor;
      this.fontStyle = fontStyle;
      this.imageKey = imageKey;
      this.filterResult = filterResult;

      this.boundsValid = false;
      this.Nodes = new TreeNodeCollection(this);
   }

   public virtual String Text 
   {
      get { return this.text; }
      set
      {
         this.text = value;
         this.Invalidate();
      }
   }

   public virtual Color BackColor
   {
      get { return this.backColor; }
      set
      {
         this.backColor = value;
         this.Invalidate();
      }
   }

   public virtual Color ForeColor
   {
      get { return this.foreColor; }
      set
      {
         this.foreColor = value;
         this.Invalidate();
      }
   }

   public virtual FontStyle FontStyle
   {
      get { return this.fontStyle; }
      set
      {
         this.fontStyle = value;
         this.Invalidate();
      }
   }

   public virtual String ImageKey 
   {
      get { return this.imageKey; }
      set
      {
         this.imageKey = value;
         this.Invalidate();
      }
   }

   public void Invalidate()
   {
      this.Invalidate(false);
   }

   public void Invalidate(Boolean recursive)
   {
      if (this.TreeView == null)
         return;

      if (this.TreeView.ClientRectangle.IntersectsWith(this.Bounds))
         this.TreeView.Invalidate(this.Bounds);

      if (recursive)
      {
         foreach (TreeNode tn in this.Nodes)
            tn.Invalidate(recursive);
      }
   }

   /// <summary>
   /// Gets or sets the parent of this node.
   /// </summary>
   /// <remarks>
   /// If the parent node is a root node, null is returned!
   /// This is because the TreeView uses a TreeNode as a (hidden) root.
   /// </remarks>
   public TreeNode Parent 
   {
      get 
      {
         if (this.parent != null && this.parent.parent != null)
            return this.parent;
         else
            return null;
      }
      set
      {
         if (value != this.parent)
         {
            if (this.parent != null)
               this.parent.Nodes.Remove(this);

            if (value != null)
               value.Nodes.Add(this);
         }
      }
   }

   /// <summary>
   /// Gets the TreeView to which this TreeNode belongs.
   /// </summary>
   public TreeView TreeView 
   {
      get { return this.treeview; }
      internal set
      {
         this.treeview = value;
         foreach (TreeNode tn in this.Nodes)
            tn.TreeView = value;
      }
   }

   /// <summary>
   /// Removes the node from the parent's TreeNodeCollection.
   /// </summary>
   public void Remove() 
   {
      if (this.parent != null)
         this.parent.Nodes.Remove(this);
   }

   /// <summary>
   /// Gets the 0-based depth level of the node.
   /// </summary>
   public Int32 Level 
   {
      get
      {
         if (this.parent == null)
            return -1;
         else
            return this.parent.Level + 1;
      }
   }

   /// <summary>
   /// Gets the index of this node within the collection that contains it.
   /// </summary>
   public Int32 Index 
   { 
      get 
      {
         if (this.parent == null)
            return -1;
         else
            return this.parent.Nodes.IndexOf(this);
      } 
   }

   #region Bounds
   
   /// <summary>
   /// Returns the node's bounds without taking the TreeView's scroll positions into account.
   /// </summary>
   internal Rectangle AbsoluteBounds
   {
      get
      {
         if (this.boundsValid)
            return this.bounds;

         if (!this.IsVisible)
            return Rectangle.Empty;

         if (this.TreeView == null || this.TreeView.TreeNodeLayout == null)
            return Rectangle.Empty;

         Int32 y = 0;
         Int32 itemHeight = this.TreeView.TreeNodeLayout.ItemHeight;
         TreeNode tn = this.TreeView.Nodes[0];
         while (tn != this)
         {
            if (tn == null)
               return Rectangle.Empty;

            y += itemHeight;
            tn = tn.NextVisibleNode;
         }

         this.bounds = new Rectangle(0, y, this.TreeView.Width, itemHeight);
         this.boundsValid = true;

         return this.bounds;
      }
   }

   /// <summary>
   /// Gets the bounds of this node. Empty when the node's parent is collapsed.
   /// </summary>
   public Rectangle Bounds 
   { 
      get
      {
         if (this.TreeView == null)
            return Rectangle.Empty;

         Rectangle localBounds = this.AbsoluteBounds;
         localBounds.Offset(-this.TreeView.HorizontalScroll.Value, -this.TreeView.VerticalScroll.Value);
         return localBounds;
      } 
   }

   internal void InvalidateBounds(Boolean includeNextNodes, Boolean includeChildren)
   {
      this.bounds = Rectangle.Empty;
      this.boundsValid = false;

      if (includeChildren)
      {
         foreach (TreeNode tn in this.Nodes)
            tn.InvalidateBounds(includeNextNodes, includeChildren);
      }

      if (includeNextNodes)
      {
         //Loop over next visible nodes (avoiding recursion because of potential stack-overflow).
         TreeNode nextNode = this.NextVisibleNode;
         while (nextNode != null)
         {
            nextNode.InvalidateBounds(false, includeChildren);
            nextNode = nextNode.NextVisibleNode;
         }
      }
   }

   #endregion


   /// <summary>
   /// Gets or sets the expanded state of the node.
   /// </summary>
   public Boolean IsExpanded 
   { 
      get 
      {
         return (this.parent == null) ? true : this.isExpanded;
      }
      set
      {
         TreeNode nextVisibleNode = this.NextVisibleNode;

         if (nextVisibleNode != null)
            nextVisibleNode.InvalidateBounds(true, false);

         this.isExpanded = value;

         if (this.TreeView != null)
            this.TreeView.Update(TreeViewUpdateFlags.Scrollbars | TreeViewUpdateFlags.Redraw);
      }
   }

   /// <summary>
   /// Expands this node and all its childnodes.
   /// </summary>
   public void ExpandAll() 
   {
      if (this.TreeView != null)
         this.TreeView.BeginUpdate(TreeViewUpdateFlags.Scrollbars | TreeViewUpdateFlags.Redraw);
      
      foreach (TreeNode tn in this.Nodes)
         tn.ExpandAll();
      this.IsExpanded = true;

      if (this.TreeView != null)
         this.TreeView.EndUpdate();
   }


   /// <summary>
   /// Tests if this TreeNode is not hidden by a parent being collapsed.
   /// </summary>
   public Boolean IsVisible 
   {
      get 
      {
         //if (this.FilterResult == FilterResults.Hide)
         //   return false;

         TreeNode pn = this.Parent;
         while (pn != null)
         {
            if (!pn.IsExpanded)
               return false;
            pn = pn.Parent;
         }
         return true;
      }
   }


   #region Filter
   
   public FilterResults FilterResult
   {
      get { return this.filterResult; }
      set
      {
         if (this.filterResult != value)
         {
            this.filterResult = value;
            if (this.parent != null)
            {
               this.parent.Nodes.updateFilter(this);
               this.Invalidate();
            }
         }
      }
   }

   internal Boolean HasUnfilteredChildren
   {
      get
      {
         foreach (TreeNode tn in this.Nodes)
         {
            if (tn.FilterResult == FilterResults.Show || tn.HasUnfilteredChildren)
               return true;
         }
         return false;
      }
   }

   #endregion


   #region Next / Previous Node

   /// <summary>
   /// The next node in the parent's node collection. Null if this is the last node.
   /// </summary>
   public TreeNode NextNode { get; internal set; }

   /// <summary>
   /// The previous node in the parent's node collection. Null if this is the first node.
   /// </summary>
   public TreeNode PreviousNode { get; internal set; }

   /// <summary>
   /// Gets the next visible node in the hierarchy.
   /// Null if the node is the last in the visible hierarchy.
   /// </summary>
   public TreeNode NextVisibleNode 
   {
      get 
      {
         if (!this.IsVisible)
            return null;

         if (this.IsExpanded && this.Nodes.Count > 0)
            return this.Nodes[0];

         TreeNode parentNode = this;
         TreeNode nextNode = null;
         while (nextNode == null && parentNode != null)
         {
            nextNode = parentNode.NextNode;
            parentNode = parentNode.parent;
         }
         return nextNode;
      }
   }

   /// <summary>
   /// Gets the previous visible node in the hierarchy.
   /// Null if the node is the first node in the entire hierarchy.
   /// </summary>
   /// <remarks>Current implementation seems to perform poorly...</remarks>
   public TreeNode PreviousVisibleNode 
   {
      get
      {
         if (this.parent == null || !this.IsVisible)
            return null;

         TreeNode prevNode = this.parent;
         TreeNode nextVis  = this.parent.NextVisibleNode;
         while (nextVis != this)
         {
            prevNode = nextVis;
            nextVis = nextVis.NextVisibleNode;
         }
         if (prevNode != null && prevNode.parent != null)
            return prevNode;
         else
            return null;
      }
   }

   #endregion


   #region TreeNodeState

   /// <summary>
   /// Gets or sets the selection state of the treenode.
   /// </summary>
   public TreeNodeStates State
   {
      get { return this.state; }
      set
      {
         this.state = value;
         TreeView tree = this.TreeView;
         if (tree != null)
            tree.Update(TreeViewUpdateFlags.Redraw);
         
         if (value.HasFlag(TreeNodeStates.Selected) || value.HasFlag(TreeNodeStates.ParentOfSelected))
         {
            if (this.parent != null)
               this.parent.State |= TreeNodeStates.ParentOfSelected;
         }
         else if (value.HasFlag(TreeNodeStates.None))
         {
            if (this.parent != null)
               this.parent.State = TreeNodeStates.None;
         }

      }
   }

   /// <summary>
   /// Tests if this TreeNode has the supplied flag set.
   /// </summary>
   public Boolean HasStateFlag(TreeNodeStates stateFlag) 
   {
      return (this.State & stateFlag) == stateFlag;
   }

   /// <summary>
   /// Sets the supplied State flag.
   /// </summary>
   public void SetStateFlag(TreeNodeStates stateFlag) 
   {
      this.State |= stateFlag;
   }

   /// <summary>
   /// Removes the supplied State flag
   /// </summary>
   public void RemoveStateFlag(TreeNodeStates stateFlag) 
   {
      this.State &= ~stateFlag;
   }

   #endregion


   public override string ToString()
   {
      return "TreeNode ( " + this.Text + " )";
   }
}
}
