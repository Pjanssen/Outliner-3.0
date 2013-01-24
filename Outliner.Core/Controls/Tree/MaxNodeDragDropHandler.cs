using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.Tree
{
   public abstract class MaxNodeDragDropHandler : IDragDropHandler
   {
      protected IMaxNode MaxNode { get; private set; }

      public MaxNodeDragDropHandler(IMaxNode maxNode)
      {
         this.MaxNode = maxNode;
      }


      
      public abstract bool AllowDrag { get; }

      public abstract bool IsValidDropTarget(IDataObject dragData);


      public virtual DragDropEffects GetDragDropEffect(IDataObject dragData)
      {
         if (this.IsValidDropTarget(dragData))
            return this.DefaultDragDropEffect;
         else
            return TreeView.NoneDragDropEffects;
      }

      public virtual void HandleDrop(IDataObject dragData) { }



      public virtual DragDropEffects DefaultDragDropEffect
      {
         get { return DragDropEffects.Move; }
      }

      public static IEnumerable<IMaxNode> GetMaxNodesFromDragData(IDataObject dragData)
      {
         return HelperMethods.GetMaxNodes(TreeView.GetTreeNodesFromDragData(dragData));
      }
   }
}
