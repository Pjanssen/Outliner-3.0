using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Modes;
using Outliner.Scene;

namespace Outliner.Controls.Tree
{
   /// <summary>
   /// Defines an abstract base class for drag &amp; drop operations on IMaxNodes.
   /// </summary>
   public abstract class MaxNodeDragDropHandler : IDragDropHandler
   {
      protected IMaxNode MaxNode { get; private set; }

      /// <summary>
      /// Initializes a new instance of the MaxNodeDragDropHandler.
      /// </summary>
      /// <param name="maxNode">The IMaxNode object the DragDropHandler is associated with.</param>
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

      /// <summary>
      /// Gets the DragDropEffects to be used when a node is dragged onto this handler.
      /// </summary>
      public virtual DragDropEffects DefaultDragDropEffect
      {
         get { return DragDropEffects.Move; }
      }

      /// <summary>
      /// Retrieves the collection of IMaxNodes from the IDataObject in a drag &amp; drop procedure.
      /// </summary>
      /// <param name="dragData">The IDataObject object containing the dragged IMaxNodes.</param>
      public static IEnumerable<IMaxNode> GetMaxNodesFromDragData(IDataObject dragData)
      {
         return TreeMode.GetMaxNodes(TreeView.GetTreeNodesFromDragData(dragData));
      }
   }
}
