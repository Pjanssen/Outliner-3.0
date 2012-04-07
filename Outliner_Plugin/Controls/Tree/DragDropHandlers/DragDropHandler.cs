using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.Tree.DragDropHandlers
{
public abstract class DragDropHandler
{
   public DragDropHandler(IMaxNodeWrapper data)
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



   protected IEnumerable<TreeNode> GetNodesFromDataObject(IDataObject dragData)
   {
      Type dataType = typeof(IEnumerable<TreeNode>);
      Boolean a = dragData.GetDataPresent(typeof(ICollection<TreeNode>));
      Boolean b = dragData.GetDataPresent(typeof(HashSet<TreeNode>));
      Boolean c = dragData.GetDataPresent(typeof(IEnumerable<TreeNode>));
      
      if (dragData.GetDataPresent(dataType))
         return dragData.GetData(dataType) as IEnumerable<TreeNode>;
      else
         return null;
   }


   /*
   public static DragDropHandler GetDragDropHandler(TreeView tree, OutlinerNode n)
   {
      if (n is OutlinerObject)
         return new ObjectDragDropHandler(tree, n);
      else if (n is OutlinerLayer)
         return new LayerDragDropHandler(tree, (OutlinerLayer)n);
      else if (n is SelectionSet)
         return new SelectionSetDragDropHandler(tree, (SelectionSet)n);

      return new NullDragDropHandler();
   }*/
}
}
