﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using PJanssen.Outliner.Controls.Tree.Layout;

namespace PJanssen.Outliner.Controls.Tree
{
/// <summary>
/// A general-purpose multiselect TreeView control, with customizable layout.
/// </summary>
public class TreeView : ScrollableControl
{
   protected HashSet<TreeNode> selectedNodes { get; private set; }
   private HashSet<TreeNode> autoExpandedNodes;

   private Int32 beginUpdateCalls = 0;
   private TreeViewUpdateFlags updating = TreeViewUpdateFlags.None;

   private TreeNodeLayout treeNodeLayout;
   private TreeViewColorScheme colors;
   private BorderStyle borderStyle = BorderStyle.FixedSingle;
   private SolidBrush brushBackground;

   private Boolean MouseEventHandledAtMouseDown;

   private Point dragStartPos;
   private Boolean isDragging;
   private TreeNode prevDragTarget;

   private IComparer<TreeNode> nodeSorter;
   private List<TreeNodeCollection> _sortQueue;
   private Timer _sortTimer;
   private Boolean _timedSortQueueOnly;

   /// <summary>
   /// Initializes a new instance of the TreeView control.
   /// </summary>
   public TreeView()
   {
      this.Root = new TreeNode(this, "root");
      this.Colors = new TreeViewColorScheme();
      this.selectedNodes = new HashSet<TreeNode>();
      this.autoExpandedNodes = new HashSet<TreeNode>();
      this.TreeNodeLayout = TreeNodeLayout.SimpleLayout;
      this.Settings = new TreeViewSettings();

      //Set double buffered user paint style.
      this.SetStyle(ControlStyles.UserPaint, true);
      this.DoubleBuffered = true;

      this.AutoScroll = true;
      this.AutoScrollMinSize = this.Size;
      this.AllowDrop = true;
   }

   protected override CreateParams CreateParams
   {
      get
      {
         const int WS_BORDER = 0x00800000;
         const int WS_EX_STATICEDGE = 0x00020000;
         CreateParams cp = base.CreateParams;
         //switch (this.BorderStyle)
         //{
         //   case BorderStyle.FixedSingle:
         //      cp.Style |= WS_BORDER;
         //      break;
         //   case BorderStyle.Fixed3D:
         //      cp.ExStyle |= WS_EX_STATICEDGE;
         //      break;
         //}
         return cp;
      }
   }

   protected override void Dispose(bool disposing)
   {
      if (this.brushBackground != null)
         this.brushBackground.Dispose();

      base.Dispose(disposing);
   }

   #region Properties

   [Browsable(false)]
   public TreeNode Root { get; private set; }

   /// <summary>
   /// Gets the root TreeNodes of the TreeView.
   /// </summary>
   [Browsable(false)]
   public TreeNodeCollection Nodes
   {
      get { return this.Root.Nodes; }
   }

   /// <summary>
   /// Gets or sets the TreeView's settings object.
   /// </summary>
   public TreeViewSettings Settings { get; set; }

   /// <summary>
   /// Gets or sets the border style of the control.
   /// </summary>
   public BorderStyle BorderStyle
   {
      get { return this.borderStyle; }
      set
      {
         this.borderStyle = value;
         this.Invalidate();
      }
   }

   #endregion


   #region Helpers

   /// <summary>
   /// Gets the TreeNode at the given location.
   /// </summary>
   /// <param name="location">The point relative to the TreeView control to find a TreeNode at.</param>
   /// <returns>A TreeNode if one exists at the given location, null otherwise.</returns>
   public TreeNode GetNodeAt(Point location)
   {
      return this.GetNodeAt(location.X, location.Y);
   }

   /// <summary>
   /// Gets the TreeNode at the given location.
   /// </summary>
   /// <param name="x">The x coordinate relative to the TreeView control to find a TreeNode at.</param>
   /// <param name="y">The y coordinate relative to the TreeView control to find a TreeNode at.</param>
   /// <returns>A TreeNode if one exists at the given location, null otherwise.</returns>
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

