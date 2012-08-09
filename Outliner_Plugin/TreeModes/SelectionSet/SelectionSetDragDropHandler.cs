using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree.DragDropHandlers;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Commands;

namespace Outliner.TreeModes.SelectionSet
{
public class SelectionSetDragDropHandler : DragDropHandler
{
   public SelectionSetDragDropHandler(SelectionSetWrapper data) : base(data) { }

   public override bool AllowDrag
   {
      get { return false; }
   }

   public override bool IsValidDropTarget(WinForms::IDataObject dragData)
   {
      IEnumerable<TreeNode> draggedNodes = DragDropHandler.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return false;

      return this.Data.CanAddChildNodes(HelperMethods.GetMaxNodes(draggedNodes));
   }

   public override WinForms::DragDropEffects GetDragDropEffect(WinForms::IDataObject dragData)
   {
      if (this.IsValidDropTarget(dragData))
         return WinForms::DragDropEffects.Copy;
      else
         return TreeView.NoneDragDropEffects;
   }

   public override void HandleDrop(System.Windows.Forms.IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<TreeNode> draggedNodes = DragDropHandler.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return;

      IEnumerable<IMaxNodeWrapper> nodes = HelperMethods.GetMaxNodes(draggedNodes);
      SelectionSetWrapper selSet = (SelectionSetWrapper)this.Data;
      IEnumerable<IMaxNodeWrapper> newNodes = selSet.WrappedChildNodes.Union(nodes);
      ModifySelectionSetCommand cmd = new ModifySelectionSetCommand(selSet, newNodes.ToList());
      cmd.Execute(true);
   }
}
}
