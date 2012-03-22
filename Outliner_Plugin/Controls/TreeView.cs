﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Outliner.Controls.Layout;

namespace Outliner.Controls
{
public class TreeView : ScrollableControl
{
   private TreeNode root;

   public TreeView()
   {
      //Member initialization.
      this.root = new TreeNode(this, "root");
      this.itemHeight = 18;
      this.Colors = new TreeViewColors();
      this.SelectedNodes = new HashSet<TreeNode>();

      //Set double buffered user paint style.
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

      //Initial scroll values.
      this.AutoScroll = true;
      this.VerticalScroll.SmallChange = this.itemHeight;
      this.VerticalScroll.LargeChange = this.ItemHeight * 3;
   }


   protected override void Dispose(bool disposing)
   {
      if (this.bgBrush != null)
         this.bgBrush.Dispose();

      base.Dispose(disposing);
   }


   public TreeNodeCollection Nodes
   {
      get
      {
         return this.root.Nodes;
      }
   }

   private Int32 itemHeight;
   public Int32 ItemHeight 
   {
      get { return this.itemHeight; }
      set
      {
         this.itemHeight = value;
         this.Update(TreeViewUpdateFlags.Redraw | TreeViewUpdateFlags.Bounds);
      }
   }


   public TreeNode GetNodeAt(Point location)
   {
      return this.GetNodeAt(location.X, location.Y);
   }
   public TreeNode GetNodeAt(Int32 x, Int32 y)
   {
      if (this.Nodes.Count == 0 || !this.ClientRectangle.Contains(x, y))
         return null;

      TreeNode tn = this.Nodes[0];
      Int32 curY = this.ItemHeight - this.VerticalScroll.Value;
      while (curY <= y && tn != null)
      {
         tn = tn.NextVisibleNode;
         curY += this.ItemHeight;
      }
      return tn;
   }


   #region Paint

   private TreeNodeLayout treeNodeLayout;
   public TreeNodeLayout TreeNodeLayout 
   {
      get { return this.treeNodeLayout; }
      set
      {
         if (value == null)
            throw new ArgumentNullException("value");

         this.treeNodeLayout = value;
         this.treeNodeLayout.TreeView = this;
      }
   }

   public void Invalidate(TreeNode tn)
   {
      if (tn == null)
         return;

      Rectangle tnBounds = tn.Bounds;
      if (this.ClientRectangle.IntersectsWith(tnBounds))
         this.Invalidate(tnBounds);
   }



   private SolidBrush bgBrush;

   private TreeViewColors colors;
   public Outliner.Controls.TreeViewColors Colors 
   {
      get { return this.colors; }
      set
      {
         if (value == null)
            throw new ArgumentNullException("value");
         this.colors = value;
         this.Update(TreeViewUpdateFlags.Redraw);
      }
   }

   public override Color BackColor
   {
      get { return this.Colors.BackColor; }
      set
      {
         this.Colors.BackColor = value;
         this.bgBrush = null;
      }
   }

   protected override void OnPaintBackground(PaintEventArgs e)
   {
      if (this.TestUpdateFlag(TreeViewUpdateFlags.Redraw))
         return;

      if (e == null)
         return;

      if (this.bgBrush == null)
         this.bgBrush = new SolidBrush(this.BackColor);

      e.Graphics.FillRectangle(this.bgBrush, e.ClipRectangle);
   }

   protected override void OnPaint(PaintEventArgs e)
   {
      if (this.TestUpdateFlag(TreeViewUpdateFlags.Redraw))
         return;

      if (e == null || this.Nodes.Count == 0 || this.TreeNodeLayout == null)
         return;
      
      Int32 startY = e.ClipRectangle.Y - (this.ItemHeight - 1);
      Int32 endY = e.ClipRectangle.Bottom;

      TreeNode tn = this.Nodes[0];
      Int32 curY = 0 - this.VerticalScroll.Value;
      while (curY <= endY && tn != null)
      {
         if (curY >= startY)
         {
            Console.WriteLine(tn.ToString() + ", " + tn.Bounds.ToString());
            this.TreeNodeLayout.DrawTreeNode(e.Graphics, tn);
         }
      
         tn = tn.NextVisibleNode;
         curY += this.ItemHeight;
      }
   }

   #endregion


   #region Update

   public void InvalidateTreeNode(TreeNode tn)
   {
      if (tn == null)
         return;

      this.Invalidate(tn.Bounds);
   }

   private Int32 beginUpdateCalls = 0;
   private TreeViewUpdateFlags updating = TreeViewUpdateFlags.None;

