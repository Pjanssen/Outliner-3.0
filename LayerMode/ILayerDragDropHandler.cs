using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.Controls.Tree;

namespace Outliner.Modes.Layer
{
public class ILayerDragDropHandler : DragDropHandler
{
   private ILayerWrapper layer;
   public ILayerDragDropHandler(ILayerWrapper data)
      : base(data)
   {
      this.layer = data;
   }

   public override bool AllowDrag
   {
      get { return !this.layer.IsDefault; }
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

   public override void HandleDrop(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<TreeNode> draggedNodes = DragDropHandler.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return;

      MoveMaxNodeCommand cmd = new MoveMaxNodeCommand( HelperMethods.GetMaxNodes(draggedNodes)
                                                     , this.Data
                                                     , Resources.Command_AddToLayer
                                                     , Resources.Command_UnlinkLayer);
      cmd.Execute(true);
   }
}
}
