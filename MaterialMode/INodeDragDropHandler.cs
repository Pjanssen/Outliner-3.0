using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Controls.Tree;
using Outliner.Scene;

namespace Outliner.Modes.MaterialMode
{
   public class INodeDragDropHandler : MaxNodeDragDropHandler
   {
      public INodeDragDropHandler(INodeWrapper node) : base(node) { }

      public override bool AllowDrag
      {
         get { return true; }
      }

      public override bool IsValidDropTarget(IDataObject dragData)
      {
         return false;
      }
   }
}
