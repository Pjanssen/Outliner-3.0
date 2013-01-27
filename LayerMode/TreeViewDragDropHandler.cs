using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Outliner.LayerTools;
using Outliner.Commands;

namespace Outliner.Modes.Layer
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
         return draggedNodes.Any(n => n is ILayerWrapper);
      }

      public WinForms::DragDropEffects GetDragDropEffect(WinForms::IDataObject dragData)
      {
         if (IsValidDropTarget(dragData))
            return WinForms::DragDropEffects.Move;
         else
            return TreeView.NoneDragDropEffects;
      }

      public void HandleDrop(WinForms::IDataObject dragData)
      {
         if (!IsValidDropTarget(dragData))
            return;

         IEnumerable<IMaxNode> draggedNodes = MaxNodeDragDropHandler.GetMaxNodesFromDragData(dragData);
         UnlinkNodesCommand cmd = new UnlinkNodesCommand( draggedNodes.Where(n => n is ILayerWrapper)
                                                        , Resources.Command_UnlinkLayer);
         cmd.Execute(true);
      }
   }
}
