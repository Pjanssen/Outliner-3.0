using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Outliner.Controls.Tree.Layout;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using Outliner.Controls.Tree.DragDropHandlers;

namespace Outliner.Controls.Tree
{
public class TreeView : ScrollableControl
{
   private TreeNode root;

   public TreeView()
   {
      //Member initialization.
      this.root = new TreeNode(this, "root");
      this.Colors = new TreeViewColorScheme();
      this.SelectedNodes = new HashSet<TreeNode>();
      this.TreeNodeLayout = TreeNodeLayout.DefaultLayout; //TODO check that this does not cause unnecessary redrawing.

      //Set double buffered user paint style.
      this.SetStyle(ControlStyles.UserPaint, true);
      this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

      this.AutoScroll = true;
      this.AllowDrop = true;
   }


   protected override void Dispose(bool disposing)
   {
      if (this.brushBackground != null)
         this.brushBackground.Dispose();

      base.Dispose(disposing);
   }


   public TreeNodeCollection Nodes
   {
      get { return this.root.Nodes; }
   }


   public TreeNode GetNodeAt(Point location)
   {
      return this.GetNodeAt(location.X, location.Y);
   }
   public TreeNode GetNodeAt(Int32 x, Int32 y)
   {
      if (this.Nodes.Count == 0 || this.TreeNodeLayout == null || !this.ClientRectangle.Contains(x, y))
         return null;

      Int32 itemHeight = this.TreeNodeLayout.ItemHeight;
      TreeNode tn = this.Nodes[0];
      Int32 curY = itemHeight - this.VerticalScroll.Value;
      while (curY <= y && tn != null)
      {
         tn = tn.NextVisibleNode;
         curY += itemHeight;
      }
      return tn;
   }

   protected override CreateParams CreateParams
   {
      get
      {
         const int WS_BORDER = 0x00800000;
         const int WS_EX_STATICEDGE = 0x00020000;
         CreateParams cp = base.CreateParams;
         //switch (_borderStyle)
         //{
         //   case BorderStyle.FixedSingle:
               cp.Style |= WS_BORDER;
         //      break;
         //   case BorderStyle.Fixed3D:
         //      cp.ExStyle |= WS_EX_STATICEDGE;
         //      break;
         //}
         return cp;
      }
   }

   protected override void OnResize(EventArgs e)
   {
      this.Update(TreeViewUpdateFlags.TreeNodeBounds);
   }

   #region Paint

   private TreeNodeLayout treeNodeLayout;
   [Browsable(false)]
   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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



   private SolidBrush brushBackground;

   private TreeViewColorScheme colors;
   [Browsable(false)]
   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
   public TreeViewColorScheme Colors 
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
      get { return this.Colors.Background.Color; }
      set
      {
         this.Colors.Background = new SerializableColor(value);
         this.brushBackground = null;
      }
   }


   internal Color GetNodeForeColor(TreeNode tn)
   {
      if (tn == null || this.Colors == null)
         return Color.Empty;

      if (tn.State.HasFlag(TreeNodeStates.DropTarget))
         return this.Colors.DropTargetForeground.Color;
      if (tn.State.HasFlag(TreeNodeStates.Selected))
         return this.Colors.SelectionForeground.Color;
      if (tn.State.HasFlag(TreeNodeStates.ParentOfSelected))
         return this.Colors.ParentForeground.Color;

      if (tn.ForeColor != Color.Empty)
         return tn.ForeColor;
      else
      {
         float bBack = this.GetNodeBackColor(tn).GetBrightness();
         float bDark = this.Colors.ForegroundDark.Color.GetBrightness();
         float bLight = this.Colors.ForegroundLight.Color.GetBrightness();

         if (Math.Abs(bBack - bDark) > Math.Abs(bBack - bLight))
            return this.Colors.ForegroundDark.Color;
         else
            return this.Colors.ForegroundLight.Color;
      }
   }

   internal Color GetNodeBackColor(TreeNode tn)
   {
      if (tn == null || this.Colors == null || this.TreeNodeLayout == null)
         return Color.Empty;

      Boolean fullRowSelect = this.TreeNodeLayout.FullRowSelect;

      if (fullRowSelect && tn.State.HasFlag(TreeNodeStates.DropTarget))
         return this.Colors.DropTargetBackground.Color;
      if (fullRowSelect && tn.State.HasFlag(TreeNodeStates.Selected))
         return this.Colors.SelectionBackground.Color;
      if (fullRowSelect && tn.State.HasFlag(TreeNodeStates.ParentOfSelected))
         return this.Colors.ParentBackground.Color;

      Color bgColor = this.Colors.Background.Color;
      if (this.TreeNodeLayout.AlternateBackground && (tn.Bounds.Y / this.TreeNodeLayout.ItemHeight) % 2 != 0)
         bgColor = this.Colors.AltBackground.Color;

      if (tn.BackColor != Color.Empty)
         return tn.BackColor;
      else
         return bgColor;
   }