   /// <summary>
   /// Tests if one or more update flags are set.
   /// </summary>
   private Boolean TestUpdateFlag(TreeViewUpdateFlags flag)
   {
      return (this.updating & flag) == flag;
   }

   /// <summary>
   /// Sets one or more update flags.
   /// </summary>
   private void SetUpdateFlag(TreeViewUpdateFlags flag)
   {
      this.updating |= flag;
   }

   /// <summary>
   /// Unsets one or more update flags
   /// </summary>
   private void RemoveUpdateFlag(TreeViewUpdateFlags flag)
   {
      this.updating &= ~flag;
   }

   /// <summary>
   /// Updates the TreeView according to the supplied update flags. 
   /// If BeginUpdate was called before, the flags are added to the update queue.
   /// </summary>
   public void Update(TreeViewUpdateFlags flags)
   {
      this.SetUpdateFlag(flags);
      if (this.beginUpdateCalls == 0)
         this.EndUpdate();
   }

   /// <summary>
   /// Stops the TreeView updates until EndUpdate is called, then updates using TreeViewUpdate.All.
   /// </summary>
   public void BeginUpdate()
   {
      this.BeginUpdate(TreeViewUpdateFlags.All);
   }

   /// <summary>
   /// Stops the TreeView updates until EndUpdate is called, then updates according to the provided update flags.
   /// </summary>
   public void BeginUpdate(TreeViewUpdateFlags flags)
   {
      this.updating = flags;
      this.beginUpdateCalls += 1;
   }

   /// <summary>
   /// Resumes updating the TreeView, and processes all update flags set using Update or BeginUpdate.
   /// </summary>
   public void EndUpdate()
   {
      this.beginUpdateCalls -= 1;
      if (this.beginUpdateCalls <= 0)
      {
         this.beginUpdateCalls = 0;

         if (this.TestUpdateFlag(TreeViewUpdateFlags.Bounds))
         {
            this.RemoveUpdateFlag(TreeViewUpdateFlags.Bounds);
            this.AutoScrollMinSize = this.getMaxBounds();
         }

         if (this.TestUpdateFlag(TreeViewUpdateFlags.Redraw))
         {
            this.RemoveUpdateFlag(TreeViewUpdateFlags.Redraw);
            this.Invalidate();
         }

         this.RemoveUpdateFlag(TreeViewUpdateFlags.All);
      }
   }

   private Size getMaxBounds()
   {
      if (this.TreeNodeLayout == null || this.Nodes.Count == 0)
         return Size.Empty;

      Int32 maxWidth = 0;
      Int32 maxHeight = 0;
      TreeNode tn = this.Nodes[0];
      while (tn != null)
      {
         //tn.InvalidateBounds();
         Int32 nodeWidth = this.TreeNodeLayout.GetTreeNodeWidth(tn);
         if (nodeWidth > maxWidth)
            maxWidth = nodeWidth;

         maxHeight += this.ItemHeight;
         tn = tn.NextVisibleNode;
      }
      return new Size(maxWidth + 5, maxHeight);
   }

   #endregion


   #region Mouse Events
   
   protected override void OnMouseMove(MouseEventArgs e)
   {
      if (e == null || this.TreeNodeLayout == null)
         return;

      TreeNode tn = this.GetNodeAt(e.Location);
      if (tn != null)
         this.TreeNodeLayout.HandleMouseMove(e, tn);
   }

   protected override void OnMouseClick(MouseEventArgs e)
   {
      if (e == null || this.TreeNodeLayout == null)
         return;

      this.Select(); //Select the treeview to be able to end a potential TreeNodeText::TextEdit.

      TreeNode tn = this.GetNodeAt(e.Location);
      if (tn == null)
         return;

      this.TreeNodeLayout.HandleClick(e, tn);
   }

   protected override void OnMouseDoubleClick(MouseEventArgs e)
   {
      if (e == null || this.TreeNodeLayout == null)
         return;

      TreeNode tn = this.GetNodeAt(e.X, e.Y);

      if (tn != null)
         this.TreeNodeLayout.HandleMouseDoubleClick(e, tn);
   }

   #endregion


   #region Selection

   public ICollection<TreeNode> SelectedNodes { get; private set; }
   public TreeNode LastSelectedNode { get; private set; }
   public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

   internal void OnSelectionChanged()
   {
      if (this.SelectionChanged != null)
         this.SelectionChanged(this, new SelectionChangedEventArgs(this.SelectedNodes));
   }