   #endregion


   #region Colors

   /// <summary>
   /// Gets or sets the color scheme for the TreeView.
   /// </summary>
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

   /// <summary>
   /// Gets or sets the background color of the TreeView.
   /// </summary>
   public override Color BackColor
   {
      get { return this.Colors.Background; }
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
         color = Outliner.MaxUtils.Colors.SelectContrastingColor( this.GetNodeBackColor(tn, highlight)
                                                                      , this.Colors.ForegroundLight.Color
                                                                      , this.Colors.ForegroundDark.Color);
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
      Int32 itemHeight = this.TreeNodeLayout.ItemHeight;
      return (tn.AbsoluteBounds.Y % (2 * itemHeight)) >= itemHeight;
   }

   #endregion


   #region Paint

   /// <summary>
   /// Gets or sets the layout of the TreeView.
   /// </summary>
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

   private void DrawBorder(Graphics graphics)
   {
      Rectangle r = new Rectangle(-1, 0, this.Width + 1, this.Height);
      //ControlPaint.DrawBorder(graphics, r, Color.FromArgb(126, 106, 37), ButtonBorderStyle.Solid);
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
      this.DrawBorder(e.Graphics);
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

      this.DrawBorder(e.Graphics);
   }

   #endregion
   

   #region Update

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

      if ((e.Button & this.Settings.DragDropMouseButton) == this.Settings.DragDropMouseButton)
         this.dragStartPos = e.Location;