   protected override void OnPaintBackground(PaintEventArgs e)
   {
      if (this.TestUpdateFlag(TreeViewUpdateFlags.Redraw))
         return;

      if (e == null)
         return;

      if (this.brushBackground == null)
         this.brushBackground = new SolidBrush(this.BackColor);

      e.Graphics.FillRectangle(this.brushBackground, e.ClipRectangle);
   }

   protected override void OnPaint(PaintEventArgs e)
   {
      if (this.TestUpdateFlag(TreeViewUpdateFlags.Redraw))
         return;

      if (e == null || this.Nodes.Count == 0 || this.TreeNodeLayout == null)
         return;

      TreeNodeLayout layout = this.TreeNodeLayout;
      Int32 itemHeight = layout.ItemHeight;
      Int32 startY = e.ClipRectangle.Y - (itemHeight - 1);
      Int32 endY = e.ClipRectangle.Bottom;

      TreeNode tn = this.Nodes[0];
      Int32 curY = 0 - this.VerticalScroll.Value;
      while (curY <= endY && tn != null)
      {
         if (curY >= startY)
         {
            if (layout.FullRowSelect || layout.AlternateBackground || layout.UseLayerColors)
            {
               Color bgColor = this.GetNodeBackColor(tn);
               using (SolidBrush bgBrush = new SolidBrush(bgColor))
               {
                  //Color bgGradColor = Color.FromArgb(bgColor.A, Math.Min(bgColor.R + 25, 255), Math.Min(bgColor.G + 25, 255), Math.Min(bgColor.B + 25, 255));
                  //LinearGradientBrush brush = new LinearGradientBrush(tn.Bounds, bgGradColor, bgColor, LinearGradientMode.Vertical);
                  e.Graphics.FillRectangle(bgBrush, tn.Bounds);
                  //Pen lPen = new Pen(Color.FromArgb(bgColor.A, Math.Max(bgColor.R - 10, 0), Math.Max(bgColor.G - 10, 0), Math.Max(bgColor.B - 10, 0)));
                  //e.Graphics.DrawLine(lPen, tn.Bounds.Left, tn.Bounds.Bottom - 1, tn.Bounds.Right, tn.Bounds.Bottom -1);
               }
            }
            
            this.TreeNodeLayout.DrawTreeNode(e.Graphics, tn);
         }
      
         tn = tn.NextVisibleNode;
         curY += itemHeight;
      }
   }

   #endregion


   #region Update

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

         if (this.TestUpdateFlag(TreeViewUpdateFlags.Brushes))
         {
            this.brushBackground = null;
         }

         if (this.TestUpdateFlag(TreeViewUpdateFlags.TreeNodeBounds))
         {
            this.root.InvalidateBounds(false, true);
         }

         if (this.TestUpdateFlag(TreeViewUpdateFlags.Scrollbars))
         {
            this.RemoveUpdateFlag(TreeViewUpdateFlags.Scrollbars);
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

      Int32 itemHeight = this.TreeNodeLayout.ItemHeight;
      Int32 maxWidth = 0;
      Int32 maxHeight = 0;
      TreeNode tn = this.Nodes[0];
      while (tn != null)
      {
         //tn.InvalidateBounds();
         Int32 nodeWidth = this.TreeNodeLayout.GetTreeNodeWidth(tn);
         if (nodeWidth > maxWidth)
            maxWidth = nodeWidth;

         maxHeight += itemHeight;
         tn = tn.NextVisibleNode;
      }
      return new Size(maxWidth + 5, maxHeight);
   }

   #endregion


   #region Mouse Events

   protected override void OnMouseDown(MouseEventArgs e)
   {
      if (e.Button == MouseButtons.Left)
         this.dragStartPos = e.Location;
   }

