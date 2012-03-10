using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Max;
using System.Drawing;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.ComponentModel;
using Outliner.Controls.Layout;


namespace Outliner.Controls
{
public class TreeView : System.Windows.Forms.TreeView
{
   public TreeView() : base() 
   {
      this._colors = new TreeViewColors();

      this._tnlayout = TreeNodeLayout.DefaultLayout;
      this._tnlayout.TreeView = this;
      this.Indent    = 15;

      this.SelectedNodes = new HashSet<TreeNode>();

      this._sortQueue = new List<TreeNodeCollection>();

      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

      this.AllowDrop = true;
      this.CheckBoxes = true; //Enable checkboxes to get scrollbars to show up in time.
   }


   protected override void Dispose(bool disposing)
   {
      if (this._backgroundBrush != null)
      {
         this._backgroundBrush.Dispose();
         this._backgroundBrush = null;
      }

      base.Dispose(disposing);
   }


   #region Selection

   public ICollection<TreeNode> SelectedNodes { get; private set; }
   public TreeNode LastSelectedNode { get; private set; }
   public event SelectionChangedEventHandler SelectionChanged;

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
         this.SetNodeColor(tn, TreeNodeColor.Selected);

         if (!this.IsSelectedNode(tn))
            this.SelectedNodes.Add(tn);

//            this.HighlightParentNodes(tn);