   public void SelectNode(TreeNode tn, Boolean select)
   {
      if (tn == null)
         return;

      if (select)
      {
         tn.State = TreeNodeStates.Selected;

         if (!this.IsSelectedNode(tn))
            this.SelectedNodes.Add(tn);

         //            this.HighlightParentNodes(tn);

         this.LastSelectedNode = tn;
      }
      else
      {
         SelectedNodes.Remove(tn);

         if (IsParentOfSelectedNode(tn, true))
            tn.State = TreeNodeStates.ParentOfSelected;
         else
            tn.State = TreeNodeStates.None;

         //            this.RemoveParentHighlights(tn);
      }
   }

   public void SelectAllNodes(Boolean select)
   {
      if (select)
         this.SelectAllNodes(select, this.Nodes);
      else
      {
         List<TreeNode> selNodes = this.SelectedNodes.ToList();
         foreach (TreeNode tn in selNodes)
         {
            this.SelectNode(tn, false);
         }
      }
      this.LastSelectedNode = null;
   }
   private void SelectAllNodes(Boolean select, TreeNodeCollection nodes)
   {
      foreach (TreeNode tn in nodes)
      {
         SelectNode(tn, select);
         this.SelectAllNodes(select, tn.Nodes);
      }
   }

   public void SelectNodesInsideRange(TreeNode startNode, TreeNode endNode)
   {
      if (startNode == null || endNode == null)
         return;

      // Calculate start node and end node
      TreeNode firstNode = startNode;
      TreeNode lastNode = endNode;
      if (startNode.Bounds.Y > endNode.Bounds.Y)
      {
         firstNode = endNode;
         lastNode = startNode;
      }

      // Select each node in range
      TreeNode tnTemp = firstNode;
      while (tnTemp != lastNode && tnTemp != null)
      {
         if (tnTemp != null)
         {
            SelectNode(tnTemp, true);
            tnTemp = tnTemp.NextVisibleNode;
         }
      }
      SelectNode(lastNode, true);
   }

   public Boolean IsSelectedNode(TreeNode tn)
   {
      if (tn == null)
         return false;

      return SelectedNodes.Contains(tn);
   }

   public Boolean IsParentOfSelectedNode(TreeNode tn, Boolean includeChildren)
   {
      if (tn == null || this.IsSelectedNode(tn))
         return false;

      foreach (TreeNode sn in SelectedNodes)
      {
         if (!includeChildren)
         {
            if (sn.Parent == tn)
               return true;
         }
         else
         {
            TreeNode pn = sn.Parent;
            while (pn != null)
            {
               if (pn == tn)
                  return true;
               pn = pn.Parent;
            }
         }
      }

      return false;
   }

   public Boolean IsChildOfSelectedNode(TreeNode tn)
   {
      if (tn == null)
         return false;

      TreeNode parentNode = tn.Parent;
      while (parentNode != null)
      {
         if (IsSelectedNode(parentNode))
            return true;

         parentNode = parentNode.Parent;
      }

      return false;
   }

   #endregion


   #region Sort

   public IComparer<TreeNode> NodeSorter { get; set; }
   private List<TreeNodeCollection> _sortQueue;
   private Timer _sortTimer;
   private Boolean _timedSortQueueOnly;
   
   /// <summary>
   /// Sorts all nodes in the tree.
   /// </summary>
   public void Sort()
   {
      this.Sort(false);
   }

   /// <summary>
   /// Sorts the nodes in this TreeView.
   /// </summary>
   /// <param name="queueOnly">If true, only the nodes in the sortqueue will be sorted, otherwise all nodes are sorted.</param>
   public void Sort(Boolean queueOnly)
   {
      if (!queueOnly)
         this.Nodes.Sort(this.NodeSorter, true);
      else
      {
         if (this._sortQueue == null || this._sortQueue.Count == 0)
            return;

         this.BeginUpdate(TreeViewUpdateFlags.Redraw | TreeViewUpdateFlags.Bounds);

         foreach (TreeNodeCollection nodes in this._sortQueue)
         {
            nodes.Sort(this.NodeSorter, false);
         }

         this.EndUpdate();
      }
      
      if(this._sortQueue != null)
        this. _sortQueue.Clear();
   }

   /// <summary>
   /// Sorts the TreeView after a certain amount of time has expired without this method being called.
   /// </summary>
   /// <param name="queueOnly">Sort is applied only to nodes in the sortqueue if true, or to all nodes otherwise.</param>
   public void TimedSort(Boolean queueOnly)
   {
      if (_sortTimer == null)
      {
         _sortTimer = new Timer();
         _sortTimer.Tick += new EventHandler(_sortTimer_Tick);
      }
      _sortTimer.Interval = 100;
      _sortTimer.Start();

      _timedSortQueueOnly = queueOnly;
   }

   private void _sortTimer_Tick(object sender, EventArgs e)
   {
      _sortTimer.Stop();

      this.Sort(this._timedSortQueueOnly);
   }

