using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.Controls.Tree;

namespace Outliner.Modes.Hierarchy
{
public class TreeViewDragDropHandler : IDragDropHandler
{
   public TreeViewDragDropHandler() { }

   public bool AllowDrag
   {
      get { return false; }
   }

   public bool IsValidDropTarget(WinForms::IDataObject dragData)
   {
      IEnumerable<IMaxNode> draggedNodes = MaxNodeDragDropHandler.GetMaxNodesFromDragData(dragData);

      return draggedNodes.All(n => n is INodeWrapper);
   }

   public WinForms::DragDropEffects GetDragDropEffect(WinForms::IDataObject dragData)
   {
      if (this.IsValidDropTarget(dragData))
         return WinForms::DragDropEffects.Move;
      else
         return TreeView.NoneDragDropEffects;
   }

   public void HandleDrop(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<IMaxNode> draggedNodes = MaxNodeDragDropHandler.GetMaxNodesFromDragData(dragData);

      MoveMaxNodeCommand cmd = new MoveMaxNodeCommand( draggedNodes
                                                     , null
                                                     , Resources.Command_Link
                                                     , Resources.Command_Unlink);
      cmd.Execute(true);
   }
}
}
