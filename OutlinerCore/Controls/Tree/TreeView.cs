using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Outliner.Controls.Tree.Layout;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Outliner.Controls.Tree
{
public class TreeView : ScrollableControl
{
   public TreeNode Root { get; private set; }

   public TreeView()
   {
      //Member initialization.
      this.Root = new TreeNode(this, "root");
      this.Colors = new TreeViewColorScheme();
      this.selectedNodes = new HashSet<TreeNode>();
      this.TreeNodeLayout = TreeNodeLayout.SimpleLayout; //TODO check that this does not cause unnecessary redrawing.

      //Set double buffered user paint style.
      this.SetStyle(ControlStyles.UserPaint, true);
      this.DoubleBuffered = true;

      this.AutoScroll = true;
      this.AutoScrollMinSize = this.Size;
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
      get { return this.Root.Nodes; }
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

   private BorderStyle borderStyle = BorderStyle.FixedSingle;
   public BorderStyle BorderStyle
   {
      get { return this.borderStyle; }
      set
      {
         this.borderStyle = value;
         this.Invalidate();
      }
   }
   protected override CreateParams CreateParams
   {
      get
      {
         const int WS_BORDER = 0x00800000;
         const int WS_EX_STATICEDGE = 0x00020000;
         CreateParams cp = base.CreateParams;
         switch (this.BorderStyle)
         {
            case BorderStyle.FixedSingle:
               cp.Style |= WS_BORDER;
               break;
            case BorderStyle.Fixed3D:
               cp.ExStyle |= WS_EX_STATICEDGE;
               break;
         }
         return cp;
      }
   }


   #region Colors

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

   public Int32 GetNodeOpacity(TreeNode tn)
   {
      if (tn == null)
         return 0;

      if (tn.ShowNode)
         return 255;
      else
         return TreeNode.FilteredNodeOpacity;
   }

   public Color GetLineColor(TreeNode tn)
   {
      if (this.TreeNodeLayout.FullRowSelect)
         return GetNodeForeColor(tn, true);
      else
      {
         if (tn.ForeColor != Color.Empty)
            return tn.ForeColor;
         else
         {
            float bBack = this.GetNodeBackColor(tn, false).GetBrightness();
            float bDark = this.Colors.ForegroundDark.Color.GetBrightness();
            float bLight = this.Colors.ForegroundLight.Color.GetBrightness();

            if (Math.Abs(bBack - bDark) > Math.Abs(bBack - bLight))
               return this.Colors.ForegroundDark.Color;
            else
               return this.Colors.ForegroundLight.Color;
         }
      }
   }

   /// <summary>
   /// Gets the foreground color for a treenode.
   /// </summary>
   /// <param name="highlight">True if the foreground color is part of a highlight area (e.g. selection, drop-target)</param>
   public Color GetNodeForeColor(TreeNode tn, Boolean highlight)
   {
      if (tn == null || this.Colors == null)
         return Color.Empty;

      if (tn.HasStateFlag(TreeNodeStates.DropTarget))
         return this.Colors.DropTargetForeground.Color;
      else if (tn.HasStateFlag(TreeNodeStates.Selected))
         return  this.Colors.SelectionForeground.Color;
      else if (tn.HasStateFlag(TreeNodeStates.ParentOfSelected))
         return this.Colors.ParentForeground.Color;

      Color color;
      if (tn.ForeColor != Color.Empty)
         color = tn.ForeColor;
      else
      {
         float bBack = this.GetNodeBackColor(tn, highlight).GetBrightness();
         float bDark = this.Colors.ForegroundDark.Color.GetBrightness();
         float bLight = this.Colors.ForegroundLight.Color.GetBrightness();

         if (Math.Abs(bBack - bDark) > Math.Abs(bBack - bLight))
            color = this.Colors.ForegroundDark.Color;
         else
            color = this.Colors.ForegroundLight.Color;
      }

      return Color.FromArgb(this.GetNodeOpacity(tn), color);
   }


   /// <summary>
   /// Gets the background color for a treenode.
   /// </summary>
   /// <param name="highlight">True if the background color is part of a highlight area (e.g. selection, drop-target)</param>
   public Color GetNodeBackColor(TreeNode tn, Boolean highlight)
   {
      if (tn == null || this.Colors == null)
         return Color.Empty;

      if (highlight && tn.HasStateFlag(TreeNodeStates.DropTarget))
         return this.Colors.DropTargetBackground.Color;
      if (highlight && tn.HasStateFlag(TreeNodeStates.Selected))
         return this.Colors.SelectionBackground.Color;
      if (highlight && tn.HasStateFlag(TreeNodeStates.ParentOfSelected))
         return this.Colors.ParentBackground.Color;

      if (highlight && !tn.BackColor.IsEmpty)
         return tn.BackColor;
      else if (this.Colors.AlternateBackground && this.isAlternatingTn(tn))
         return this.Colors.AltBackground.Color;
      else
         return this.Colors.Background.Color;
   }

   private Boolean isAlternatingTn(TreeNode tn)
   {
      return ((tn.Bounds.Y - this.VerticalScroll.Value) / this.TreeNodeLayout.ItemHeight) % 2 != 0;
   }

   #endregion


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

      if (e == null || this.Nodes.Count == 0 
                    || this.TreeNodeLayout == null
                    || this.Colors == null)
         return;
      
      TreeNodeLayout layout = this.TreeNodeLayout;
      Int32 itemHeight = layout.ItemHeight;
      Int32 curY = e.ClipRectangle.Top;
      Int32 endY = e.ClipRectangle.Bottom + itemHeight;
      TreeNode tn = this.GetNodeAt(e.ClipRectangle.Location);
      Boolean fullRowSelect = this.TreeNodeLayout.FullRowSelect;

      while (curY < endY && tn != null)
      {
         Color bgColor = this.GetNodeBackColor(tn, fullRowSelect);
         using (SolidBrush brush = new SolidBrush(bgColor))
         {
            e.Graphics.FillRectangle(brush, tn.Bounds);
         }

         this.TreeNodeLayout.DrawTreeNode(e.Graphics, tn);

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
            this.Root.InvalidateBounds(false, true);
         }

         if (this.TestUpdateFlag(TreeViewUpdateFlags.Scrollbars))
         {
            this.RemoveUpdateFlag(TreeViewUpdateFlags.Scrollbars);
            this.AutoScrollMinSize = this.getMaxBounds();
            this.AutoScroll = true;
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
         //Int32 nodeWidth = this.TreeNodeLayout.GetTreeNodeWidth(tn);
         //if (nodeWidth > maxWidth)
         //   maxWidth = nodeWidth;

         maxHeight += itemHeight;
         tn = tn.NextVisibleNode;
      }
      
      maxWidth = this.Width - 2;
      if (this.VerticalScroll.Visible)
         maxWidth -= SystemInformation.VerticalScrollBarWidth;

      return new Size(0, maxHeight);
   }

   protected override void OnResize(EventArgs e)
   {
      this.Root.InvalidateBounds(false, true);

      base.OnResize(e);
   }

   #endregion


   #region Mouse Events

   protected override void OnMouseDown(MouseEventArgs e)
   {
      if (e == null || this.TreeNodeLayout == null)
         return;

      if (e.Button == MouseButtons.Left)
         this.dragStartPos = e.Location;

      TreeNode tn = this.GetNodeAt(e.Location);
      if (tn != null)
         this.TreeNodeLayout.HandleMouseDown(e, tn);
      
      base.OnMouseDown(e);
   }

   protected override void OnMouseUp(MouseEventArgs e)
   {
      if (e == null || this.TreeNodeLayout == null)
         return;

      this.Select(); //Select the treeview to be able to end a potential TreeNodeText::TextEdit.

      TreeNode tn = this.GetNodeAt(e.Location);
      if (tn != null)
         this.TreeNodeLayout.HandleMouseUp(e, tn);
      else if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed && !HelperMethods.AltPressed)
      {
         int selCount = this.selectedNodes.Count;
         this.SelectAllNodes(false);
         if (selCount > 0)
            this.OnSelectionChanged();
      }

      if ((e.Button & MouseButtons.Right) == MouseButtons.Right && this.ContextMenu != null)
      {
         this.ContextMenu.Show(this, e.Location);
      }

      base.OnMouseUp(e);
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

      //Start dragging.
      if (tn != null && !this.isDragging
          && e.Button == MouseButtons.Left
          && HelperMethods.Distance(e.Location, this.dragStartPos) > 5
          && this.selectedNodes.Count > 0)
      {
         this.isDragging = true;
         if (tn.DragDropHandler != null && tn.DragDropHandler.AllowDrag)
         {
            DataObject data = new DataObject( typeof(IEnumerable<TreeNode>).FullName
                                            , this.selectedNodes);
            this.DoDragDrop(data, TreeView.AllowedDragDropEffects);
         }
      }
      else
         this.TreeNodeLayout.HandleMouseMove(e, tn);
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

   /// <summary>
   /// The allowed DragDropEffects to use in a drag action.
   /// </summary>
   private const DragDropEffects AllowedDragDropEffects = DragDropEffects.Copy
                                                        | DragDropEffects.Link
                                                        | DragDropEffects.Move;
   
   /// <summary>
   /// Use DragDropEffects.Scroll instead of DragDropEffects.None, 
   /// otherwise the OnDragDrop event won't be raised.
   /// </summary>
   public const DragDropEffects NoneDragDropEffects = DragDropEffects.Scroll;

   private Point dragStartPos;
   private Boolean isDragging;
   private TreeNode prevDragTarget;

   /// <summary>
   /// The DragDropHandler for the TreeView.
   /// </summary>
   [Browsable(false)]
   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
   public DragDropHandler DragDropHandler { get; set; }

   protected override void OnDragEnter(DragEventArgs drgevent)
   {
      if (drgevent == null)
         throw new ArgumentNullException("drgevent");

      this.isDragging = true;
      drgevent.Effect = TreeView.NoneDragDropEffects;
      
      base.OnDragEnter(drgevent);
   }

   protected override void OnDragLeave(EventArgs e)
   {
      if (this.prevDragTarget != null)
         prevDragTarget.RemoveStateFlag(TreeNodeStates.DropTarget);

      base.OnDragLeave(e);
   }

   private DragDropHandler getDragDropHandler(TreeNode tn, Point location)
   {
      TreeNodeLayoutItem layoutItem = this.TreeNodeLayout.GetItemAt(tn, location);

      if (layoutItem == null || (layoutItem is EmptySpace && !this.TreeNodeLayout.FullRowSelect))
         return this.DragDropHandler;
      else
         return tn.DragDropHandler;
   }

   protected override void OnDragOver(DragEventArgs drgevent)
   {
      if (drgevent == null)
         throw new ArgumentNullException("drgevent");

      Point location = this.PointToClient(new Point(drgevent.X, drgevent.Y));
      TreeNode tn = this.GetNodeAt(location);
      DragDropHandler dragDropHandler = this.getDragDropHandler(tn, location);

      if (prevDragTarget != null && prevDragTarget != tn)
      {
         prevDragTarget.RemoveStateFlag(TreeNodeStates.DropTarget);
         prevDragTarget = null;
      }

      if (dragDropHandler != null)
      {
         drgevent.Effect = dragDropHandler.GetDragDropEffect(drgevent.Data);

         if (tn != null && drgevent.Effect != TreeView.NoneDragDropEffects
                        && dragDropHandler != this.DragDropHandler)
         {
            tn.SetStateFlag(TreeNodeStates.DropTarget);
            prevDragTarget = tn;
         }
      }
      else
         drgevent.Effect = TreeView.NoneDragDropEffects;

      base.OnDragOver(drgevent);
   }

   protected override void OnDragDrop(DragEventArgs drgevent)
   {
      if (drgevent == null)
         throw new ArgumentNullException("drgevent");

      Point location = this.PointToClient(new Point(drgevent.X, drgevent.Y));
      TreeNode tn = this.GetNodeAt(location);
      DragDropHandler dragDropHandler = this.getDragDropHandler(tn, location);

      if (prevDragTarget != null)
      {
         prevDragTarget.RemoveStateFlag(TreeNodeStates.DropTarget);
         prevDragTarget = null;
      }

      if (dragDropHandler != null)
      {
         dragDropHandler.HandleDrop(drgevent.Data);
      }
      
      base.OnDragDrop(drgevent);
   }

   #endregion


   #region Selection

   protected HashSet<TreeNode> selectedNodes { get; private set; }
   public IEnumerable<TreeNode> SelectedNodes
   {
      get { return this.selectedNodes.ToList(); }
   }

   public TreeNode LastSelectedNode { get; private set; }
   public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

   internal void OnSelectionChanged()
   {
      if (this.SelectionChanged != null)
         this.SelectionChanged(this, new SelectionChangedEventArgs(this.selectedNodes));
   }

   public void SelectNode(TreeNode tn, Boolean select)
   {
      if (tn == null)
         return;

      if (select)
      {
         tn.State = TreeNodeStates.Selected;

         if (!this.IsSelectedNode(tn))
            this.selectedNodes.Add(tn);

         //TODO:  this.HighlightParentNodes(tn);

         this.LastSelectedNode = tn;
      }
      else
      {
         selectedNodes.Remove(tn);

         if (IsParentOfSelectedNode(tn, true))
            tn.State = TreeNodeStates.ParentOfSelected;
         else
            tn.State = TreeNodeStates.None;

         //TODO: this.RemoveParentHighlights(tn);
      }
   }

   public void SelectAllNodes(Boolean select)
   {
      if (select)
         this.SelectAllNodes(select, this.Nodes);
      else
      {
         List<TreeNode> selNodes = this.selectedNodes.ToList();
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

      return selectedNodes.Contains(tn);
   }

   public Boolean IsParentOfSelectedNode(TreeNode tn, Boolean includeChildren)
   {
      if (tn == null || this.IsSelectedNode(tn))
         return false;

      foreach (TreeNode sn in selectedNodes)
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

   private IComparer<TreeNode> nodeSorter;
   [Browsable(false)]
   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
   public IComparer<TreeNode> NodeSorter 
   {
      get { return this.nodeSorter; }
      set
      {
         this.nodeSorter = value;
         this.Sort();
      }
   }
   
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

   public void StartTimedSort(TreeNode tn)
   {
      this.AddToSortQueue(tn);
      this.StartTimedSort(true);
   }

   public void StartTimedSort(IEnumerable<TreeNode> tns)
   {
      this.AddToSortQueue(tns);
      this.StartTimedSort(true);
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
   private Boolean editTextBoxOpening;

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
      this.editTextBoxOpening = true;
      this.editTextBox = new TextBox();

      Rectangle bounds = layoutItem.GetBounds(tn);
      this.editTextBox.Location = bounds.Location;
      this.editTextBox.Size = new Size (Math.Max(100, bounds.Width), 18);
      this.editTextBox.Text = e.EditText;
      this.editTextBox.KeyDown += new KeyEventHandler(editTextBox_KeyDown);
      this.editTextBox.LostFocus += new EventHandler(editTextBox_LostFocus);

      this.editTextBox.Parent = this;
      this.editTextBox.Focus();
      this.editTextBox.SelectAll();
      
   }

   private void editTextBox_LostFocus(object sender, EventArgs e)
   {
      if (this.editTextBoxOpening)
      {
         this.editTextBox.Focus();
         this.editTextBoxOpening = false;
      }
      else
         this.EndNodeTextEdit(false);
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
