using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Controls.Tree;
using Outliner.Commands;
using Outliner.Scene;

namespace Outliner.Modes.Hierarchy
{
public class GroupDragDropHandler : IINodeDragDropHandler
{
   public GroupDragDropHandler(IMaxNodeWrapper data) : base(data) { }

   public override bool AllowDrag
   {
      get { return true; }
   }

   public override WinForms::DragDropEffects GetDragDropEffect(WinForms::IDataObject dragData)
   {
      if (HelperMethods.ShiftPressed)
         return base.GetDragDropEffect(dragData);
      else
      {
         if (this.IsValidDropTarget(dragData))
            return WinForms.DragDropEffects.Copy;
         else
            return TreeView.NoneDragDropEffects;
      }
   }

   public override void HandleDrop(WinForms::IDataObject dragData)
   {
      if (HelperMethods.ShiftPressed)
         base.HandleDrop(dragData);
      else
      {
         if (!this.IsValidDropTarget(dragData))
            return;

         IEnumerable<TreeNode> draggedNodes = DragDropHandler.GetNodesFromDataObject(dragData);
         if (draggedNodes == null)
            return;

         IEnumerable<IMaxNodeWrapper> nodes = HelperMethods.GetMaxNodes(draggedNodes);
         ChangeGroupCommand cmd = new ChangeGroupCommand(nodes, this.Data, true);
         cmd.Execute(true);
      }
   }
}
}
