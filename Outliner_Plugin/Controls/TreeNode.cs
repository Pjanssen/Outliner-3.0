﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.FiltersBase;
using System.Drawing;

namespace Outliner.Controls
{
public class TreeNode
{
   internal TreeNode parent;
   private TreeNodeState state;
   private Boolean isExpanded;
   private String text;
   private Boolean boundsValid;
   private Rectangle bounds;

   public TreeView TreeView { get; internal set; }
   public TreeNodeCollection Nodes { get; private set; }
   public DragDropHandler DragDropHandler { get; set; }
   public FilterResult FilterResult { get; set; }
   public Object Tag { get; set; }
   public String ImageKey { get; set; }

   public TreeNode() : this("") { }
   internal TreeNode(TreeView tree, String text) : this(text)
   {
      this.TreeView = tree;
   }

   public TreeNode(String text)
   {
      this.text = text;
      this.boundsValid = false;
      this.Nodes = new TreeNodeCollection(this);
      this.FilterResult = FiltersBase.FilterResult.Show;
   }

   public String Text 
   {
      get { return this.text; }
      set
      {
         this.text = value;
         TreeView tree = this.TreeView;
         if (tree != null)
            tree.Invalidate(this);
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
         if (this.parent != null)
            this.parent.Nodes.Remove(this);

         if (value != null)
            value.Nodes.Add(this);
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

         if (this.TreeView == null)
            return Rectangle.Empty;

         Int32 y = 0;
         Int32 itemHeight = this.TreeView.ItemHeight;
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
   internal void InvalidateBounds()
   {
      this.boundsValid = false;
   }

   /// <summary>
   /// Gets or sets the expanded state of the node.
   /// </summary>
   public Boolean IsExpanded 
   { 
      get 
      {
         if (this.parent == null)
            return true;
         else
            return this.isExpanded;
      }
      set
      {
         this.isExpanded = value;
         TreeView tree = this.TreeView;
         if (tree != null)
         {
            tree.Update(TreeViewUpdate.Bounds | TreeViewUpdate.Redraw);
         }
      }
   }

   /// <summary>
   /// Expands this node and all its childnodes.
   /// </summary>
   public void ExpandAll() 
   {
      foreach (TreeNode tn in this.Nodes)
         tn.ExpandAll();
      this.IsExpanded = true;
   }

   /// <summary>
   /// Tests if this TreeNode is not hidden by a parent being collapsed.
   /// </summary>
   public Boolean IsVisible 
   {
      get 
      {
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
         if (this.IsExpanded && this.Nodes.Count > 0)
            return this.Nodes[0];

         TreeNode parent = this;
         TreeNode nextNode = null;
         while (nextNode == null && parent != null)
         {
            nextNode = parent.NextNode;
            parent = parent.parent;
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
         if (this.parent == null)
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

   /// <summary>
   /// Gets or sets the selection state of the treenode.
   /// </summary>
   public TreeNodeState State
   {
      get { return this.state; }
      set
      {
         this.state = value;
         TreeView tree = this.TreeView;
         if (tree != null)
            tree.Update(TreeViewUpdate.Redraw);
         
         if (value.HasFlag(TreeNodeState.Selected) || value.HasFlag(TreeNodeState.ParentOfSelected))
         {
            if (this.parent != null)
               this.parent.State |= TreeNodeState.ParentOfSelected;
         }
         else if (value.HasFlag(TreeNodeState.Unselected))
         {
            if (this.parent != null)
               this.parent.State = TreeNodeState.Unselected;
         }

      }
   }

   public override string ToString()
   {
      return "TreeNode ( " + this.Text + " )";
   }
}
}
