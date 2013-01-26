using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Commands;
using Outliner.Controls.Tree;
using Outliner.Scene;
using WinForms = System.Windows.Forms;

namespace Outliner.Modes.MaterialMode
{
   public class MaterialDragDropHandler : MaxNodeDragDropHandler
   {
      public MaterialDragDropHandler(IMaxNode material) : base(material)
      { }

      public override bool AllowDrag
      {
         get { return false; }
      }

      public override bool IsValidDropTarget(WinForms.IDataObject dragData)
      {
         IEnumerable<IMaxNode> nodes = GetMaxNodesFromDragData(dragData);
         return this.MaxNode.CanAddChildNodes(nodes);
      }

      public override WinForms.DragDropEffects GetDragDropEffect(WinForms.IDataObject dragData)
      {
         if (this.IsValidDropTarget(dragData))
            return WinForms.DragDropEffects.Copy;
         else
            return TreeView.NoneDragDropEffects;
      }

      public override void HandleDrop(WinForms.IDataObject dragData)
      {
         if (!this.IsValidDropTarget(dragData))
            return;

         IEnumerable<IMaxNode> nodes = GetMaxNodesFromDragData(dragData);
         AddNodesCommand cmd = new AddNodesCommand(this.MaxNode, nodes, Resources.Command_AssignMaterial);
         //MoveMaxNodeCommand cmd = new MoveMaxNodeCommand(nodes, this.MaxNode, Resources.Command_AssignMaterial);
         cmd.Execute(true);
      }
   }
}