   protected override void OnMouseMove(MouseEventArgs e)
   {
      if (e == null || this.TreeNodeLayout == null)
         return;

      if (e.Button != MouseButtons.Left)
      {
         this.isDragging = false;
         this.dragStartPos = Point.Empty;
      }

      TreeNode tn = this.GetNodeAt(e.Location);
      if (tn != null)
      {
         if (!this.isDragging && e.Button == MouseButtons.Left
             && HelperMethods.Distance(e.Location, this.dragStartPos) > 5
             && this.SelectedNodes.Count > 0)
         {
            DataObject data = new DataObject(typeof(IEnumerable<TreeNode>).FullName, 
                                             this.SelectedNodes);
            this.DoDragDrop(data, TreeView.AllowedDragDropEffects);
         }
         else
            this.TreeNodeLayout.HandleMouseMove(e, tn);
      }
   }

   protected override void OnMouseClick(MouseEventArgs e)
   {
      if (e == null || this.TreeNodeLayout == null)
         return;

      this.Select(); //Select the treeview to be able to end a potential TreeNodeText::TextEdit.

      TreeNode tn = this.GetNodeAt(e.Location);
      if (tn != null)
         this.TreeNodeLayout.HandleClick(e, tn);
      else if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed && !HelperMethods.AltPressed)
      {
         this.SelectAllNodes(false);
         this.OnSelectionChanged();
      }
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

   #region DragDrop

   private const DragDropEffects AllowedDragDropEffects = DragDropEffects.Copy
                                                        | DragDropEffects.Link
                                                        | DragDropEffects.Move;

   private Point dragStartPos;
   private Boolean isDragging;
   private TreeNode prevDragTarget;
   private ToolTip dragTooltip;

   public DragDropHandler DragDropHandler { get; set; }

   protected override void OnDragEnter(DragEventArgs drgevent)
   {
      this.isDragging = true;
      drgevent.Effect = DragDropEffects.None;
      
      base.OnDragEnter(drgevent);
   }

   protected override void OnDragLeave(EventArgs e)
   {
      if (this.prevDragTarget != null)
         prevDragTarget.RemoveStateFlag(TreeNodeStates.DropTarget);

      base.OnDragLeave(e);
   }

   protected override void OnDragOver(DragEventArgs drgevent)
   {
      Point location = this.PointToClient(new Point(drgevent.X, drgevent.Y));
      TreeNode tn = this.GetNodeAt(location);
      DragDropHandler dragDropHandler = (tn != null) ? tn.DragDropHandler 
                                                     : this.DragDropHandler;

      if (prevDragTarget != null && prevDragTarget != tn)
      {
         prevDragTarget.RemoveStateFlag(TreeNodeStates.DropTarget);
         prevDragTarget = null;
      }

      if (dragDropHandler != null && dragDropHandler.IsValidDropTarget(drgevent.Data))
      {
         drgevent.Effect = dragDropHandler.GetDragDropEffect(drgevent.Data);

         if (tn != null)
         {
            tn.SetStateFlag(TreeNodeStates.DropTarget);
            prevDragTarget = tn;
         }
      }
      else
         drgevent.Effect = DragDropEffects.None;

      base.OnDragOver(drgevent);
   }

   protected override void OnDragDrop(DragEventArgs drgevent)
   {
      Point location = this.PointToClient(new Point(drgevent.X, drgevent.Y));
      TreeNode tn = this.GetNodeAt(location);

      DragDropHandler dragDropHandler = (tn != null) ? tn.DragDropHandler
                                                     : this.DragDropHandler;

      if (prevDragTarget != null)
      {
         prevDragTarget.RemoveStateFlag(TreeNodeStates.DropTarget);
         prevDragTarget = null;
      }

      if (dragDropHandler != null && dragDropHandler.IsValidDropTarget(drgevent.Data))
      {
         dragDropHandler.HandleDrop(drgevent.Data);
      }
      
      base.OnDragDrop(drgevent);
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

   [Browsable(false)]
   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

         this.BeginUpdate(TreeViewUpdateFlags.Redraw | TreeViewUpdateFlags.Scrollbars);

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
   public void StartTimedSort(Boolean queueOnly)
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
      if (nodes == null)
         return;

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
   /// <summary>
   /// Adds all TreeNodes in the collection to the sort queue.
   /// </summary>
   public void AddToSortQueue(IEnumerable<TreeNode> nodes)
   {
      if (nodes == null)
         return;

      nodes.ForEach(AddToSortQueue);
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

      this.BeginNodeTextEdit(tn, this.TreeNodeLayout.LayoutItems.FirstOrDefault(item => item is TreeNodeText));
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
   None           = 0x00,
   Redraw         = 0x01,
   TreeNodeBounds = 0x02,
   Scrollbars     = 0x04,
   Brushes        = 0x08,
   All            = Redraw | TreeNodeBounds | Scrollbars | Brushes
}
}
