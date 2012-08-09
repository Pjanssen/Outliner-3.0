using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.Tree
{
public abstract class DragDropHandler
{
   protected DragDropHandler(IMaxNodeWrapper data)
   {
      this.Data = data;
   }

   /// <summary>
   /// The data object this DragDropHandler is associated with.
   /// </summary>
   public IMaxNodeWrapper Data { get; private set; }

   /// <summary>
   /// Determines whether the node can be dragged.
   /// </summary>
   public abstract Boolean AllowDrag { get; }

   /// <summary>
   /// Returns true if the dragged data can be dropped onto this handler.
   /// </summary>
   public abstract Boolean IsValidDropTarget(IDataObject dragData);

   /// <summary>
   /// Returns the DragDropEffect for this node as a drop-target.
   /// </summary>
   public abstract DragDropEffects GetDragDropEffect(IDataObject dragData);

   /// <summary>
   /// Called when a selection of nodes is dropped onto this node.
   /// </summary>
   public abstract void HandleDrop(IDataObject dragData);



   protected static IEnumerable<TreeNode> GetNodesFromDataObject(IDataObject dragData)
   {
      if (dragData == null)
         throw new ArgumentNullException("dragData");

      Type dataType = typeof(IEnumerable<TreeNode>);
      
      if (dragData.GetDataPresent(dataType))
         return dragData.GetData(dataType) as IEnumerable<TreeNode>;
      else
         return null;
   }
}
}
