using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.LayerTools;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Modes.Layer
{
   internal class TreeViewDragDropHandler : IDragDropHandler
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
         cmd.Execute();
         Viewports.Redraw();
      }
   }
}
