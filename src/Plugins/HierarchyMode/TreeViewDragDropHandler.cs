using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Modes.Hierarchy
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

      UnlinkNodesCommand cmd = new UnlinkNodesCommand( draggedNodes
                                                     , Resources.Command_Unlink);
      cmd.Execute();
      Viewports.Redraw();
   }
}
}
