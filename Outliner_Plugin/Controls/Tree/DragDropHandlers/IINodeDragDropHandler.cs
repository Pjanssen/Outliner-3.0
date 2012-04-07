using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Tree.DragDropHandlers
{
public class IINodeDragDropHandler : DragDropHandler
{
   public IINodeDragDropHandler(IMaxNodeWrapper data) : base(data) { }

   public override bool AllowDrag
   {
      get { return true; }
   }

   public override bool IsValidDropTarget(IDataObject dragData)
   {
      IEnumerable<TreeNode> draggedNodes = this.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return false;

      return this.Data.CanAddChildNodes(draggedNodes.Select(HelperMethods.GetMaxNode));

      /*
      foreach (TreeNode tn in draggedNodes)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (!(node is IINodeWrapper))
            return false;

         //TODO verify that this works across treeviews (different wrappers for same underlying node!)
         if (node.WrappedNode == this.Data.WrappedNode)
            return false;
      }

      return true;*/
   }

   public override DragDropEffects GetDragDropEffect(IDataObject dragData)
   {
      if (this.IsValidDropTarget(dragData))
         return DragDropEffects.Link;
      else
         return DragDropEffects.None;
   }

   public override void HandleDrop(IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<TreeNode> draggedNodes = this.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return;

      LinkIINodeCommand cmd = new LinkIINodeCommand(draggedNodes.Select(HelperMethods.GetMaxNode), this.Data);
      cmd.Execute(true);
   }
}
}