         this.LastSelectedNode = tn;
      }
      else
      {
         SelectedNodes.Remove(tn);

         if (IsParentOfSelectedNode(tn, true))
            this.SetNodeColor(tn, TreeNodeColor.ParentOfSelected);
         else
            this.SetNodeColor(tn, TreeNodeColor.Default);

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


   #region Mouse Events

   private const int WM_LBUTTONDOWN  = 0x0201;
   private const int WM_LBUTTONUP    = 0x0202;
   private const int WM_LBUTTONDBCLK = 0x0203;

   protected override void DefWndProc(ref Message m)
   {
      //Block mouse event messages.
      if (   m.Msg == WM_LBUTTONDOWN 
          || m.Msg == WM_LBUTTONUP 
          || m.Msg == WM_LBUTTONDBCLK
         ) return;

      base.DefWndProc(ref m);
   }


   protected override void OnMouseUp(MouseEventArgs e)
   {
      TreeNode tn = this.GetNodeAt(e.X, e.Y);

      if (tn != null)
         this.TnLayout.HandleMouseUp(e, tn);
      else if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
      {
         this.SelectAllNodes(false);
         this.OnSelectionChanged();
      }
   }



   #endregion


   #region Colors

   private TreeViewColors _colors;
   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
   public TreeViewColors Colors
   {
      get { return _colors; }
      set
      {
         _colors = value;
         _backgroundBrush = null;
         this.Invalidate();
      }
   }

   public override Color ForeColor
   {
      get { return this.Colors.NodeForeColor; }
      set { this.Colors.NodeForeColor = value; }
   }

   public override Color BackColor
   {
      get { return this.Colors.BackColor; }
      set { this.Colors.BackColor = value; }
   }

   public new Color LineColor
   {
      get { return this.Colors.LineColor; }
      set { this.Colors.LineColor = value; }
   }

   internal void SetNodeColor(TreeNode tn, TreeNodeColor color)
   {
      switch (color)
      {
         case TreeNodeColor.Default:
            tn.ForeColor = this.Colors.NodeForeColor;//this.GetNodeForeColor(n);
            tn.BackColor = this.Colors.NodeBackColor;//this.GetNodeBackColor(n);
            break;
         case TreeNodeColor.Selected:
            tn.ForeColor = this.Colors.SelectionForeColor;
            tn.BackColor = this.Colors.SelectionBackColor;
            break;
            /*
         case TreeNodeColor.ParentOfSelected:
            if (n is OutlinerObject)
            {
               tn.ForeColor = this.Colors.ParentForeColor;
               tn.BackColor = this.Colors.ParentBackColor;
            }
            else if (n is OutlinerLayer || n is OutlinerMaterial || n is SelectionSet)
            {
               tn.ForeColor = this.Colors.LayerForeColor;
               tn.BackColor = this.Colors.LayerBackColor;
            }
            else
            {
               tn.ForeColor = this.GetNodeForeColor(n);
               tn.BackColor = this.GetNodeBackColor(n);
            }
            break;
            */
         case TreeNodeColor.LinkTarget:
            tn.ForeColor = this.Colors.LinkForeColor;
            tn.BackColor = this.Colors.LinkBackColor;
            break;
      }
   }

   internal void SetNodeColor(TreeNode tn, Color foreColor, Color backColor)
   {
      tn.ForeColor = foreColor;
      tn.BackColor = backColor;
   }

   internal void SetNodeColorAuto(TreeNode tn)
   {
      TreeNodeColor nodeColor;
      if (this.IsSelectedNode(tn))
         nodeColor = TreeNodeColor.Selected;
      else if (this.IsParentOfSelectedNode(tn, true))
         nodeColor = TreeNodeColor.ParentOfSelected;
      else
         nodeColor = TreeNodeColor.Default;

      SetNodeColor(tn, nodeColor);
   }

   #endregion


   #region Paint

   private TreeNodeLayout _tnlayout;
   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
   public TreeNodeLayout TnLayout 
   {
      get { return _tnlayout; }
      set
      {
         _tnlayout = value;
         value.TreeView = this;
         this.Invalidate();
      }
   }

   public void InvalidateTreeNode(TreeNode tn)
   {
      if (tn == null)
         return;

      Rectangle tnBounds = tn.Bounds;
      if (this.ClientRectangle.IntersectsWith(tnBounds))
         this.Invalidate(tnBounds);
   }


   private SolidBrush _backgroundBrush;

   protected override void OnPaintBackground(PaintEventArgs pevent)
   {
      if (_backgroundBrush == null)
         _backgroundBrush = new SolidBrush(this.BackColor);

      pevent.Graphics.FillRectangle(_backgroundBrush, pevent.ClipRectangle);
   }
      
   protected override void OnPaint(PaintEventArgs e)
   {
      if (this.Nodes.Count == 0 || this.TnLayout == null)
         return;
         
      Int32 curY = e.ClipRectangle.Y;
      while (curY <= e.ClipRectangle.Bottom)
      {
         TreeNode tn = this.GetNodeAt(0, curY);

         if (tn == null)
            break;

         Rectangle tnBounds = tn.Bounds;
         if (tnBounds.Width != 0 && tnBounds.Height != 0)
            this.TnLayout.DrawTreeNode(e.Graphics, tn);

         curY += this.ItemHeight;
      }
   }


   #endregion


   #region ScrollPosition

   protected Point ScrollPos
   {
      get { return new Point(this.ScrollPosX, this.ScrollPosY); }
      set 
      {
         this.ScrollPosX = value.X;
         this.ScrollPosY = value.Y;
      }
   }
   protected int ScrollPosX
   {
      get { return NativeMethods.GetScrollPos(this.Handle, 0); }
      set { NativeMethods.SetScrollPos(this.Handle, NativeMethods.SB_HOR, value, true); }
   }
   protected int ScrollPosY
   {
      get { return NativeMethods.GetScrollPos(this.Handle, 1); }
      set { NativeMethods.SetScrollPos(this.Handle, NativeMethods.SB_VER, value, true); }
   }

   #endregion


   #region Sort

   new public IComparer TreeViewNodeSorter
   {
      get { return null; }
      set { }
   }
   new public Boolean Sorted
   {
      get { return false; }
      set { }
   }

   private IComparer<TreeNode> _nodeSorter;
   public IComparer<TreeNode> NodeSorter
   {
      get { return _nodeSorter; }
      set
      {
         _nodeSorter = value;
         this.Sort();
      }
   }
   
   new public void Sort()
   {
      Point scrollPos = this.ScrollPos;

      this.Sort(this.Nodes, true);
   //   this.restoreExpandedStates();
      _sortQueue.Clear();

      this.ScrollPos = scrollPos;
   }
   protected void Sort(TreeNodeCollection nodes, Boolean recursive)
   {
      if (this.NodeSorter == null || nodes == null)
         return;

      if (nodes.Count == 1 && recursive)
         this.Sort(nodes[0].Nodes, true);
      else
      {
         TreeNode[] dest = new TreeNode[nodes.Count];
         nodes.CopyTo(dest, 0);

         Array.Sort<TreeNode>(dest, this.NodeSorter);

         if (recursive)
         {
            foreach (TreeNode tn in dest)
            {
               if (tn.GetNodeCount(false) > 0)
                  this.Sort(tn.Nodes, true);
            }
         }

         nodes.Clear();
         nodes.AddRange(dest);
      }
   }
      
   internal void SortQueue()
   {
      if (_sortQueue == null || _sortQueue.Count == 0)
         return;

      Point scrollPos = this.ScrollPos;

      foreach (TreeNodeCollection nodes in _sortQueue)
      {
         this.Sort(nodes, false);
      }

      _sortQueue.Clear();

      this.ScrollPos = scrollPos;

      //   this.restoreExpandedStates();
   }

   private Timer _sortTimer;
   private Boolean _timedSortQueueOnly;
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

   void _sortTimer_Tick(object sender, EventArgs e)
   {
      _sortTimer.Stop();

      if (_timedSortQueueOnly)
         this.SortQueue();
      else
         this.Sort();
   }

   protected List<TreeNodeCollection> _sortQueue;
   /// <summary>
   /// Adds the TreeNodeCollection to the sort queue.
   /// </summary>
   internal void AddToSortQueue(TreeNodeCollection nodes)
   {
      if (!_sortQueue.Contains(nodes))
         _sortQueue.Add(nodes);
   }
   /// <summary>
   /// Adds the parent's TreeNodeCollection to the sort queue.
   /// </summary>
   internal void AddToSortQueue(TreeNode tn)
   {
      if (tn == null)
         return;

      if (tn.Parent != null)
         this.AddToSortQueue(tn.Parent.Nodes);
      else
         this.AddToSortQueue(this.Nodes);
   }

   #endregion
      
}
}
