using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Controls.Tree;

namespace Outliner.Modes.SelectionSet
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
         return false;
      }
   }
}