      TreeNode tn = this.GetNodeAt(e.Location);
      if (tn != null)
         this.MouseEventHandledAtMouseDown = this.TreeNodeLayout.HandleMouseDown(e, tn);
      else
         this.MouseEventHandledAtMouseDown = false;

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
      else if (!this.MouseEventHandledAtMouseDown && !ControlHelpers.ControlPressed
                                                  && !ControlHelpers.ShiftPressed
                                                  && !ControlHelpers.AltPressed)
      {
         if ((e.Button & MouseButtons.Right) != MouseButtons.Right)
         {
            int selCount = this.selectedNodes.Count;
            this.SelectAllNodes(false);
            if (selCount > 0)
               this.OnSelectionChanged();
         }
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

      Boolean isDragDropMouseButton = (e.Button & this.Settings.DragDropMouseButton) == this.Settings.DragDropMouseButton;
      if (!isDragDropMouseButton)
      {
         this.isDragging = false;
         this.dragStartPos = Point.Empty;
      }

      TreeNode tn = this.GetNodeAt(e.Location);

      //Start dragging.
      if (tn != null && !this.isDragging
          && isDragDropMouseButton
          && ControlHelpers.Distance(e.Location, this.dragStartPos) > 5
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

   private const DragDropEffects AllowedDragDropEffects = DragDropEffects.Copy
                                                        | DragDropEffects.Link
                                                        | DragDropEffects.Move;
   
   // Use DragDropEffects.Scroll instead of DragDropEffects.None, 
   // otherwise the OnDragDrop event won't be raised.
   public const DragDropEffects NoneDragDropEffects = DragDropEffects.Scroll;

   /// <summary>
   /// The DragDropHandler for the TreeView.
   /// </summary>
   [Browsable(false)]
   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
   public IDragDropHandler DragDropHandler { get; set; }

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

   private IDragDropHandler getDragDropHandler(TreeNode tn, Point location)
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
      IDragDropHandler dragDropHandler = this.getDragDropHandler(tn, location);

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
      IDragDropHandler dragDropHandler = this.getDragDropHandler(tn, location);

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

   /// <summary>
   /// Extracts the TreeNodes being dragged from an IDataObject.
   /// </summary>
   public static IEnumerable<TreeNode> GetTreeNodesFromDragData(IDataObject dragData)
   {
      Throw.IfNull(dragData, "dragData");

      Type dataType = typeof(IEnumerable<TreeNode>);

      if (dragData.GetDataPresent(dataType))
         return dragData.GetData(dataType) as IEnumerable<TreeNode>;
      else
         return Enumerable.Empty<TreeNode>();
   }

   #endregion


   #region Selection

   /// <summary>
   /// Gets the selected TreeNodes.
   /// </summary>
   [Browsable(false)]
   public IEnumerable<TreeNode> SelectedNodes
   {
      get { return this.selectedNodes; }
   }

   /// <summary>
   /// Gets the last selected TreeNode.
   /// </summary>
   [Browsable(false)]
   public TreeNode LastSelectedNode { get; private set; }

   /// <summary>
   /// Occurs when the selection has changed.
   /// </summary>
   public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

   internal void OnSelectionChanged()
   {
      if (this.SelectionChanged != null)
         this.SelectionChanged(this, new SelectionChangedEventArgs(this.selectedNodes));
   }

   /// <summary>
   /// Selects or deselects a TreeNode.
   /// </summary>
   /// <param name="tn">The TreeNode to select or deselect.</param>
   /// <param name="select">If true, the TreeNode will be selected, if false, it will be deselected.</param>
   public void SelectNode(TreeNode tn, Boolean select)
   {
      if (tn == null)
         return;

      this.BeginUpdate(TreeViewUpdateFlags.Redraw | TreeViewUpdateFlags.Scrollbars);

      if (select)
      {
         if (!tn.IsSelected)
         {
            if (!this.Settings.MultiSelect)
               this.SelectAllNodes(false);

            this.selectedNodes.Add(tn);

            tn.SetStateFlag(TreeNodeStates.Selected);

            this.LastSelectedNode = tn;

            if (this.Settings.AutoExpandSelectionParents)
               this.AutoExpandParents(tn);

            if (this.Settings.ScrollToSelection && !this.IsSelectionInView())
               this.ScrollTreeNodeIntoView(tn);
         }
      }
      else
      {
         selectedNodes.Remove(tn);

         tn.RemoveStateFlag(TreeNodeStates.Selected);

         if (this.Settings.CollapseAutoExpandedParents)
            this.CollapseAutoExpandedParents(tn);
      }

      this.EndUpdate();
   }

   /// <summary>
   /// Selects or deselects a collection of TreeNodes.
   /// </summary>
   /// <param name="tns">The TreeNodes to select or deselect.</param>
   /// <param name="select">If true, the TreeNodes will be selected, if false, they will be deselected.</param>
   public void SelectNodes(IEnumerable<TreeNode> tns, Boolean select)
   {
      foreach (TreeNode tn in tns)
      {
         this.SelectNode(tn, select);
      }
   }

   /// <summary>
   /// Selects or deselects all TreeNodes in the TreeView.
   /// </summary>
   /// <param name="select">If true, the TreeNodes will be selected, if false, they will be deselected.</param>
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

   /// <summary>
   /// Selects all visible TreeNodes between two TreeNodes.
   /// </summary>
   /// <param name="startNode">The TreeNode to start selecting from.</param>
   /// <param name="endNode">The TreeNode to stop selecting at.</param>
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

   /// <summary>
   /// Expands the parents of the given TreeNode, and registers them to be closed automatically
   /// when the selection changes.
   /// </summary>
   public void AutoExpandParents(TreeNode tn)
   {
      Throw.IfNull(tn, "tn");
      
      TreeNode parent = tn.Parent;
      while (parent != null)
      {
         if (!parent.IsExpanded)
         {
            parent.IsExpanded = true;
            this.autoExpandedNodes.Add(parent);
         }

         parent = parent.Parent;
      }
   }

   private void CollapseAutoExpandedParents(TreeNode tn)
   {
      Throw.IfNull(tn, "tn");

      TreeNode parent = tn.Parent;
      while (parent != null)
      {
         if (this.autoExpandedNodes.Contains(parent) && !parent.IsParentOfSelectedNode)
         {
            parent.IsExpanded = false;
            this.autoExpandedNodes.Remove(parent);
         }

         parent = parent.Parent;
      }
   }

   /// <summary>
   /// Tests if any of the currently selected TreeNodes are scrolled into view.
   /// </summary>
   public Boolean IsSelectionInView()
   {
      Rectangle treeBounds = this.Bounds;
      foreach (TreeNode selTn in this.SelectedNodes)
      {
         if (treeBounds.Contains(selTn.Bounds))
            return true;
      }

      return false;
   }

   /// <summary>
   /// Adjusts the vertical scrollbar so that the given TreeNode is scrolled into view.
   /// </summary>
   /// <param name="tn"></param>
   public void ScrollTreeNodeIntoView(TreeNode tn)
   {
      Throw.IfNull(tn, "tn");

      int y = tn.Bounds.Y;
      if (y < 0 || y > this.Height)
      {
         this.AutoScrollPosition = new Point(this.AutoScrollPosition.X, y);
      }
   }

   #endregion


   #region Sort

   /// <summary>
   /// Gets or sets the NodeSorter object used to sort the TreeNodes in the tree.
   /// </summary>
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

   /// <summary>
   /// Adds the given node to the sort queue and then sorts the TreeView after a certain 
   /// amount of time has expired without this method being called.
   /// </summary>
   public void StartTimedSort(TreeNode tn)
   {
      this.AddToSortQueue(tn);
      this.StartTimedSort(true);
   }

   /// <summary>
   /// Adds the given nodes to the sort queue and then sorts the TreeView after a certain 
   /// amount of time has expired without this method being called.
   /// </summary>
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

   /// <summary>
   /// Initiates the TreeNode edit text procedure.
   /// </summary>
   /// <param name="tn">The TreeNode of which the text will be edited.</param>
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

      if (this.editingTreeNode != null)
         this.EndNodeTextEdit(true);

      BeforeNodeTextEditEventArgs e = new BeforeNodeTextEditEventArgs(tn);
      this.OnBeforeNodeTextEdit(e);

      if (e.Cancel)
         return;

      this.editingTreeNode = tn;
      this.editTextBoxOpening = true;
      this.editTextBox = new TextBox();

      Rectangle bounds = layoutItem.GetBounds(tn);
      int itemIndex = layoutItem.Layout.LayoutItems.IndexOf(layoutItem);
      if (itemIndex < layoutItem.Layout.LayoutItems.Count - 1)
      {
         TreeNodeLayoutItem nextItem = layoutItem.Layout.LayoutItems[itemIndex + 1];
         if (nextItem is EmptySpace)
         {
            bounds.Width += nextItem.GetWidth(tn);
         }
      }
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
      //This appears to be more reliable than e.KeyCode.HasFlag(Keys.Enter),
      //which also returns true for 'o' ?!
      if (e.KeyValue == 13)
         this.EndNodeTextEdit(false);
      else if (e.KeyValue == 27)
         this.EndNodeTextEdit(true);

      e.Handled = true;
   }

   /// <summary>
   /// Stops the TreeNode edit text procedure.
   /// </summary>
   /// <param name="cancel">Indicates whether the operation should be cancelled or committed.</param>
   public void EndNodeTextEdit(Boolean cancel)
   {
      String oldText = null;
      String newText = null;
      TreeNode editingTreeNode = this.editingTreeNode;

      if (!cancel && this.editTextBox != null
                  && this.editingTreeNode != null
                  && !String.IsNullOrEmpty(this.editTextBox.Text)
                  && this.editTextBox.Text != this.editingTreeNode.Text)
      {
         oldText = this.editingTreeNode.Text;
         newText = this.editTextBox.Text;
         editingTreeNode.Text = this.editTextBox.Text;
      }

      this.editTextBox.KeyDown -= editTextBox_KeyDown;
      this.editTextBox.LostFocus -= editTextBox_LostFocus;

      this.Controls.Remove(this.editTextBox);
      this.editTextBox.Dispose();
      this.editTextBox = null;
      this.editingTreeNode = null;

      this.OnAfterNodeTextEdit(new AfterNodeTextEditEventArgs(editingTreeNode, cancel, oldText, newText));
   }

   #endregion
}
}
