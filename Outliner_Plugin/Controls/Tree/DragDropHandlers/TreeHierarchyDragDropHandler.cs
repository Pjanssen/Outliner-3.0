using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Tree.DragDropHandlers
{
public class TreeHierarchyDragDropHandler : DragDropHandler
{
   public TreeHierarchyDragDropHandler() : base(null) { }

   public override bool AllowDrag
   {
      get { return false; }
   }

   public override bool IsValidDropTarget(IDataObject dragData)
   {
      IEnumerable<TreeNode> draggedNodes = this.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return false;

      return HelperMethods.GetMaxNodes(draggedNodes).All(n => n is IINodeWrapper);
   }

   public override DragDropEffects GetDragDropEffect(IDataObject dragData)
   {
      if (this.IsValidDropTarget(dragData))
         return DragDropEffects.Move;
      else
         return TreeView.NoneDragDropEffects;
   }

   public override void HandleDrop(IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<TreeNode> draggedNodes = this.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return;

      LinkIINodeCommand cmd = new LinkIINodeCommand(HelperMethods.GetMaxNodes(draggedNodes), null);
      cmd.Execute(true);
   }
}
}
