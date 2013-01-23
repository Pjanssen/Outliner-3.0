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
public class INodeDragDropHandler : MaxNodeDragDropHandler
{
   public INodeDragDropHandler(IMaxNode node) : base(node) { }

   public override bool AllowDrag
   {
      get { return true; }
   }

   public override bool IsValidDropTarget(WinForms::IDataObject dragData)
   {
      return this.MaxNode.CanAddChildNodes(GetMaxNodesFromDragData(dragData));
   }

   public override WinForms.DragDropEffects DefaultDragDropEffect
   {
      get { return WinForms.DragDropEffects.Link; }
   }

   public override void HandleDrop(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<IMaxNode> draggedNodes = GetMaxNodesFromDragData(dragData);

      MoveMaxNodeCommand cmd = new MoveMaxNodeCommand( draggedNodes
                                                     , this.MaxNode
                                                     , Resources.Command_Link
                                                     , Resources.Command_Unlink);
      cmd.Execute(true);
   }
}
}
