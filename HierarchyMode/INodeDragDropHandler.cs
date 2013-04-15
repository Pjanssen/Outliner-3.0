using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;

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

      AddNodesCommand cmd = new AddNodesCommand( this.MaxNode
                                               , draggedNodes
                                               , Resources.Command_Link);
      cmd.Execute();
      Viewports.Redraw();
   }
}
}