   /// <summary>
   /// Adds the TreeNodeCollection to the sort queue.
   /// </summary>
   public void AddToSortQueue(TreeNodeCollection nodes)
   {
      if (this._sortQueue == null)
         this._sortQueue = new List<TreeNodeCollection>();

      if (!this._sortQueue.Contains(nodes))
         this._sortQueue.Add(nodes);
   }
   /// <summary>
   /// Adds the parent's TreeNodeCollection to the sort queue.
   /// </summary>
   public void AddToSortQueue(TreeNode tn)
   {
      if (tn == null)
         return;

      if (tn.Parent != null)
         this.AddToSortQueue(tn.Parent.Nodes);
      else
         this.AddToSortQueue(this.Nodes);
   }

   #endregion
 

   #region NodeTextEdit

   /// <summary>
   /// Occurs before a TreeNode's text is going to be edited.
   /// </summary>
   public event EventHandler<BeforeNodeTextEditEventArgs> BeforeNodeTextEdit;
   protected void OnBeforeNodeTextEdit(BeforeNodeTextEditEventArgs e)
   {
      if (this.BeforeNodeTextEdit != null)
         this.BeforeNodeTextEdit(this, e);
   }

   /// <summary>
   /// Occurs after a TreeNode's text has been edited.
   /// </summary>
   //public event AfterNodeTextEditEventHandler AfterNodeTextEdit;
   public event EventHandler<AfterNodeTextEditEventArgs> AfterNodeTextEdit;
   protected void OnAfterNodeTextEdit(AfterNodeTextEditEventArgs e)
   {
      if (this.AfterNodeTextEdit != null)
         this.AfterNodeTextEdit(this, e);
   }

   private TextBox editTextBox;
   private TreeNode editingTreeNode;

   public void BeginNodeTextEdit(TreeNode tn)
   {
      if (this.TreeNodeLayout == null)
         return;

      this.BeginNodeTextEdit(tn, this.TreeNodeLayout.FirstOrDefault(item => item is TreeNodeText));
   }

   internal void BeginNodeTextEdit(TreeNode tn, TreeNodeLayoutItem layoutItem)
   {
      if (tn == null || layoutItem == null)
         return;

      BeforeNodeTextEditEventArgs e = new BeforeNodeTextEditEventArgs(tn);
      this.OnBeforeNodeTextEdit(e);

      if (e.Cancel)
         return;

      this.editingTreeNode = tn;
      Rectangle bounds = layoutItem.GetBounds(tn);
      this.editTextBox = new TextBox();
      this.editTextBox.Parent = this;
      this.editTextBox.Location = bounds.Location;
      this.editTextBox.Size = new Size (Math.Max(100, bounds.Width), 18);
      this.editTextBox.Text = e.EditText;
      this.editTextBox.SelectAll();
      this.editTextBox.KeyDown += new KeyEventHandler(editTextBox_KeyDown);
      this.editTextBox.LostFocus += new EventHandler(editTextBox_LostFocus);

      this.editTextBox.Show();
      this.editTextBox.Focus();
   }

   private void editTextBox_LostFocus(object sender, EventArgs e)
   {
      this.EndNodeTextEdit(true);
   }

   private void editTextBox_KeyDown(object sender, KeyEventArgs e)
   {
      if (e.KeyCode.HasFlag(Keys.Enter))
         this.EndNodeTextEdit(false);
      else if (e.KeyCode.HasFlag(Keys.Escape))
         this.EndNodeTextEdit(true);
   }

   public void EndNodeTextEdit(Boolean cancel)
   {
      String oldText = null;
      String newText = null;

      if (!cancel && this.editTextBox != null
                  && this.editingTreeNode != null
                  && !String.IsNullOrEmpty(this.editTextBox.Text)
                  && this.editTextBox.Text != this.editingTreeNode.Text)
      {
         oldText = this.editingTreeNode.Text;
         newText = this.editTextBox.Text;
         this.editingTreeNode.Text = this.editTextBox.Text;
      }

      this.OnAfterNodeTextEdit(new AfterNodeTextEditEventArgs(this.editingTreeNode, cancel, oldText, newText));

      this.editTextBox.KeyDown -= editTextBox_KeyDown;
      this.editTextBox.LostFocus -= editTextBox_LostFocus;

      this.Controls.Remove(this.editTextBox);
      this.editTextBox.Dispose();
      this.editTextBox = null;
      this.editingTreeNode = null;
   }

   #endregion
}

[Flags]
public enum TreeViewUpdateFlags
{
   None = 0x00,
   Redraw = 0x01,
   Bounds = 0x02,
   All = 0x03 //When adding new flags, adjust this value.
}
}
